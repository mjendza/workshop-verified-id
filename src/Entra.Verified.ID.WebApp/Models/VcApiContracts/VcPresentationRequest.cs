using System.Collections.Generic;

namespace Entra.Verified.ID.WebApp.Models.VcApiContracts;

public class VcPresentationRequest
{
    public string Authority { get; set; }
    public bool IncludeQrCode { get; set; }
    public Registration Registration { get; set; }

    public Callback Callback { get; set; }

    //public Presentation presentation { get; set; }
    public bool IncludeReceipt { get; set; }
    public List<RequestedCredential> RequestedCredentials { get; set; }
}