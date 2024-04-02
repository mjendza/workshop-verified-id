using Microsoft.Extensions.Options;
using Portal.VerifiableCredentials.API.Configuration;

namespace Portal.VerifiableCredentials.API.Services;

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