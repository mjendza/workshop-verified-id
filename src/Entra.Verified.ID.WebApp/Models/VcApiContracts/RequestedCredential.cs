using System.Collections.Generic;

namespace Portal.VerifiableCredentials.API.Models.VcApiContracts;

public class RequestedCredential
{
    public string Type { get; set; }

    //public string Manifest { get; set; }
    public string Purpose { get; set; }
    public List<string> AcceptedIssuers { get; set; }
    public Configuration Configuration { get; set; }
}

public class FaceCheck
{
    public string SourcePhotoClaimName { get; set; }
    public int MatchConfidenceThreshold { get; set; }
}