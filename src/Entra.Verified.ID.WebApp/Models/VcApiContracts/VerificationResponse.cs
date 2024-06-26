﻿using System.Text.Json.Serialization;

namespace Entra.Verified.ID.WebApp.Models.VcApiContracts;

public class VerificationResponse
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("requestId")] public string RequestId { get; set; }

    public string Url { get; set; }
    public int Expiry { get; set; }
    public string QrCode { get; set; }
}