# Dependency Injection

This library is best used via dependency injection. To make this easier, this library includes a `ServiceCollection`
extension method that will register the in-memory cache and converter services for you. The extension method requires a
reference to the configuration container to read settings from `appsettings.json`.

While the extension method registers all services specific to this library, you **MUST** still register a named
`HttpClient` provider for the library to use. The named instance must match the name of the `HttpClient` in the `appsettings.json` file.

> The `HttpClient` provider must be registered before the `AddImageToBase64` extension method is called.
> {style="warning"}

Here's an example, using default values, of how you can handle all necessary dependency injection in your `program.cs`. This can be placed anywhere in the build section (before `builder.Build()`):

```Java
builder.Services.AddHttpClient("ImageToBase64");
builder.Services.AddImageToBase64(builder.Configuration);
```

<seealso style="cards">
    <category ref="related">
        <a href="HTTP-Client.md">HTTP Client</a>
    </category>
</seealso>
