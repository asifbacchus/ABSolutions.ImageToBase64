using System.Text.Json;
using ABSolutions.ImageToBase64.Demo.Blazor.Components;
using ABSolutions.ImageToBase64.DependencyInjection;
using ABSolutions.ImageToBase64.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging(logBuilder => logBuilder.AddJsonConsole(opts =>
{
    opts.IncludeScopes = true;
    opts.TimestampFormat = "[HH:mm:ss] ";
    opts.JsonWriterOptions = new JsonWriterOptions
    {
        Indented = true
    };
}));

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient(
    builder.Configuration.GetRequiredSection(Base64ConverterConfiguration.AppSettingsKey)
        .Get<Base64ConverterConfiguration>()?.HttpClientName ?? "Base64Converter", client =>
    {
        client.DefaultRequestHeaders.Add("Accept", "image/*");
        client.DefaultRequestHeaders.UserAgent.ParseAdd("ABSolutions.ImageToBase64");
    });
builder.Services.AddImageToBase64(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();