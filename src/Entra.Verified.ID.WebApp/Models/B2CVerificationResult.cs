using System;
using System.Collections.Generic;
using Entra.Verified.ID.WebApp.Services;
using Newtonsoft.Json;

namespace Entra.Verified.ID.WebApp.Models;

[JsonConverter(typeof(FlatCommonFieldsConverter<B2CVerificationResult>))]
public record B2CVerificationResult : WithClaims
{
    public B2CVerificationResult(string vcType, string vcIss, string vcSub, string vcKey,
        IDictionary<string, string> claims) : base(claims)
    {
        VcSub = vcSub;
        VcType = vcType;
        VcKey = vcKey;
        VcIss = vcIss;
    }


    public string VcKey { get; init; }
    public string VcIss { get; init; }
    public string VcSub { get; init; }
    public string VcType { get; init; }
}

public record WithClaims
{
    public WithClaims(IDictionary<string, string> claims)
    {
        Claims = claims;
    }

    protected WithClaims()
    {
        throw new NotImplementedException();
    }

    [JsonIgnore] public IDictionary<string, string> Claims { get; init; }
}