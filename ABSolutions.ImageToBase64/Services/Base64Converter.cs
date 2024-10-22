using ABSolutions.ImageToBase64.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ABSolutions.ImageToBase64.Services;

public class Base64Converter : IBase64Converter
{
    private readonly bool _accessLocalFiles;
    private readonly IBase64Cache _base64Cache;

    private readonly Dictionary<string, object> _baseLogContexts = new()
    {
        {"ClassName", nameof(Base64Converter)}
    };

    private readonly Base64ConverterConfiguration _configuration;
    private readonly string _fileBaseDirectory = "/";

    private readonly Uri _httpBaseAddress = new("http://localhost/");

    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TimeSpan _httpTimeout = TimeSpan.FromSeconds(30);
    private readonly ILogger<Base64Converter>? _logger;

    // ReSharper disable once ConvertToPrimaryConstructor
    public Base64Converter(IOptions<Base64ConverterConfiguration> configuration, IHttpClientFactory httpClientFactory,
        ILogger<Base64Converter>? logger, IBase64Cache base64Cache)
    {
        _configuration = configuration.Value;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
        _base64Cache = base64Cache;

        var logContexts = new Dictionary<string, object>
        {
            {"MethodName", "ctor"}
        };
        using (_logger?.BeginScope(logContexts.Concat(_baseLogContexts)))
        {
            // construct base URI
            _logger?.LogDebug("Upstream image asset source configured as: {UpstreamImageAssetBaseUri}",
                _configuration.UpstreamImageAssetBaseUri);
            var configuredBaseAddress = _configuration.UpstreamImageAssetBaseUri.Trim().TrimEnd('/', '\\') + "/";
            if (configuredBaseAddress.StartsWith("file://"))
            {
                _logger?.LogInformation(
                    "Upstream image asset source ({UpstreamImageAssetBaseUri}) is a file URI which may not be available to clients or when running on a remote host",
                    configuredBaseAddress);
                _fileBaseDirectory =
                    Path.Combine(configuredBaseAddress.Trim().Replace("file://", "").Replace('\\', '/').Split('/'));
                if (!Directory.Exists(_fileBaseDirectory))
                {
                    _logger?.LogError(
                        "Upstream image asset source ({UpstreamImageAssetBaseUri}) is not a valid directory",
                        _fileBaseDirectory);
                    throw new DirectoryNotFoundException(
                        $"Upstream image asset source ({configuredBaseAddress}) is not a valid directory");
                }

                _accessLocalFiles = true;
                _logger?.LogInformation("Upstream image asset source: {UpstreamImageAssetBaseUri}", _fileBaseDirectory);
            }
            else
            {
                if (!Uri.IsWellFormedUriString(configuredBaseAddress, UriKind.Absolute))
                {
                    _logger?.LogError(
                        "Upstream image asset source ({UpstreamImageAssetBaseUri}) is not a valid absolute URI",
                        configuredBaseAddress);
                    throw new UriFormatException(
                        $"Upstream image asset source ({configuredBaseAddress}) is not a valid absolute URI");
                }

                _httpBaseAddress = new Uri(configuredBaseAddress);
                _logger?.LogInformation("Upstream image asset source: {UpstreamImageAssetBaseUri}", _httpBaseAddress);

                // construct timeout
                _httpTimeout = TimeSpan.FromSeconds(_configuration.UpstreamImageRetrievalTimeoutSeconds);
                _logger?.LogInformation(
                    "Upstream image retrieval timeout: {UpstreamImageRetrievalTimeoutSeconds} seconds",
                    _httpTimeout);
            }
        }
    }

