# Configuration Reference

## Configuration options

All library configuration options are defined in `appsettings.json`. The following is a detailed explanation of
each configuration option.

> Configuration options **MUST** be in a key called `Base64Converter` or the library will not find your settings!
> {style="warning"}

<deflist collapsible="true">
<def title="string:HttpClientName | Default: Base64Converter">
Named HTTP client to use/create to retrieve upstream remote images for conversion. <a href="HTTP-Client.md">[more info]</a>
</def>
<def title="string:UpstreamImageAssetBaseUri | Default: http://localhost">
Upstream base URI from which to retrieve images.<br/>
If a file path is provided, images will be retrieved relative to this path. If a URL is provided, images will be retrieved relative to this URL. If specifying a URL, it MUST be an absolute URL. A trailing slash will added to this value if not present.<br/>
NOTE: You MUST specify a protocol for this URI: http://, https:// or file://.
</def>
<def title="int:UpstreamImageRetrievalTimeoutSeconds | Default: 5">
Timeout in seconds for upstream image retrieval. If this timeout is exceeded, a OperationCancelledException with a TimeoutException inner exception will be thrown.
</def>
<def title="bool:EnableBase64Cache | Default: true">
Whether to enable the in-memory cache of Base64 strings. When enabled, Base64 strings for a given file name will be retrieved from the cache so long as the they have not expired. If the cache is disabled, the upstream source will be queried for each request.
</def>
<def title="bool:NoExpiry | Default: false">
Whether to set cached entries to never expire. If this is enabled, images will only be retrieved from the upstream source once and then served from the cache until your program is restarted.
</def>
<def title="int:Base64CacheExpiryMinutes | Default: 1440 (1 day)">
Number of minutes before base64 cache entries expire. Base64 strings in the cache will not be served from the cache once this time has elapsed. Please note that cached entries are "refreshed" each time they are accessed.
</def>
</deflist>

> `EnableBase64Cache` and `NoExpiry` can be overriden on a per-call basis. Please see [Calling the Library](Calling-the-Library.md) for more information.

## Configuration example

```json
{
  "Base64Converter": {
    "HttpClientName": "Base64Converter",
    "UpstreamImageAssetBaseUri": "http://localhost",
    "UpstreamImageRetrievalTimeoutSeconds": 5,
    "EnableBase64Cache": true,
    "NoExpiry": false,
    "Base64CacheExpiryMinutes": 1440
  }
}
```

## Configuration bind-model

In the event you need to read the configuration options, you can access them via dependency injection using the *options
pattern*. Inject `IOptions<Base64ConverterConfiguration>` into your class, something like this:

```c#
public class MyService
{
    private readonly Base64ConverterConfiguration _config;

    public MyService(IOptions<Base64ConverterConfiguration> config)
    {
        _config = config.Value;
    }
}
```
