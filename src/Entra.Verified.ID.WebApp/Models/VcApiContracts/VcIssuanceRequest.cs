using System.Collections.Generic;

namespace Portal.VerifiableCredentials.API.Models.VcApiContracts;

public class VcIssuanceRequest
{
    public Dictionary<string, string> Claims;
    public string Authority { get; set; }
    public bool IncludeQrCode { get; set; }
    public Registration Registration { get; set; }
    public Callback Callback { get; set; }
    public string Type { get; set; }
    public string Manifest { get; set; }
    public Pin Pin { get; set; }
}