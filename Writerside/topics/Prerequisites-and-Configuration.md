# Prerequisites and Configuration

<link-summary>API Demo: Setting up and configuring the library for use in the API demo.</link-summary>
<card-summary>Learn how to set-up and configure the library for use in the API demo project.</card-summary>
<web-summary>ABSolutions.ImageToBase64 API demo: Configuring the library.</web-summary>

## Project template

Create a new `Empty ASP.NET Core Web Application` project. Use .NET 8.0.
> An *empty* web application project is the typical
template used when creating a minimal API project.

## NuGet packages

Please install the `ABSolutions.ImageToBase64` NuGet package.

## Configuration

Let's go ahead and configure the library:

### appsettings.json

<tabs>
<tab title="Updated">
<code-block lang="json">
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "ABSolutions.ImageToBase64": "Debug"
    }
  },
  "AllowedHosts": "*",
"Base64Converter": {
    "UpstreamImageAssetBaseUri": "https://picsum.photos/",
    "UpstreamImageRetrievalTimeoutSeconds": 5,
    "EnableBase64Cache": true,
    "NoExpiry": false,
    "Base64CacheExpiryMinutes": 5
  }
}
</code-block>
</tab>
<tab title="Original">
<code-block lang="json">
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
    }
  },
  "AllowedHosts": "*",
}
</code-block>
</tab>
</tabs>

Everything is default except for the upstream URI. We'll be using a free image service to give us a random picture to
convert.

> I've also changed the logging level for the `ABSolutions.ImageToBase64` namespace to `Debug`. This is optional, but it
> will let you see more output on the console and better understand what the library is doing.

## Register services

We'll be using a lightly customized `HttpClient` using the default name for our library (`Base64Converter`).

### Program.cs

<tabs>
<tab title="Updated">
<code-block lang="c#">
using ABSolutions.ImageToBase64.Demo.Api;
using ABSolutions.ImageToBase64.DependencyInjection;
using ABSolutions.ImageToBase64.Models;
using ABSolutions.ImageToBase64.Services;
using Microsoft.AspNetCore.Mvc;
 
var builder = WebApplication.CreateBuilder(args);
 
builder.Services.AddImageToBase64(builder.Configuration);
builder.Services.AddHttpClient("Base64ConverterClient", client =>
    {
        client.DefaultRequestHeaders.Add("Accept", "image/*");
        client.DefaultRequestHeaders.UserAgent.ParseAdd("ABSolutions.ImageToBase64");
    });
 
var app = builder.Build();
 
app.MapGet("/", () => "Hello World!");
 
app.Run();
</code-block>
</tab>
<tab title="Original">
<code-block lang="c#">
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
 
app.MapGet("/", () => "Hello World!");
 
app.Run();
</code-block>
</tab>
</tabs>

First, we use the `AddImageToBase64` extension method to register the library's services. Then, we add a custom
`HttpClient` with two request headers.

> The `Accept` header tells the upstream service that we only want images. This is strongly recommended.
> The `UserAgent` header is included as an example of other headers that can be added.