    public async Task<Base64Result> GetImageAsBase64Async(string? filename, bool? useCache = null,
        bool? noExpiry = null,
        string loggingCorrelationValue = "", CancellationToken cancellationToken = default)
    {
        var logContexts = new Dictionary<string, object>
        {
            {"MethodName", nameof(GetImageAsBase64Async)}
        };
        if (!string.IsNullOrWhiteSpace(_configuration.LoggingCorrelationIdentifier) &&
            !string.IsNullOrWhiteSpace(loggingCorrelationValue))
            logContexts.Add(_configuration.LoggingCorrelationIdentifier, loggingCorrelationValue);

        using (_logger?.BeginScope(logContexts.Concat(_baseLogContexts)))
        {
            // set configuration options
            var cache = useCache ?? _configuration.EnableBase64Cache;
            var expiry = noExpiry ?? _configuration.NoExpiry;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                _logger?.LogDebug("Retrieving image {Filename} as Base64 string",
                    filename ?? "<filename not specified>");

                var (isValidFilename, fileExtension, filenameErrorMessage) = ValidateFilename(filename);
                if (!isValidFilename)
                {
                    _logger?.LogWarning("Filename is invalid ({FilenameValidationMessage}), using default image",
                        filenameErrorMessage);
                    return new Base64Result();
                }

                var responseContent = cache
                    ? (await GetCachedBase64ObjectAsync(filename!, loggingCorrelationValue))?.Base64String ?? null
                    : null;
                responseContent ??= _accessLocalFiles switch
                {
                    true => await GetImageFromLocalFileAsync(filename!, loggingCorrelationValue, cancellationToken),
                    false => await GetImageFromUpstreamAsync(filename!, loggingCorrelationValue, cancellationToken)
                };

                if (responseContent is null)
                {
                    _logger?.LogWarning("No image asset found for {Filename}, using default image", filename);
                    return new Base64Result();
                }

                if (cache)
                    await UpdateCachedBase64ObjectAsync(filename!, responseContent, expiry, loggingCorrelationValue);

                return new Base64Result(responseContent, fileExtension);
            }
            catch (OperationCanceledException exception) when (cancellationToken.IsCancellationRequested)
            {
                _logger?.LogWarning(exception,
                    "Upstream image {Filename} from {UpstreamBaseUri} request was cancelled, sending default image",
                    filename ?? "<filename not specified>", _httpBaseAddress);
                return new Base64Result();
            }
            catch (OperationCanceledException exception) when
                (exception.InnerException is TimeoutException timeoutException)
            {
                _logger?.LogWarning(timeoutException,
                    "Upstream image {Filename} from {UpstreamBaseUri} request timed out, sending default image",
                    filename ?? "<filename not specified>", _httpBaseAddress);
                return new Base64Result();
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case FileNotFoundException:
                        _logger?.LogWarning(e, "Local file {Filename} not found, sending default image",
                            Path.Combine(_fileBaseDirectory, filename ?? "<filename not specified>"));
                        break;
                    case UnauthorizedAccessException:
                        _logger?.LogError(e, "Access to {Filename} is not permitted, sending default image",
                            Path.Combine(_fileBaseDirectory, filename ?? "<filename not specified>"));
                        break;
                    default:
                        _logger?.LogError(e, "Error retrieving upstream asset {Filename}, sending default image",
                            filename ?? "<filename not specified>");
                        break;
                }

                return new Base64Result();
            }
        }
    }

    /// <summary>
    ///     Validate the filename to ensure it is not null or empty and has a file extension.
    /// </summary>
    /// <param name="filename">Filename to validate.</param>
    /// <returns>True if valid, False if invalid.</returns>
    private static (bool, string, string) ValidateFilename(string? filename)
    {
        if (string.IsNullOrWhiteSpace(filename))
            return (false, string.Empty, "no filename specified");

        var fileExtension = Path.GetExtension(filename).TrimStart('.');
        if (string.IsNullOrEmpty(fileExtension))
            return (false, fileExtension, "filename has no extension");

        return (true, string.Empty, string.Empty);
    }

    /// <summary>
    ///     Retrieve an image asset from an upstream source and convert it to a Base64 string representation.
    /// </summary>
    /// <param name="filename">Filename to retrieve from the base URL.</param>
    /// <param name="loggingCorrelationValue">Value to use for logging correlation. Default: empty string.</param>
    /// <param name="cancellationToken">Cancellation token. Default: None.</param>
    /// <returns>
    ///     Base64 string representation of the image file data or null if any errors encountered or the task is
    ///     cancelled.
    /// </returns>
    private async ValueTask<string?> GetImageFromUpstreamAsync(string filename, string loggingCorrelationValue = "",
        CancellationToken cancellationToken = default)
    {
        var logContexts = new Dictionary<string, object>
        {
            {"MethodName", nameof(GetImageFromUpstreamAsync)}
        };
        if (!string.IsNullOrWhiteSpace(_configuration.LoggingCorrelationIdentifier) &&
            !string.IsNullOrWhiteSpace(loggingCorrelationValue))
            logContexts.Add(_configuration.LoggingCorrelationIdentifier, loggingCorrelationValue);

        using (_logger?.BeginScope(logContexts.Concat(_baseLogContexts)))
        {
            _logger?.LogDebug("Requesting {Filename} from {UpstreamAssetSource}", filename, _httpBaseAddress);
            try
            {
                var httpClient = _httpClientFactory.CreateClient(_configuration.HttpClientName);
                httpClient.BaseAddress = _httpBaseAddress;
                httpClient.Timeout = _httpTimeout;

                using var httpRequest = new HttpRequestMessage(HttpMethod.Get, filename);
                httpRequest.Headers.Add("Accept", "image/*");
                var httpResponse = await httpClient.SendAsync(httpRequest, cancellationToken);

                if (httpResponse.IsSuccessStatusCode)
                {
                    var fileContents = await httpResponse.Content.ReadAsByteArrayAsync(cancellationToken);
                    if (fileContents.Length != 0) return Convert.ToBase64String(fileContents);
                    _logger?.LogWarning("Upstream asset {Filename} contains no data", filename);
                    return null;
                }

                _logger?.LogWarning(
                    "Upstream asset {Filename} from {UpstreamAssetSource} not found or not available: [{HttpStatusCode}] {HttpMessage}",
                    filename, _httpBaseAddress, httpResponse.StatusCode, httpResponse.ReasonPhrase);
                return null;
            }
            catch (OperationCanceledException exception) when (cancellationToken.IsCancellationRequested)
            {
                _logger?.LogDebug(exception, "Upstream asset {Filename} request was cancelled: {ExceptionMessage}",
                    filename,
                    exception.Message);
                return null;
            }
            catch (OperationCanceledException exception) when
                (exception.InnerException is TimeoutException timeoutException)
            {
                _logger?.LogWarning(timeoutException,
                    "Operation timed-out while retrieving upstream asset {Filename} from {UpstreamAssetSource}: {ExceptionMessage}",
                    filename, _httpBaseAddress, timeoutException.Message);
                return null;
            }
            catch (Exception e)
            {
                _logger?.LogError(e,
                    "Error while retrieving upstream asset {Filename} from {UpstreamAssetSource}: {ExceptionMessage}",
                    filename,
                    _httpBaseAddress, e.Message);
                return null;
            }
        }
    }

    /// <summary>
    ///     Retrieve an image asset from the local file system and convert it to a Base64 string representation.
    /// </summary>
    /// <param name="filename">Filename within the base directory to retrieve.</param>
    /// <param name="loggingCorrelationValue">Value to use for logging correlation. Default: empty string.</param>
    /// <param name="cancellationToken">Cancellation token. Default: None.</param>
    /// <returns>
    ///     Base64 string representation of the image file data or null if any errors encountered or the task is
    ///     cancelled.
    /// </returns>
    private async ValueTask<string?> GetImageFromLocalFileAsync(string filename, string loggingCorrelationValue = "",
        CancellationToken cancellationToken = default)
    {
        var logContexts = new Dictionary<string, object>
        {
            {"MethodName", nameof(GetImageFromLocalFileAsync)}
        };
        if (!string.IsNullOrWhiteSpace(_configuration.LoggingCorrelationIdentifier) &&
            !string.IsNullOrWhiteSpace(loggingCorrelationValue))
            logContexts.Add(_configuration.LoggingCorrelationIdentifier, loggingCorrelationValue);

        using (_logger?.BeginScope(logContexts.Concat(_baseLogContexts)))
        {
            _logger?.LogDebug("Requesting {Filename} from local file system at {UpstreamAssetSource}", filename,
                _fileBaseDirectory);
            try
            {
                var fileContents =
                    await File.ReadAllBytesAsync(Path.Combine(_fileBaseDirectory, filename), cancellationToken);
                if (fileContents.Length != 0) return Convert.ToBase64String(fileContents);
                _logger?.LogWarning("Upstream asset {Filename} contains no data", filename);
                return null;
            }
            catch (Exception e)
            {
                switch (e)
                {
                    case OperationCanceledException:
                        _logger?.LogDebug(e,
                            "Upstream asset {Filename} from {UpstreamAssetSource} request was cancelled",
                            filename, _fileBaseDirectory);
                        break;
                    case FileNotFoundException:
                        _logger?.LogWarning(e, "Local file {Filename} from {UpstreamAssetSource} not found",
                            filename, _fileBaseDirectory);
                        break;
                    case UnauthorizedAccessException:
                        _logger?.LogError(e, "Access to {Filename} from {UpstreamAssetSource} is not permitted",
                            filename, _fileBaseDirectory);
                        break;
                    default:
                        _logger?.LogError(e, "Error reading upstream asset {Filename}from {UpstreamAssetSource} ",
                            filename, _fileBaseDirectory);
                        break;
                }

                return null;
            }
        }
    }

    /// <summary>
    ///     Retrieves a Base64CachedObject from the cache.
    /// </summary>
    /// <param name="filename">Filename identifier to search for in the cache.</param>
    /// <param name="loggingCorrelationValue">Value to use for log correlation. Default: empty string.</param>
    /// <returns>Base64CachedObject if found, null otherwise</returns>
    private async ValueTask<Base64CachedObject?> GetCachedBase64ObjectAsync(string filename,
        string loggingCorrelationValue = "")
    {
        var logContexts = new Dictionary<string, object>
        {
            {"MethodName", nameof(GetCachedBase64ObjectAsync)}
        };
        if (!string.IsNullOrWhiteSpace(_configuration.LoggingCorrelationIdentifier) &&
            !string.IsNullOrWhiteSpace(loggingCorrelationValue))
            logContexts.Add(_configuration.LoggingCorrelationIdentifier, loggingCorrelationValue);

        using (_logger?.BeginScope(logContexts.Concat(_baseLogContexts)))
        {
            _logger?.LogDebug("Checking cache for Base64 string corresponding to {Filename}", filename);
            var cachedBase64String = await _base64Cache.GetCachedBase64(filename);
            _logger?.LogDebug("Received {CachedSvg} from cache", cachedBase64String?.ToString() ?? "<null>");
            if (cachedBase64String is not null &&
                (cachedBase64String.Expiry is null || cachedBase64String.Expiry > DateTime.UtcNow))
            {
                _logger?.LogDebug("Returning Base64 string for {Filename} from cache", filename);
                return cachedBase64String;
            }

            _logger?.LogDebug("No Base64 string found in cache for {Filename} or cached entry is expired", filename);
            return null;
        }
    }

    /// <summary>
    ///     Update or create a Base64CachedObject in the cache.
    /// </summary>
    /// <param name="filename">Filename identifier for this Base64 string.</param>
    /// <param name="base64">Base64 string string to store in the cache.</param>
    /// <param name="noExpiry">If true, DO NOT set an expiry time for this cache entry. Default: False.</param>
    /// <param name="loggingCorrelationValue">Value to use for log correlation. Default: empty string.</param>
    /// <returns>Boolean success status of updating the cache.</returns>
    private async ValueTask UpdateCachedBase64ObjectAsync(string filename, string base64, bool noExpiry = false,
        string loggingCorrelationValue = "")
    {
        var logContexts = new Dictionary<string, object>
        {
            {"MethodName", nameof(UpdateCachedBase64ObjectAsync)}
        };
        if (!string.IsNullOrWhiteSpace(_configuration.LoggingCorrelationIdentifier) &&
            !string.IsNullOrWhiteSpace(loggingCorrelationValue))
            logContexts.Add(_configuration.LoggingCorrelationIdentifier, loggingCorrelationValue);

        using (_logger?.BeginScope(logContexts.Concat(_baseLogContexts)))
        {
            _logger?.LogDebug("Updating cached Base64 string for {Filename}", filename);
            var cacheResult =
                await _base64Cache.RegisterAsync(filename, base64,
                    noExpiry ? 0 : _configuration.Base64CacheExpiryMinutes);
            if (cacheResult.result)
                _logger?.LogDebug(
                    "Successfully cached Base64 string for {Filename} (valid for: {ExpiryTimeMinutes} minutes)",
                    filename, noExpiry ? "infinite" : _configuration.Base64CacheExpiryMinutes.ToString());
            else
                _logger?.LogError(cacheResult.exception, "Error caching Base64 string for {Filename}", filename);
        }
    }
}