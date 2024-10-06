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
    /// <param name="cancellationToken">Cancellation token. Optional.</param>
    /// <returns></returns>
    Task<string> GetImageAsBase64Async(string? filename, bool? useCache = null, bool? noExpiry = null,
        CancellationToken cancellationToken = default);
}