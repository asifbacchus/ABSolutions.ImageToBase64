# Calling the Library

<link-summary>How to call this library to convert images to Base64 strings.</link-summary>
<card-summary>Discussion of how to call this library to convert images to Base64 strings.</card-summary>
<web-summary>How to call the ABSolutions.ImageToBase64 .NET library to convert images to Base64 strings.</web-summary>

## Injecting the library

Since the library is registered with dependency injection, you can inject it anywhere you need it.

```c#
public class MyClass
{
    private readonly IBase64Converter _base64Image;

    public MyClass(IBase64Converter base64Image)
    {
        _base64Image = base64Image;
    }
}
```

## The GetImageAsBase64Async method

The service only has one method, `GetImageAsBase64Async`, with the following signature:

`GetImageAsBase64Async(string? filename, bool? useCache = null, bool? noExpiry = null,
        CancellationToken cancellationToken = default)`

As you can see, there are actually no required parameters. In practice, however, you will be at least be supplying a
filename.

| Parameter         | Explanation                                                                                                                               | Default                  |
|-------------------|-------------------------------------------------------------------------------------------------------------------------------------------|--------------------------|
| filename          | The file (or relative path and file name) to retrieve.                                                                                    | `none`                   |
| useCache          | Whether to use the in-memory cache for this particular request. If specified (not null), this overrides the global configuration setting. | `null`                   |
| noExpiry          | Set the cache entry for this request to never expire. If specified (not null), this overrides the global configuration setting.           | `null`                   |
| cancellationToken | Optional cancellation token to use. If none supplied, this task will not be cancellable.                                                  | `CancellationToken.None` |

This is an *asynchronous* method that returns a `Task<string>` where the string is the Base64 encoded image.

Continuing with the previous example code where we injected the library, this is an example of how to call this method:

```c#
public class MyClass
{
    private readonly IBase64Converter _base64Image;

    public MyClass(IBase64Converter base64Image)
    {
        _base64Image = base64Image;
    }
    
    public async Task PrintBase64Image(string filename)
    {
        Console.WriteLine(await _base64Image.GetImageAsBase64Async(filename));
    }
}
```

### Default return

If there are any problems retrieving the image or if any exceptions are thrown for whatever reason, the method will
return a default image. This allows your webpages, applications, etc. to retain their layout while still being obvious
that something has gone wrong. The default image looks like this:

![Default Image](defaultBase64ReturnImage.png)

The image is 512px x 512px `png` file of a black exclamation mark surrounded by a black outline of a rounded triangle.
The image has a transparent background.
