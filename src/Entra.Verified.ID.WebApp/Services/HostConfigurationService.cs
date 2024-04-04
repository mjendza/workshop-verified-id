using Microsoft.AspNetCore.Http;

namespace Entra.Verified.ID.WebApp.Services;

public class HostConfigurationService : IHostConfigurationService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public HostConfigurationService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetRequestHostName()
    {
        var request = _httpContextAccessor.HttpContext.Request;
        var scheme = "https";
        return string.Format("{0}://{1}", scheme, request.Host);
    }
}