# HTTP Client

To make this library more flexible and to account for the numerous possible upstream configuration cases, this library
allows you to configure the `HttpClient` it uses externally in your code. This also allows you to share the client
between services, if desired.

The `HttpClient` should be configured as per the `IHttpClientFactory` pattern. This allows you to configure many options such as default request headers, resiliance policies and even delegating handlers for things like authentication.

> The most important configuration option is the `HttpClient` name. This must match the name you specify in the libary configuration in `appsettings.json`.

## Example configurations

### Basic configuration

```Java
builder.Services.AddHttpClient("MyHttpClient");
```

This will create an `HttpClient` named "MyHttpClient" with default settings.

### Set response headers

```Java
builder.Services.AddHttpClient("MyHttpClient", client =>
{
    client.DefaultRequestHeaders.Add("Accept", "image/*");
    client.DefaultRequestHeaders.UserAgent.ParseAdd("ABSolutions.ImageToBase64");
});
```

This will create an `HttpClient` named "MyHttpClient". An `Accept: image/*` request header will be added to all requests by default. In addition, a `User-Agent` header will be added with the value `ABSolutions.ImageToBase64`.

### Upstream authentication

Since methods and requirements for upstream authentication can vary greatly, this is probably the best example of why the choice was made to require injecting a separately configured `HttpClient`. In general, something like would be used for bearer token authentication:

```Java
builder.Services.AddHttpClient("MyHttpClient", client =>
{
    client.DefaultRequestHeaders.Add("Accept", "image/*");
    client.DefaultRequestHeaders.Authorization =
        new AuthenticationHeaderValue("Bearer", "token123456");
});
```

> In production you will probably either retrieve the token from a secure vault or some form of secured configuration file. Alternatively, you can create a *delegating handler* to handler authentication as needed. The latter option is beyond the scope of this document.