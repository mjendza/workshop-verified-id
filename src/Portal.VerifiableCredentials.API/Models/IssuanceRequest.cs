namespace Portal.VerifiableCredentials.API.Models;

public class IssuanceRequest
{
    public string Photo { get; set; }
    public string AccountNumber { get; set; }
    
    //ingredient issuance
    public string CarbonPrintName { get; set; }
    public string CarbonPrintValue { get; set; }
    //B2C
    public string CardNumber { get; set; }
    public string ProductSessionId { get; set; }
}