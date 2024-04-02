using Microsoft.Extensions.Options;
using Portal.VerifiableCredentials.API.Configuration;

namespace Portal.VerifiableCredentials.API.Services;



public class VcConfigurationService
{
    public VcConfigurationService(IOptions<AppSettingsModel> settings)
    {
        _settings = settings.Value;
    }
    
    private readonly AppSettingsModel _settings;

    public AppSettingsModel ForHostSettings()
    {
        return _settings;
    }
}