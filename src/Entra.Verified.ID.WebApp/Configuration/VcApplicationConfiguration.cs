﻿namespace Portal.VerifiableCredentials.API.Configuration;

public static class VcApplicationConfiguration
{
    public static string VcClientName => "Factorlabs";
    public static string VcPurpose => "Hey Factorlabs user! Please share with us your Digital Identity";

    public static string VerifiableCredentialsApiEndpoint =>
        "https://verifiedid.did.msidentity.com/v1.0/verifiableCredentials/";

    public static int IssuancePinCodeLength => 4;
}

public static class VcServicePrincipalConfiguration
{
    public static string VcServiceScope => "3db474b9-6a0c-4840-96ac-1fceb342124f/.default";
    public static string Authority => "https://login.microsoftonline.com/{0}";
}