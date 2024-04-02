using System.Collections.Generic;

namespace Portal.VerifiableCredentials.API.Models.VcApiContracts;

public class Attestations
{
    public List<IdToken> IdTokens { get; set; }
    
    public List<IdToken> AccessTokens { get; set; }
}