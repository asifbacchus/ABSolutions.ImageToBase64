using System.Net;
using System.Text.Json;
using ABSolutions.ImageToBase64.Demo.Api;
using ABSolutions.ImageToBase64.DependencyInjection;
using ABSolutions.ImageToBase64.Models;
using ABSolutions.ImageToBase64.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logBuilder => logBuilder.AddJsonConsole(opts =>
{
    opts.IncludeScopes = true;
    opts.TimestampFormat = "[HH:mm:ss]";
    opts.JsonWriterOptions = new JsonWriterOptions
    {
        Indented = true
    };
}));
builder.Services.AddHttpClient(
    builder.Configuration.GetRequiredSection(Base64ConverterConfiguration.AppSettingsKey)
        .Get<Base64ConverterConfiguration>()?.HttpClientName ?? "Base64ConverterClient", client =>
    {
        client.DefaultRequestHeaders.Add("Accept", "image/*");
        client.DefaultRequestHeaders.UserAgent.ParseAdd("ABSolutions.ImageToBase64");
    });
builder.Services.AddImageToBase64(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Endpoints: '/picture' or '/base64'. Use 'cache' query parameter to control caching.");

app.MapGet("/base64", async (IBase64Converter converter, bool cache = false) =>
{
    try
    {
        var loggingCorrelationValue = Guid.NewGuid().ToString();
        var imageAsBase64 =
            await converter.GetImageAsBase64Async("500.webp", cache, loggingCorrelationValue: loggingCorrelationValue);
        return imageAsBase64.IsSuccess
            ? Results.Ok(imageAsBase64.Base64String)
            : Results.NotFound(imageAsBase64.Base64String);
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

app.MapGet("/picture", async (IBase64Converter converter, bool cache = false) =>
{
    try
    {
        var loggingCorrelationValue = Guid.NewGuid().ToString();
        const string htmlTemplate =
            "<!DOCTYPE html><html><head><title>Base64 Image Test</title></head><body><img src=\"{0}\" alt=\"image embedded as base64 string\" /></body></html>";
        var imageAsBase64 =
            await converter.GetImageAsBase64Async("500.webp", cache, loggingCorrelationValue: loggingCorrelationValue);
        var htmlResults = string.Format(htmlTemplate, imageAsBase64.Base64String);
        return new CustomHtmlResult(htmlResults, imageAsBase64.IsSuccess ? HttpStatusCode.OK : HttpStatusCode.NotFound);
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

app.Run();