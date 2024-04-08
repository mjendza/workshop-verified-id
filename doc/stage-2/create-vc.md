## Goals
- create new credential based on the `Workshop User Identity Card` schema

## Actions
Based on my sample `iam\workshop-verified-id\src\Entra.Verified.ID\factorlabs-face-check`
### DisplayDefinition.json 
Copy the file to new one and modify the content:
- change the title, colors,
- review claims
### RulesDefinition.json
Copy the file to new one and modify the content:
- check and set validityInterval defined in seconds
- define new VC type name (for the shared tenant use a unique name with the prefix)


## Verification Steps
- deployed new credential to the service
- review manifest in the Azure Portal

## MS Documentation
https://learn.microsoft.com/en-us/entra/verified-id/verifiable-credentials-configure-issuer#create-the-verified-credential-expert-card-in-azure

