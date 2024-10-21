using ABSolutions.ImageToBase64.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ABSolutions.ImageToBase64.Services;

public class Base64Converter : IBase64Converter
{
    private const string DefaultBase64String =
        "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";

    private const string DefaultFileExtension = "png";
    private const string ErrorReturnValue = $"data:image/{DefaultFileExtension};base64,{DefaultBase64String}";

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

    public async Task<string> GetImageAsBase64Async(string? filename, bool? useCache = null, bool? noExpiry = null,
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
            string? responseContent = null;
            var usingDefault = false;

            // set configuration options
            var cache = useCache ?? _configuration.EnableBase64Cache;
            var expiry = noExpiry ?? _configuration.NoExpiry;

            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                if (cache && filename is not null)
                {
                    var cacheResponse = await GetCachedBase64ObjectAsync(filename, loggingCorrelationValue);
                    responseContent = cacheResponse?.Base64String;
                }

                responseContent ??= filename is null
                    ? null
                    : _accessLocalFiles switch
                    {
                        true => await GetImageFromLocalFileAsync(filename, loggingCorrelationValue, cancellationToken),
                        false => await GetImageFromUpstreamAsync(filename, loggingCorrelationValue, cancellationToken)
                    };

                if (responseContent is null)
                {
                    _logger?.LogWarning("No image asset found for {Filename}, using default image", filename);
                    responseContent = DefaultBase64String;
                    usingDefault = true;
                }

                var fileExtension = Path.GetExtension(filename!).TrimStart('.');
                if (string.IsNullOrEmpty(fileExtension))
                {
                    _logger?.LogWarning(
                        "Filename {Filename} has no extension, unable to determine image type: using default image",
                        filename);
                    responseContent = DefaultBase64String;
                    fileExtension = DefaultFileExtension;
                    usingDefault = true;
                }

                // update cache with image as Base64 string, unless disabled or using default image
                if (cache && !usingDefault)
                    await UpdateCachedBase64ObjectAsync(filename!, responseContent, expiry, loggingCorrelationValue);

                return $"data:image/{fileExtension};base64,{responseContent}";
            }
            catch (OperationCanceledException exception) when (cancellationToken.IsCancellationRequested)
            {
                _logger?.LogWarning(exception,
                    "Upstream image {Filename} from {UpstreamBaseUri} request was cancelled, sending default image",
                    filename ?? "<filename not specified>", _httpBaseAddress);
                return ErrorReturnValue;
            }
            catch (OperationCanceledException exception) when
                (exception.InnerException is TimeoutException timeoutException)
            {
                _logger?.LogWarning(timeoutException,
                    "Upstream image {Filename} from {UpstreamBaseUri} request timed out, sending default image",
                    filename ?? "<filename not specified>", _httpBaseAddress);
                return ErrorReturnValue;
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

                return ErrorReturnValue;
            }
        }
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