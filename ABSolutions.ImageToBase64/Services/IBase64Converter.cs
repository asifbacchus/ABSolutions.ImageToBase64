using ABSolutions.ImageToBase64.Models;

namespace ABSolutions.ImageToBase64.Services;

public interface IBase64Converter
{
    /// <summary>
    ///     Retrieves an image from an upstream asset source and returns the image as a base64 string.
    /// </summary>
    /// <param name="filename">Filename or 'slug' of the upstream asset.</param>
    /// <param name="useCache">
    ///     Use cached result if available and not expired. If not specified, configuration value will be
    ///     applied.
    /// </param>
    /// <param name="noExpiry">
    ///     If a cached entry is created or refreshed due to this call, set that cached entry to NEVER
    ///     expire. If not specified, configuration value will be applied.
    /// </param>
    /// <param name="loggingCorrelationValue">
    ///     Use this value for the configured logging correlation key. If empty, the
    ///     correlation key will not be added.
    /// </param>
    /// <param name="cancellationToken">Cancellation token. Optional.</param>
    /// <returns>
    ///     Base64Result object containing a success flag and Base64-encoded string. The Base64-encoded string will always
    ///     be populated regardless of success condition (i.e. never null).
    /// </returns>
    Task<Base64Result> GetImageAsBase64Async(string? filename, bool? useCache = null, bool? noExpiry = null,
        string loggingCorrelationValue = "", CancellationToken cancellationToken = default);
}