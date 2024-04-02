    "ApiEndpoint": "https://verifiedid.did.msidentity.com/v1.0/verifiableCredentials/",
    "TenantId": "<your-AAD-tenant-for-VC>",
    "Authority": "https://login.microsoftonline.com/{0}",
    "scope": "3db474b9-6a0c-4840-96ac-1fceb342124f/.default",
    "ClientId": "<your-clientid-with-API-Permissions->",
    "ClientSecret": "your-secret",
    "VerifierAuthority": "did:ion:...your DID...",
    "IssuerAuthority": "did:ion:...your DID...",
    "B2C1ARestApiKey": "your-b2c-app-key",
    "CredentialType": "B2CVerifiedAccount",
    "DidManifest": "https://verifiedid.did.msidentity.com/v1.0/<your-tenant-id-for-VC>/verifiableCredential/contracts/<your-name>",
    "IssuancePinCodeLength": 0

```
- **ApiEndpoint** - Request Service API endpoint
- **TenantId** - This is the Azure AD tenant that you have setup Verifiable Credentials in. It is not the B2C tenant.
- **ClientId** - This is the App you have registered that has the VC permission `VerifiableCredential.Create.All` and that has access to your VC Azure KeyVault.
- **VerifierAuthority** - This DID for your Azure AD tenant. You can find in your VC blade in portal.azure.com.
- **IssuerAuthority** - This DID for your Azure AD tenant. You can find in your VC blade in portal.azure.com.
- **CredentialType** - Whatever you have as type in the Rules file(s). The default is `B2CVerifiedAccount`.
- **DidManifest**- The complete url to the DID manifest. It is used to set the attribute `manifest` and it is used for both issuance and presentation.
- **IssuancePinCodeLength** - If you want your issuance process to use the pin code method, you specify how many digits the pin code should have. A value of zero will not use the pin code method.

### Standalone
To run the sample standalone, just clone the repository, compile & run it. It's callback endpoint must be publically reachable, and for that reason, use `ngrok` as a reverse proxy to read your app.

## TODO List
- move the website (wwwroot: index, issuer) into dedicated Frontend Application
- Azure B2C Is not authenticated 