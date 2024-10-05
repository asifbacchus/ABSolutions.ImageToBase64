using ABSolutions.ImageToBase64.Models;

namespace ABSolutions.ImageToBase64.Services;

public class Base64CacheInMemory : IBase64Cache
{
    public async ValueTask<(bool result, Exception? exception)> RegisterAsync(string filename, string base64,
        int expiryMinutes)
    {
        throw new NotImplementedException();
    }

    public async ValueTask<Base64CachedObject?> GetCachedBase64(string filename)
    {
        throw new NotImplementedException();
    }
}