## Goals
- issue your credential and store on Microsoft Authenticator App

## Actions
- run the flow to present the credential wihout face check
- run the flow to present the credential with face check - does it work? 

### update presetation request
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

### trusted issuers
- share your ngrok or cloudflare tunnel with workshop collegue - **should be different issuer-tenant** - check if works

### limit accepted issuers
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
![screen](screencapture-learn-microsoft-en-us-entra-verified-id-get-started-request-api-2024-04-09-15_04_07.png)