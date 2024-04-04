using System.Collections.Generic;
using System.Linq;

namespace Entra.Verified.ID.WebApp.Models.VcApiContracts;

public class Manifest
{
    public Input Input { get; set; }
    public Display Display { get; set; }

    public Dictionary<string, string> GetSelfAssertedClaims()
    {
        var claims = new Dictionary<string, string>();

        if (Input.Attestations.IdTokens == null || !Input.Attestations.IdTokens.Any() ||
            Input.Attestations.IdTokens[0].Id != IdToken.ExpectedIdByTheProcess)
            return claims;
        foreach (var claim in Input.Attestations.IdTokens[0].Claims) claims.Add(claim.Claim, "");
        return claims;
    }
}