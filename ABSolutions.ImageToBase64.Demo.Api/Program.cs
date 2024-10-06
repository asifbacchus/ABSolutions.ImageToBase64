using ABSolutions.ImageToBase64.Demo.Api;
using ABSolutions.ImageToBase64.DependencyInjection;
using ABSolutions.ImageToBase64.Models;
using ABSolutions.ImageToBase64.Services;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddImageToBase64(builder.Configuration);
builder.Services.AddHttpClient(
    builder.Configuration.GetRequiredSection(Base64ConverterConfiguration.AppSettingsKey)
        .Get<Base64ConverterConfiguration>()?.HttpClientName ?? "Base64ConverterClient", client =>
    {
        client.DefaultRequestHeaders.Add("Accept", "image/*");
        client.DefaultRequestHeaders.UserAgent.ParseAdd("ABSolutions.ImageToBase64");
    });

var app = builder.Build();

app.MapGet("/", () => "Endpoints: '/picture' or '/base64'. Use 'cache' query parameter to control caching.");

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

app.MapGet("/picture", async (IBase64Converter converter, bool cache = false) =>
{
    try
    {
        const string htmlTemplate =
            "<!DOCTYPE html><html><head><title>Base64 Image Test</title></head><body><img src=\"{0}\" alt=\"image embedded as base64 string\" /></body></html>";
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

app.Run();