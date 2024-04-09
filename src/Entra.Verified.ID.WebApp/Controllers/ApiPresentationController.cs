using System;
using System.Linq;
using System.Threading.Tasks;
using Entra.Verified.ID.WebApp.Configuration;
using Entra.Verified.ID.WebApp.Models;
using Entra.Verified.ID.WebApp.Models.VcApiContracts;
using Entra.Verified.ID.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Entra.Verified.ID.WebApp.Controllers.Base;

[Route(VcUrls.PresentationEndpoint)]
[ApiController]
public class ApiPresentationController : ControllerBase
{
    private readonly string _apiKey;
    protected readonly CacheServiceWrapper _cache;
    private readonly ILogger<ApiPresentationController> _log;
    private readonly VcService _vcService;

    public ApiPresentationController(VcService vcService
        , IOptions<AppSettingsModel> appSettings
        , CacheServiceWrapper cache
        , ILogger<ApiPresentationController> log)
    {
        _vcService = vcService;
        _cache = cache;
        _log = log;
        _apiKey = appSettings.Value.ApiKeyForVerifiedCredentialsCallback;
    }
    
    [HttpPost("request")]
    public async Task<ActionResult> PresentationRequestPost([FromBody] PresentationRequest model)
    {
        var type = Request.Headers["x-type"];
        var userPresentationRequestId = Guid.NewGuid().ToString();
        model.PurposeOfPresentation = "Please share expected VC data.";
        var vcResponse =
            await _vcService.CallCreatePresentationRequestVcService(userPresentationRequestId, model);
        vcResponse.Id = userPresentationRequestId;
        _log.LogTrace("VC Client API Response\n{0}", vcResponse);
        return new OkObjectResult(vcResponse);
    }

    [HttpPost("response-html")]
    public virtual async Task<ActionResult> PresentationHtmlResponse([FromBody] PresentationResponseVcRequest request)
    {
        var userPresentationRequestId = request.Id;
        if (!_cache.GetCachedObject(userPresentationRequestId, out VcCallbackEvent vcCallbackCachedResult))
            return new NotFoundResult();
        _cache.RemoveCacheValue(userPresentationRequestId);
        // setup the response that we are returning to B2C
        var firstVerifiedCredentialsData = vcCallbackCachedResult.VerifiedCredentialsData[0];
        var obj = new HtmlVerificationResult(vcCallbackCachedResult, firstVerifiedCredentialsData
                .Type.Last(),
            firstVerifiedCredentialsData.Issuer, vcCallbackCachedResult.Subject,
            vcCallbackCachedResult.Subject.Replace("did:ion:", "did.ion.").Split(":")[0],
            firstVerifiedCredentialsData.Claims, firstVerifiedCredentialsData.FaceCheck?.MatchConfidenceScore);
        await RunHtmlResponseManipulator(obj);
        return new OkObjectResult(obj);
    }

    protected virtual Task RunHtmlResponseManipulator(HtmlVerificationResult obj)
    {
        return Task.CompletedTask;
    }

    [HttpPost(VcUrls.PresentationCallback)]
    public async Task<ActionResult> Callback([FromBody] dynamic request)
    {
        var requestAsType = (VcCallbackEvent) request.ToObject<VcCallbackEvent>();
        _log.LogTrace($"presentation-callback: {JsonConvert.SerializeObject(request)}");
        Request.Headers.TryGetValue("api-key", out var apiKey);
        if (_apiKey != apiKey) return new ForbidResult();
        var state = requestAsType.State;
        _cache.CacheObjectWithExpiery(state, requestAsType, 600);
        return new OkResult();
    }

    [HttpGet("status")]
    public virtual ActionResult Status()
    {
        string correlationId = Request.Query["id"];
        if (string.IsNullOrEmpty(correlationId)) return new BadRequestResult();

        if (_cache.GetCachedObject(correlationId, out VcCallbackEvent callback))
        {
            if (callback.RequestStatus == "request_retrieved")
                return new OkObjectResult(new
                    {status = 1, message = "QR Code is scanned. Waiting for validation..."});

            if (callback.RequestStatus == "presentation_verified")
                //_cache.RemoveCacheValue(correlationId);
                return new OkObjectResult(new {status = 2, message = "OK"});

            if (callback.RequestStatus == "presentation_error")
                return new OkObjectResult(new
                    {status = 99, message = "Presentation failed: " + callback.Error.Message});
        }

        return new OkObjectResult(new {status = 0, message = "No data"});
    }
}