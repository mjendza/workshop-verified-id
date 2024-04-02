using System.Collections.Generic;
using Newtonsoft.Json;
using Portal.VerifiableCredentials.API.Models.VcApiContracts;
using Portal.VerifiableCredentials.API.Services;

namespace Portal.VerifiableCredentials.API.Models;

[JsonConverter(typeof(FlatCommonFieldsConverter<HtmlVerificationResult>))]
public record HtmlVerificationResult : WithClaims
{
    public HtmlVerificationResult(VcCallbackEvent fullResult, string vcType, string vcIss, string vcSub, string vcKey,
        IDictionary<string, string> claims, double? faceCheckMatchConfidenceScore) : base(claims)
    {
        VcSub = vcSub;
        VcType = vcType;
        VcKey = vcKey;
        FaceCheckMatchConfidenceScore = faceCheckMatchConfidenceScore;
        VcIss = vcIss;
        FullObject = fullResult;
    }

    public string PhotoBase64 { get; set; }


    public string VcKey { get; init; }
    public double? FaceCheckMatchConfidenceScore { get; }
    public string VcIss { get; init; }
    public string VcSub { get; init; }
    public string VcType { get; init; }

    /// <summary>
    ///     For Debug
    /// </summary>
    public VcCallbackEvent FullObject { get; init; }

    public decimal? AccountBalance { get; set; }
}