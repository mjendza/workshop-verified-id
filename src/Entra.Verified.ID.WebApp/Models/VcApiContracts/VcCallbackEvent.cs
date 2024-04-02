namespace Portal.VerifiableCredentials.API.Models.VcApiContracts;

public class VcCallbackEvent
{
    public VcCallbackEvent()
    {
    }

    public VcCallbackEvent(string state, ClaimsIssuer[] claims = null)
    {
        State = state;
        VerifiedCredentialsData = claims;
    }

    public string Id { get; set; }
    public string RequestId { get; set; }
    public string RequestStatus { get; set; }
    public Error Error { get; set; }

    /// <summary>
    ///     State of the VC process
    /// </summary>
    public string State { get; set; }

    public string Subject { get; set; }
    public ClaimsIssuer[] VerifiedCredentialsData { get; set; }
    public Receipt Receipt { get; set; }
}