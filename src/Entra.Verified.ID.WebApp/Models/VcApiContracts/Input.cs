namespace Portal.VerifiableCredentials.API.Models.VcApiContracts;

public class Input
{
    public string Issuer { get; set; }
    public Attestations Attestations { get; set; }
}