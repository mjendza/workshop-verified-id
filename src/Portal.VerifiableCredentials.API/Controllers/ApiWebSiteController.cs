using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Portal.VerifiableCredentials.API.Configuration;
using Portal.VerifiableCredentials.API.Services;

namespace Portal.VerifiableCredentials.API.Controllers;

[Route("api/website/[action]")]
[ApiController]
public class WebSiteController : ControllerBase
{
    private readonly VcService _vcService;
    private readonly VcConfigurationService _config;

    public WebSiteController(VcService vcService, VcConfigurationService config)
    {
        _vcService = vcService;
        _config = config;
    }
   
    [HttpGet("/api/website/issuer/settings")]
    public async Task<ActionResult> Settings()
    {
        var settings = _config.ForHostSettings();
        var manifest = await _vcService.VcManifest();
        var info = new
        {
            date = DateTime.Now.ToString(),
            // host = _vcService.GetRequestHostName(),
            // api =  $"{_vcService.GetRequestHostName()}/{VcUrls.IssuanceEndpoint}",
            didIssuer = manifest.Input.Issuer,
            credentialType = settings.CredentialType,
            displayCard = manifest.Display.Card,
            buttonColor = "#FFBB05",
            contract = manifest.Display.Contract,
            selfAssertedClaims = manifest.GetSelfAssertedClaims()
        };
        return new OkObjectResult(info);
    }
}