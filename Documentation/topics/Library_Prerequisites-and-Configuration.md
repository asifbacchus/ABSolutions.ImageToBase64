# Prerequisites and Configuration

<link-summary>API Demo: Setting up and configuring the library for use in the API demo.</link-summary>
<card-summary>Learn how to set-up and configure the library for use in the API demo project.</card-summary>
<web-summary>ABSolutions.ImageToBase64 API demo: Configuring the library.</web-summary>

## Project template

Create a new `Empty ASP.NET Core Web Application` project. Use .NET 8.0.
> An *empty* web application project is the typical
> template used when creating a minimal API project.

## NuGet packages

Please install the `%ProjectLibraryName%` NuGet package.

## Configuration

Let's go ahead and configure the library by modifying the `appsettings.json` file to appear as follows:
<code-block lang="json" src="Library_Demo_appsettings.json"/>

Everything is default except for the `UpstreamImageAssetBaseUri`. We'll be using a free image service to give us a
random picture to
convert.

> I've also changed the logging level for the `ABSolutions.ImageToBase64` namespace to `Debug`. This is optional, but it
> will let you see more output on the console and better understand what the library is doing.

## Register services

To use this library, we need to register two services with the dependency injection container: the `HttpClient` service
and the `Base64Converter` service.

### HTTP Client

We'll be using a lightly customized named `HttpClient` instance using the default name the library expects
(`Base64Converter`) and two custom request headers as an example:

<code-block src="Library_Demo_Program.cs" include-lines="9-15" lang="c#"/>

> The `Accept` header tells the upstream service that we only want images. This is strongly recommended.
> The `UserAgent` header is included as an example of other headers that can be added.

This can be added to the `Program.cs` anywhere before the `builder.Build()` method is called.

### Base64Converter service

We'll use the library's service extension method to register the library's services with the dependency injection
container. Since the extension method requires access to the service configuration in `appsettings.json`, we'll pass the
builder configuration object as an argument.

You can add this registration call anywhere in the `Program.cs` file before the `builder.Build()` method is called.
Here's an example along with the `HttpClient` registration:

<code-block src="Library_Demo_Program.cs" include-lines="9-18" lang="c#"/>
