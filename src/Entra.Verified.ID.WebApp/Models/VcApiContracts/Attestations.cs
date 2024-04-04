using System.Collections.Generic;

namespace Entra.Verified.ID.WebApp.Models.VcApiContracts;

public class Attestations
{
    public List<IdToken> IdTokens { get; set; }

    public List<IdToken> AccessTokens { get; set; }
}