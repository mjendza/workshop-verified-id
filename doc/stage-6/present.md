## Goals
- issue your credential and store on Microsoft Authenticator App

## Actions
- run the flow to present the credential wihout face check
- run the flow to present the credential with face check - does it work?

## Big Picture
![Flow](flow.png)

### Flow
![Flow](flow-technical.png)

### Update presetation request
please find in the code presetation request add add missing part like on the raw http request below

```json
// POST https://verifiedid.did.msidentity.com/v1.0/verifiableCredentials/createPresentationRequest
...
  "requestedCredentials": [
    {
      "type": "VerifiedEmployee",
      "acceptedIssuers": [ "did:web:yourdomain.com" ],
      "configuration": {
        "validation": {
          "allowRevoked": false,
          "validateLinkedDomain": true,
          "faceCheck": {
            "sourcePhotoClaimName": "photo",
            "matchConfidenceThreshold": 70
          }
        }
```

and update code here:
```csharp
if (requestModel.FaceCheckEnabled)
{
    
}
```

### Trusted issuers
- share your ngrok or cloudflare tunnel with workshop collegue - **should be different issuer-tenant** - check if works

### Limit accepted issuers
[Presentation Request details](https://learn.microsoft.com/en-us/entra/verified-id/get-started-request-api?tabs=http%2Cissuancerequest%2Cpresentationrequest#presentation-request-example)

update the `acceptedIssuers` to limit the accepted issuers to the one you trust
```json
{
  "includeQRCode": true,
   ....
  "includeReceipt": true,
  "requestedCredentials": [
    {
      "acceptedIssuers": [
        "did:web:verifiedid.contoso.com"
      ],
      "configuration": {
        "validation": {
          "allowRevoked": true,
          "validateLinkedDomain": true
        }
      }
    }
  ]
}
```


## Verification Steps
- manual verification case by case

## MS Documentation
https://learn.microsoft.com/en-us/entra/verified-id/get-started-request-api?tabs=http%2Cissuancerequest%2Cpresentationrequest#presentation-request-example

## FaceCheck details ScreenGrab
![screen](screencapture-learn-microsoft-en-us-entra-verified-id-using-facecheck-2024-04-09-16_12_32.png)