using ABSolutions.ImageToBase64.Models;

namespace ABSolutions.ImageToBase64.Services;

/// <summary>
///     Get or set a Base64 string in the cache.
/// </summary>
public interface IBase64Cache
{
    /// <summary>
    ///     Register a Base64 string in the cache.
    /// </summary>
    /// <param name="filename">Filename of the source image, used as an identifier.</param>
    /// <param name="base64">Base64 string to cache.</param>
    /// <param name="expiryMinutes">Number of minutes until cached object expires. Set to 0 if object should NEVER expire.</param>
    /// <returns>Boolean result of registering a new cached object.</returns>
    public ValueTask<(bool result, Exception? exception)> RegisterAsync(string filename, string base64,
        int expiryMinutes);

    /// <summary>
    ///     Retrieve a cached Base64 string.
    /// </summary>
    /// <param name="filename">Filename of the image represented by a Base64 string in the cache.</param>
    /// <returns>Object containing the Base64 string representing an image or null if nothing found in the cache.</returns>
    public ValueTask<Base64CachedObject?> GetCachedBase64(string filename);
}