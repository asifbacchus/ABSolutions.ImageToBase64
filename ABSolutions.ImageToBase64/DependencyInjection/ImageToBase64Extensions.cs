using ABSolutions.ImageToBase64.Models;
using ABSolutions.ImageToBase64.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ABSolutions.ImageToBase64.DependencyInjection;

public static class ImageToBase64Extensions
{
    public static IServiceCollection AddImageToBase64(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<Base64ConverterConfiguration>(
            configuration.GetSection(Base64ConverterConfiguration.AppSettingsKey));
        services.AddSingleton<IBase64Cache, Base64CacheInMemory>();
        services.AddSingleton<IBase64Converter, Base64Converter>();
        return services;
    }
}