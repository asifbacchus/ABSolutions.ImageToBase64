# Quickstart

<link-summary>Get up and running quickly with a default configuration of this library.</link-summary>
<card-summary>Guide to get this library set-up and running in a default configuration.</card-summary>
<web-summary>Quickstart guide to setting up and running ABSolutions.ImageToBase64 .NET library.</web-summary>

This quickstart guides you through:

- <a href="#installation" summary="Installing the NuGet package">Installation</a>
- <a href="#configuration" summary="Configuration options">Configuration</a>
- <a href="#dependency-injection" summary="Use this library via dependency injection">Dependency injection</a>
- <a href="#calling-the-library" summary="How to call this library in your code">Calling the library</a>
- <a href="#more-information" summary="Get more in-depth information about this library">More information</a>

> This quickstart is meant to provide an overview of the library and its features. It is not a comprehensive guide. More
> information is provided on separate pages of this documentation and in the demo library.

> This library has only been tested using .NET Core 8.
> {style="note"}

## Installation

You can install this library from NuGet using your preferred method. The library is named `ABSolutions.ImageToBase64`.
Alternatively, you can download the source code from
the [GitHub repository](https://github.com/asifbacchus/ABSolutions.ImageToBase64) and incorporate the
`ABSolutions.ImageToBase64` class-library project into your solution.

## Configuration

All configuration is handled via `appsettings.json` under the **required** key `Base64Converter`. The following settings
are available:

| Property Name                          | Type     | Description                                                                  | Default            |
|----------------------------------------|----------|------------------------------------------------------------------------------|--------------------|
| `HttpClientName`                       | `string` | The name of the externally configured HttpClient to use for all connections. | `Base64Converter`  |
| `UpstreamImageAssetBaseUri`            | `string` | The full base-URL or base-directory for your upstream image assets.          | `http://localhost` |
| `UpstreamImageRetrievalTimeoutSeconds` | `int`    | Number of seconds to try retrieving the image before cancelling the task.    | `5`                |
| `EnableBase64Cache`                    | `true`   | Whether to use the in-memory cache.                                          | `true`             |
| `NoExpiry`                             | `false`  | If `true`, cached items will NEVER expire.                                   | `false`            |
| `Base64CacheExpiryMinutes`             | `int`    | Number of minutes to cache the image in memory.                              | `1440`             |

> `EnableBase64Cache` and `NoExpiry` can be overridden per-request.
> {style="note"}

`UpstreamImageAssetBaseUri` can be a local directory or a remote URL.

- A trailing slash (`/`) will be added to the base URI if it is missing.
- Filenames are appended to this base URI when retrieving images.
    - The base URI should be a common parent directory if you are using local files.
    - The base URI should be the base URL if you are using remote files.
- Filename are not restricted to only being a file name. You can use entire paths relative to the base URI. If doing
  this, ensure that the path ends in the actual file to be retrieved.

> You MUST supply a valid URI scheme (`http://`, `https://`, `file://`) in the `UpstreamImageAssetBaseUri` setting. If
> using `http` or `https`, invalid URLs will throw a `UriFormatException`. If using `file`, a missing or invalid path
> will throw a `DirectoryNotFoundException`.
> {style="warning"}

## Dependency injection

This library includes a `ServiceCollection` extension method to simplify the registration of the library. Include the
following in your `Program.cs`:

1. Add the following `using` statement:

    ```c#
    using Base64Converter.DependencyInjection;
    ```
2. Add the following lines anywhere before `builder.Build()`:

    ```c#
   builder.Services.AddHttpClient("Base64Converter");
   builder.Services.AddImageToBase64(builder.Configuration);
   ```

   > The `AddHttpClient()` method must be called before `AddImageToBase64()` to ensure that the `HttpClient` is
   available for injection.
   > {style="warning"}

   <tip>
    You can customize the `HttpClient` with headers, delegating handlers, etc. Some examples are provided on the <a href="HTTP-Client.md"></a> page.
   </tip>

## Calling the library

The library only has one public method: `GetImageAsBase64Async()`. This method takes a `string` parameter that
represents the filename or relative path from the `UpstreamImageAssetBaseUri` to the image you want to
convert to a Base64 string. The method returns a `Task<string>`.

To use the library, inject `IBase64Converter` into your class and call the method. Here is a trivial example:

```c#
using ABSolutions.ImageToBase64.Services;

public class MyClass
{
    private readonly IBase64Converter _base64Img;

    public MyClass(IBase64Converter base64Converter)
    {
        _base64Img = base64Converter;
    }

    public async Task<string> GetBase64Image()
    {
        return await _base64Img.GetImageAsBase64Async("image.jpg");
    }
}
```

## More information

If you need more information about available options or features, please refer to the other pages in this documentation.
Please also check out
the [demo library (github)](https://github.com/asifbacchus/ABSolutions.ImageToBase64/tree/main/ABSolutions.ImageToBase64.Demo.Api)
for an example of using this library in a minimal API. The [](Build-a-Demo-API.md) pages provide a step-by-step guide to
building the afore mentioned demo library.