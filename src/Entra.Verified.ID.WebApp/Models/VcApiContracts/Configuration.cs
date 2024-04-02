namespace Portal.VerifiableCredentials.API.Models.VcApiContracts;

public class Configuration
{
    public Configuration()
    {
        Validation = new Validation();
    }
    public Validation Validation { get; set; }
}