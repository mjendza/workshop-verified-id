using System.Collections.Generic;

namespace Portal.VerifiableCredentials.API.Models.VcApiContracts;

public class Callback
{
    public string Url { get; set; }
    public string State { get; set; }
    public Dictionary<string, string> Headers { get; set; }
}