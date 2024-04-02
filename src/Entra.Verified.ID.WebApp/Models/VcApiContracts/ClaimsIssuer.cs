using System.Collections.Generic;

namespace Portal.VerifiableCredentials.API.Models.VcApiContracts;

public class ClaimsIssuer
{
    public string Issuer { get; set; }
    public string Domain { get; set; }
    public string Verified { get; set; }
    public string[] Type { get; set; }

    public CredentialState CredentialState { get; set; }
    public IDictionary<string, string> Claims { get; set; }
    public FaceCheckResult FaceCheck { get; set; }
    public DomainValidation DomainValidation { get; set; }
}

public class DomainValidation
{
    public string Url { get; set; }
}

public class CredentialState
{
    public string RevocationStatus { get; set; }
}

public class FaceCheckResult
{
    public double MatchConfidenceScore { get; set; }
}