# Start your journey with a Decentralized Identity. Create the first Verifiable Credential with the Entra Verified ID

## Prerequisites
### Technical
- [ ] [Entra ID Tenant](https://www.microsoft.com/en-gb/security/business/identity-access/microsoft-entra-id)
- [ ] Global administrator or the authentication policy administrator permission assigned is required.
- [ ] [Custom Domain is Registratered for tenant](https://learn.microsoft.com/en-us/entra/identity/users/domains-manage)
- [ ] Blob Storage Account to store logo (publicly accessible jpg/png file) - or Netlify site to host the logo (Azure Subscription)

- [ ] Visual Studio 2022 or Jetbrains Rider
- [ ] .NET 7 SDK
- [ ] JavasScript - JQuery and Vanilla JS basics
- [ ] ngrok (for local development) - or Cloudflare tunnel

### Skills
- [ ] basic knowledge of C# and .NET
- [ ] Basic understanding of Service Principal Authentication

## Achievements
- Know how to set up a Verified ID service.
- Design and deploy first digital credentials.

- Modify the sample application to build:
    - setup Service Principal to build Verified ID requests
    - issueance request and pesentation request
    - modify presentation request to require Face Check
    - extend VC with new claims
    - work with multiple issuers
    - check the protocol details during the test cases(DID for holder and issuer)


## Steps
### Create Entra Verified ID
#### Tasks
- Quick setup for tenant (with one click)
- Advanced setup with KeyVault as homework
### Summary
- Entra Verified ID is a global service - one per tenant

### Setup Service Principal to build Verified ID requests
#### Tasks
Official doc [link](https://learn.microsoft.com/en-us/entra/verified-id/verifiable-credentials-configure-tenant#register-an-application-in-microsoft-entra-id)
- Create SP with `VerifiableCredential.Create.All`
- update the `appsettings.json` with the SP details
```json
  "EntraTenantServicePrincipal": {
    "TenantId": "",
    "ClientId": "",
    "ClientSecret": ""
  },
```
### Summary
- Issue|Present VC with the Entra Verified ID without scope - access to all Credentials Types




