using System.Threading.Tasks;
using Entra.Verified.ID.WebApp.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Entra.Verified.ID.WebApp.Configuration;

internal static class VcServiceCollectionExtensions
{
    public static async Task<IServiceCollection> AddVcServices(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddScoped<VcService>();
        services.AddScoped<IHostConfigurationService, HostConfigurationService>();
        services.AddScoped<VcConfigurationService>();
        services.AddSingleton<CacheServiceWrapper>();
        return services;
    }
}