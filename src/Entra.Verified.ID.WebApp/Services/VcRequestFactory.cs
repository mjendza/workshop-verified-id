using System.Collections.Generic;
using Entra.Verified.ID.WebApp.Configuration;
using Entra.Verified.ID.WebApp.Models;
using Entra.Verified.ID.WebApp.Models.VcApiContracts;

namespace Entra.Verified.ID.WebApp.Services;

public static class VcRequestFactory
{
    public static VcPresentationRequest CreatePresentationRequest(string userPresentationRequestId,
        AppSettingsModel settings, string path, string apiKey, PresentationRequest requestModel)
    {
        var request = new VcPresentationRequest
        {
            IncludeQrCode = false,
            Authority = settings.VerifierAuthority,
            Registration = new Registration
            {
                ClientName = requestModel.PurposeOfPresentation
            },
            Callback = new Callback
            {
                Url = $"{path}/{settings.PresentationCallbackUrl}",
                State = userPresentationRequestId,
                Headers = new Dictionary<string, string> {{"api-key", apiKey}}
            },
            IncludeReceipt = false,
            RequestedCredentials = new List<RequestedCredential>()
        };
        var cred = new RequestedCredential
        {
            Type = settings.CredentialType,
            //Manifest = settings.CredentialManifestUrl,
            Purpose = requestModel.PurposeOfPresentation
            //AcceptedIssuers = new List<string>(new[] {_appSettings.VerifierAuthority})
        };

        if (requestModel != null && requestModel.FaceCheckEnabled)
        {
            cred.Configuration = new Models.VcApiContracts.Configuration();
            cred.Configuration.Validation.FaceCheck = new FaceCheck
            {
                SourcePhotoClaimName = "photo",
                MatchConfidenceThreshold = 70
            };
        }

        request.RequestedCredentials.Add(cred);

        return request;
    }


    public static VcIssuanceRequest CreateIssuanceRequest(string stateId, Pin pin, AppSettingsModel settings,
        string path, string apiKey)
    {
        var request = new VcIssuanceRequest
        {
            IncludeQrCode = false,
            Authority = settings.IssuerAuthority,
            Registration = new Registration
            {
                ClientName = VcApplicationConfiguration.VcClientName
            },
            Callback = new Callback
            {
                Url = $"{path}/{settings.IssuanceCallbackUrl}",
                State = stateId,
                Headers = new Dictionary<string, string> {{"api-key", apiKey}}
            },
            Type = settings.CredentialType,
            Manifest = settings.CredentialManifestUrl,
            Pin = pin
        };
        return request;
    }
}