
namespace Portal.VerifiableCredentials.API.Models;

public record IssuanceResponse(string Id, string RequestId, string Url, int Expiry, string QrCode, string Pin, string Debug);