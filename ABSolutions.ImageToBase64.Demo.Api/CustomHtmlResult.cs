using System.Net;
using System.Net.Mime;
using System.Text;

namespace ABSolutions.ImageToBase64.Demo.Api;

public class CustomHtmlResult : IResult
{
    private readonly string _htmlContent;
    private readonly HttpStatusCode _statusCode;

    public CustomHtmlResult(string htmlContent, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        _htmlContent = htmlContent;
        _statusCode = statusCode;
    }

    public async Task ExecuteAsync(HttpContext httpContext)
    {
        httpContext.Response.ContentType = MediaTypeNames.Text.Html;
        httpContext.Response.ContentLength = Encoding.UTF8.GetByteCount(_htmlContent);
        httpContext.Response.StatusCode = (int) _statusCode;
        await httpContext.Response.WriteAsync(_htmlContent);
    }
}