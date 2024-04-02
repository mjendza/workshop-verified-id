namespace Entra.Verified.ID.WebApp.Models.VcApiContracts;

public class Validation
{
    public bool AllowRevoked { get; set; } // default false
    public bool ValidateLinkedDomain { get; set; } // default false
    public FaceCheck FaceCheck { get; set; }
}