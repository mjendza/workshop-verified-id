﻿using System;
using System.Net;
using System.Threading.Tasks;
using Entra.Verified.ID.WebApp.Configuration;
using Entra.Verified.ID.WebApp.Models;
using Entra.Verified.ID.WebApp.Models.VcApiContracts;
using Entra.Verified.ID.WebApp.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Entra.Verified.ID.WebApp.Controllers.Base;

[Route(VcUrls.IssuanceEndpoint)]
[ApiController]
public class ApiIssuanceController : ControllerBase
{
    private readonly AppSettingsModel _appSettings;
    protected readonly CacheServiceWrapper _cache;
    protected readonly ILogger _logger;
    private readonly VcService _vcService;

    public ApiIssuanceController(IOptions<AppSettingsModel> appSettings
        , CacheServiceWrapper cache
        , VcService vcService, ILogger<ApiIssuanceController> logger)
    {
        _cache = cache;
        _vcService = vcService;
        _logger = logger;
        _appSettings = appSettings.Value;
    }

    [HttpPost("request")]
    public async Task<ActionResult> IssuanceRequestPost([FromBody] IssuanceRequest model)
    {
        var userPresentationRequestId = Guid.NewGuid().ToString();
        var response =
            await _vcService.CallCreateIssuanceRequestVcService(userPresentationRequestId, model);
        return new OkObjectResult(response);
    }

    [HttpPost(VcUrls.IssuanceCallback)]
    public async Task<ActionResult> Callback([FromBody] VcCallbackEvent vcCallbackEvent)
    {
        Request.Headers.TryGetValue("api-key", out var apiKey);
        if (_appSettings.ApiKeyForVerifiedCredentialsCallback != apiKey)
            return new ContentResult
                {StatusCode = (int) HttpStatusCode.Unauthorized, Content = "api-key wrong or missing"};
        var state = vcCallbackEvent.State;
        _cache.CacheObjectWithExpiery(state, vcCallbackEvent, 3600);
        return new OkResult();
    }


    [HttpGet("status")]
    public virtual ActionResult IssuanceResponse()
    {
        string correlationId = Request.Query["id"];
        if (string.IsNullOrEmpty(correlationId)) return new ConflictResult();
        if (_cache.GetCachedObject(correlationId, out VcCallbackEvent callback))
        {
            if (callback.RequestStatus == "request_retrieved")
                return new OkObjectResult(new VcStatus(1,
                    "QR Code is scanned. Waiting for issuance to complete."));

            if (callback.RequestStatus == "issuance_successful")
            {
                _cache.RemoveCacheValue(correlationId);
                _logger.LogInformation("NEW VC ISSUANCE SUCCESSFUL");
                return new OkObjectResult(new VcStatus(2, "Issuance process is completed"));
            }

            if (callback.RequestStatus == "issuance_error")
            {
                _cache.RemoveCacheValue(correlationId);
                return new OkObjectResult(new VcStatus(99,
                    "Issuance process failed with reason: \" + callback.Error.Message"));
            }
        }

        return new OkResult();
    }
}