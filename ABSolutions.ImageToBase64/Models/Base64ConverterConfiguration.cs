namespace ABSolutions.ImageToBase64.Models;

/// <summary>
///     Base64 converter configuration options.
/// </summary>
public record Base64ConverterConfiguration
{
    /// <summary>
    ///     JSON key for configuration options.
    /// </summary>
    public static readonly string AppSettingsKey = "Base64Converter";

    /// <summary>
    ///     Named HTTP client to use/create for the base64 converter. Default: Base64Converter.
    /// </summary>
    public string HttpClientName { get; init; } = "Base64Converter";

    /// <summary>
    ///     Upstream image asset base URI. Default: http://localhost.
    /// </summary>
    public string UpstreamImageAssetBaseUri { get; init; } = "http://localhost";

    /// <summary>
    ///     Timeout in seconds for upstream image retrieval. Default: 5.
    /// </summary>
    public int UpstreamImageRetrievalTimeoutSeconds { get; init; } = 5;

    /// <summary>
    ///     Whether to enable base64 cache. Default: True.
    /// </summary>
    public bool EnableBase64Cache { get; init; } = true;

    /// <summary>
    ///     Whether to set cached entries to never expire. Default: False.
    /// </summary>
    public bool NoExpiry { get; init; }

    /// <summary>
    ///     Time in minutes before base64 cache entries expire. Default: 1440 (24 hours).
    /// </summary>
    public int Base64CacheExpiryMinutes { get; init; } = 1440;

    /// <summary>
    ///     Add a key with this name to all log entries to facilitate log correlation. If empty, no key will be added. Default:
    ///     empty.
    /// </summary>
    public string LoggingCorrelationIdentifier { get; init; } = string.Empty;
}