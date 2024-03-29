using System.Collections.Generic;

namespace Portal.VerifiableCredentials.API.Models.VcApiContracts;

public class IdToken
{
    public string Id { get; set; }
    public static string ExpectedIdByTheProcess => "https://self-issued.me";
    public List<ClaimDefinition> Claims { get; set; }
}