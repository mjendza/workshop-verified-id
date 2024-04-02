using Entra.Verified.ID.WebApp.Configuration;
using Microsoft.Extensions.Options;

namespace Entra.Verified.ID.WebApp.Services;

public class VcConfigurationService
{
    private readonly AppSettingsModel _settings;

    public VcConfigurationService(IOptions<AppSettingsModel> settings)
    {
        _settings = settings.Value;
    }

    public AppSettingsModel ForHostSettings()
    {
        return _settings;
    }
}