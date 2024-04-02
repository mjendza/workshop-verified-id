using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Portal.VerifiableCredentials.API.Configuration;
using Portal.VerifiableCredentials.API.Models;
using Portal.VerifiableCredentials.API.Models.VcApiContracts;
using IssuanceResponse = Portal.VerifiableCredentials.API.Models.IssuanceResponse;

namespace Portal.VerifiableCredentials.API.Services;

public class VcService
{
    private static  string ManifestCacheKey => "Manifest";
    private static string IssuanceApiEndpoint => VcApplicationConfiguration.VerifiableCredentialsApiEndpoint + "createIssuanceRequest";
    private static string PresentationApiEndpoint => VcApplicationConfiguration.VerifiableCredentialsApiEndpoint + "createPresentationRequest";
    
    private readonly HttpClient _httpClient;
    private readonly AppSettingsModel _appSettings;
    private readonly VcConfigurationService _settings;
    private readonly EntraServicePrincipal _spSettings;
    private CacheServiceWrapper _cache;
    private readonly string _apiKeyForVerifiedCredentialsCallback;
    private readonly ILogger<VcService> _logger;
    private IHostConfigurationService _httpConfigurationService;

    public VcService(HttpClient httpClient, IOptions<AppSettingsModel> appSettings, ILogger<VcService> logger,
        CacheServiceWrapper cache, VcConfigurationService settings, IOptions<EntraServicePrincipal> spSettings, IHostConfigurationService httpConfigurationService)
    {
        _httpClient = httpClient;
        _appSettings = appSettings.Value;
        _settings = settings;
        _httpConfigurationService = httpConfigurationService;
        _spSettings = spSettings.Value;
        _logger = logger;
        _cache = cache;
        
        _apiKeyForVerifiedCredentialsCallback = _appSettings.ApiKeyForVerifiedCredentialsCallback;
    }
    public string GetRequestHostName()
    {
        return _httpConfigurationService.GetRequestHostName();
    }
    private async Task<string> GetAccessToken()
    {
        var authority = string.Format(VcServicePrincipalConfiguration.Authority, _spSettings.TenantId);
        if (!_cache.GetCachedValue($"AccessToken", out string accessToken) && accessToken != null)
        {
            return accessToken;
        }
        var app = ConfidentialClientApplicationBuilder.Create(_spSettings.ClientId)
            .WithClientSecret(_spSettings.ClientSecret)
            .WithAuthority(new Uri(authority))
            .Build();
        string[] scopes = {VcServicePrincipalConfiguration.VcServiceScope};
        var result = await app.AcquireTokenForClient(scopes).ExecuteAsync();
        _logger.LogTrace(result.AccessToken);
        _cache.CacheObjectWithExpiery($"AccessToken", result.AccessToken, 600);
        return result.AccessToken;
    }

    public async Task<VerificationResponse> CallCreatePresentationRequestVcService(string userPresentationRequestId, PresentationRequest requestModel, string type = null)
    {
        var request = VcRequestFactory.CreatePresentationRequest(userPresentationRequestId, _settings.ForHostSettings(), GetRequestHostName(), _apiKeyForVerifiedCredentialsCallback, requestModel);
        var requestAsString = AspNetHelper.SerializeToCamelCase(request);
        _logger.LogTrace("VC Client API Request\n{0}", requestAsString);

        var token = await GetAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var result = await _httpClient
            .PostAsync(PresentationApiEndpoint,
                new StringContent(requestAsString, Encoding.UTF8, "application/json"));
        var verificationResponseAsString = await result.Content.ReadAsStringAsync();
        if (result.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<VerificationResponse>(verificationResponseAsString);
        }
        _logger.LogError($"VC Client API Error Response {verificationResponseAsString}");
        throw new ExternalException("wrong request to VC");
    }
    
    public async Task<IssuanceResponse> CallCreateIssuanceRequestVcService(string userPresentationRequestId, IssuanceRequest requestModel)
    {
        var runtimeConfiguration = _settings.ForHostSettings();
        var pin = GeneratePinWhenNeeded(runtimeConfiguration);
        var path = GetRequestHostName();
        var request = await CreateIssuanceRequestWithClaims(userPresentationRequestId, pin, runtimeConfiguration, path, requestModel);
        var jsonString = AspNetHelper.SerializeToCamelCase(request);
        _logger.LogTrace("VC Client API Request\n{0}", jsonString);

        var token = await GetAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var result = await _httpClient
            .PostAsync(IssuanceApiEndpoint,
                new StringContent(jsonString, Encoding.UTF8, "application/json"));
        
        var responseAsString = await result.Content.ReadAsStringAsync();
        if (result.IsSuccessStatusCode)
        {
            return JsonConvert.DeserializeObject<IssuanceResponse>(responseAsString) with {Id = userPresentationRequestId, Pin = pin?.Value};
        }
        _logger.LogError($"VC Client API Error Response: {responseAsString}");
        throw new ExternalException("wrong request to VC");
    }

    private async Task<VcIssuanceRequest> CreateIssuanceRequestWithClaims(string userPresentationRequestId, Pin pin,AppSettingsModel config,  string path, IssuanceRequest requestModel)
    {
        var request = VcRequestFactory.CreateIssuanceRequest(userPresentationRequestId, pin, config, path, _apiKeyForVerifiedCredentialsCallback);
        var manifest = await VcManifest();
        var claims = manifest.GetSelfAssertedClaims();
        MapClaimsToVcRequest(request, claims, requestModel);
        return request;
    }

    private void MapClaimsToVcRequest(VcIssuanceRequest request, Dictionary<string, string> claims, IssuanceRequest requestModel)
    {
        if (claims.Count == 0 )
        {
            return;
        }
        else if (claims.Count > 0 )
        {
            request.Claims = new Dictionary<string, string>();
            request.Claims.Add("photo", requestModel.Photo);
            request.Claims.Add("firstName", requestModel.FirstName);
            request.Claims.Add("lastName", requestModel.LastName);
            request.Claims.Add("dateOfBirth", requestModel.DateOfBirth);
            request.Claims.Add("address", requestModel.Address);
        }
        else
        {
            //wihout claims we can't use PIN (400 error from API) 
            //TODO fix this with request creation
            request.Pin = null;
        }
    }
    public Pin GeneratePinWhenNeeded(AppSettingsModel settings)
    {
        if (string.IsNullOrEmpty(settings.Pin))
        {
            return null;
        }
        var pinCode =
            RandomNumberGenerator.GetInt32(1, int.Parse("".PadRight(VcApplicationConfiguration.IssuancePinCodeLength, '9')));
        _logger.LogTrace("pin={0}", pinCode);
        return new Pin
        {
            Length = VcApplicationConfiguration.IssuancePinCodeLength,
            Value = string.Format("{0:D" + VcApplicationConfiguration.IssuancePinCodeLength + "}", pinCode)
        };
    }

    public async Task<Manifest> VcManifest()
    {
        if (_cache.GetCachedValue(ManifestKey(), out var content))
        {
            return JsonConvert.DeserializeObject<Manifest>(content);
        }
        var settings = _settings.ForHostSettings();
        var request = new HttpRequestMessage(HttpMethod.Get, settings.CredentialManifestUrl);
        request.Headers.Add("x-ms-sign-contract", "false");
        var result = await _httpClient.SendAsync(request);
        var resultAsString = await result.Content.ReadAsStringAsync();
        _cache.CacheObjectWithExpiery(ManifestKey(), resultAsString, 180);
        return  JsonConvert.DeserializeObject<Manifest>(resultAsString);
    }

    private static string ManifestKey()
    {
        return $"{ManifestCacheKey}";
    }
}