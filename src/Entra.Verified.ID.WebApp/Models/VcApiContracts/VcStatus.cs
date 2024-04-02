using System.Text.Json.Serialization;

namespace Entra.Verified.ID.WebApp.Models.VcApiContracts;

public class VcStatus
{
    public VcStatus(int status, string message)
    {
        Status = status;
        Message = message;
    }

    [JsonPropertyName("status")] public int Status { get; set; }

    [JsonPropertyName("message")] public string Message { get; set; }
}