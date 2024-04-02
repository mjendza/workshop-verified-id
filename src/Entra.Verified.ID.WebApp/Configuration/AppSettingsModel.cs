namespace Portal.VerifiableCredentials.API.Configuration;

public class EntraServicePrincipal
{
    public string TenantId { get; set; }
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
}

public class AppSettingsModel
{
    public string ApiKeyForVerifiedCredentialsCallback { get; set; }

    public string IssuerAuthority { get; set; }
    public string VerifierAuthority { get; set; }
    public string CredentialType { get; set; }
    public string CredentialManifestUrl { get; set; }
    public string Pin { get; set; }

    public string Domain { get; set; }
    public string IssuanceCallbackUrl { get; set; }
    public string PresentationCallbackUrl { get; set; }
}