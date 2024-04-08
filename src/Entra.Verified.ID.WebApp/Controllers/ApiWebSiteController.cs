using System;
using System.Threading.Tasks;
using Entra.Verified.ID.WebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace Entra.Verified.ID.WebApp.Controllers;

[Route("api/website/[action]")]
[ApiController]
public class WebSiteController : ControllerBase
{
    private readonly VcConfigurationService _config;
    private readonly VcService _vcService;

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