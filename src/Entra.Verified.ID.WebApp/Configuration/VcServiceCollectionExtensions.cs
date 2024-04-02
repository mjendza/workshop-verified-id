using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Portal.VerifiableCredentials.API.Services;

namespace Portal.VerifiableCredentials.API.Configuration;

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