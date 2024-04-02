namespace Entra.Verified.ID.WebApp.Models;

public class IssuanceRequest
{
    public string Photo { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string DateOfBirth { get; set; }
    public string Address { get; set; }
}