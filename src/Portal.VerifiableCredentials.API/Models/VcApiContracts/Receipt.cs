﻿namespace Portal.VerifiableCredentials.API.Models.VcApiContracts;

public class Receipt
{
    public string id_token { get; set; }
    public string state { get; set; }
    public string vp_token { get; set; }
}