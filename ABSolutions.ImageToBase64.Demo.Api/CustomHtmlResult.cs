using System.Net.Mime;
using System.Text;

namespace ABSolutions.ImageToBase64.Demo.Api;

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