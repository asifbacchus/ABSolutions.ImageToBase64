using ABSolutions.ImageToBase64.Models;

namespace ABSolutions.ImageToBase64.Services;

public class Base64CacheInMemory : IBase64Cache
{
    private readonly List<Base64CachedObject> _base64CachedObjects = [];

    public async ValueTask<(bool result, Exception? exception)> RegisterAsync(string filename, string base64,
        int expiryMinutes)
    {
        // delete existing
        var existingBase64Obj = _base64CachedObjects.FirstOrDefault(i => i.Filename == filename);
        if (existingBase64Obj is not null) _base64CachedObjects.Remove(existingBase64Obj);

        // register new object
        try
        {
            _base64CachedObjects.Add(new Base64CachedObject
            {
                Filename = filename,
                Base64String = base64,
                Expiry = expiryMinutes == 0 ? null : DateTime.UtcNow.AddMinutes(expiryMinutes)
            });
            return await ValueTask.FromResult<(bool result, Exception? exception)>((true, null));
        }
        catch (Exception e)
        {
            return await ValueTask.FromResult<(bool, Exception?)>((false, e));
        }
    }

    public async ValueTask<Base64CachedObject?> GetCachedBase64(string filename)
    {
        return await ValueTask.FromResult(_base64CachedObjects.FirstOrDefault(i => i.Filename == filename));
    }
}