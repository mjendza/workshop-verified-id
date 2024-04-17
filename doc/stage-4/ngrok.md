## Goals
Enable https://localhost:5000 to be accessible from the internet.

There is a callback from Entra Verified ID endpoint - so the API must be public.

## Actions

- download and install ngrok
- Login to the https://ngrok.com
- create edge with endpoint based on the instruction
- run ngrok tunnel pointing to the http://localhost:5000

## Known issues
- with DEV Tunnel (VisualStudio 2022) - 

## Verification Steps
- access api via ngrok tunnel URL

## Screen
![how-to-setup](ngrok-setup-edge.png)