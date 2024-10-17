# Create Endpoints

<link-summary>API Demo: Creating endpoints to show different library uses.</link-summary>
<card-summary>Build 2 endpoints to see variations of how this library can be used.</card-summary>
<web-summary>ABSolutions.ImageToBase64 API demo: Building the request endpoints.</web-summary>

We'll be making three endpoints for this demo project: `root`, `base64`, and `picture`. To keep things simple, we'll
just put all the endpoints in the `Program.cs` file. Add the following sections after the `builder.Build();` line.

<tabs>
<tab title="root">
Modify the existing `Get: /` endpoint to return a helpful message.
<br/>
<compare>
<code-block lang="c#">
app.MapGet("/", () => "Hello World!");
</code-block>
<code-block>
app.MapGet("/", () => "Endpoints: '/picture' or '/base64'. Use 'cache' query parameter to control caching.");
</code-block>
</compare>
</tab>
<tab title="base64">
<code-block lang="c#">
app.MapGet("/base64", async (IBase64Converter converter, bool cache = false) =>
{
    try
    {
        var imageAsBase64String = await converter.GetImageAsBase64Async("500.webp", cache);
        return Results.Ok(imageAsBase64String);
    }
    catch (Exception e)
    {
        return Results.Problem(new ProblemDetails
        {
            Detail = e.Message,
            Status = StatusCodes.Status500InternalServerError
        });
    }
});
</code-block>
<br/>

This endpoint returns the Base64 representation of a random remote image. The `cache` query parameter is optional and
defaults to `false`. If `true`, the image will be cached for the duration specified in `appsettings.json`.

The `cache` parameter defaults to `false` so that a new image is fetched from the remote server on reload. This is only
to help
you confirm that new Base64 strings are being generated.
</tab>
<tab title="picture">
<code-block lang="c#">
app.MapGet("/picture", async (IBase64Converter converter, bool cache = false) =&gt;
{
    try
    {
        const string htmlTemplate =&lt;
            "&lt;!DOCTYPE html&gt;&lt;html&gt;&lt;head&gt;&lt;title&gt;Base64 Image Test&lt;/title&gt;&lt;/head&gt;&lt;body&gt;&lt;img src=\"{0}\" alt=\"image embedded as base64 string\" /&gt;&lt;/body&gt;&lt;/html&gt;";
        var imageAsBase64String = await converter.GetImageAsBase64Async("500.webp", cache);
        var htmlResults = string.Format(htmlTemplate, imageAsBase64String);
        return new CustomHtmlResult(htmlResults);
    }
    catch (Exception e)
    {
        return Results.Problem(new ProblemDetails
        {
            Detail = e.Message,
            Status = StatusCodes.Status500InternalServerError
        });
    }
});
</code-block>
<br/>

This endpoint retrieves a random remote image, converts it to a Base64 string, and returns a webpage displaying that
image using the embedded Base64 string. The `cache` query parameter is optional and defaults to `false`. If `true`, the
image will be cached for the duration specified in the configuration file.

The `cache` parameter defaults to `false` so that a new image is fetched from the remote server on reload. This is only to help
you confirm that new Base64 strings are being generated.
</tab>
</tabs>

## The `CustomHtmlResult` class

Notice in the `picture` endpoint we are returning a `CustomHtmlResult`. This is a custom class that we'll need to
create. Add the following class to the project.

<code-block lang="c#">
public class CustomHtmlResult : IResult
{
    private readonly string _htmlContent;
     
    public CustomHtmlResult(string htmlContent)
    {
        _htmlContent = htmlContent;
    }
     
    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_htmlContent);
        await httpContext.Response.WriteAsync(_htmlContent);
    }
}
</code-block>

This class allows us to return a custom HTML string as a response as opposed to text or JSON. Without this class, the
`htmlTemplate` in the `picture` endpoint would be returned as plain-text instead of HTML.

> Explaining this class and the `IResult` type is beyond the scope of this document.
> {style="note"}
