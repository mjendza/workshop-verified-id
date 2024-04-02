using System.Collections.Generic;

namespace Entra.Verified.ID.WebApp.Models.VcApiContracts;

public class Callback
{
    public string Url { get; set; }
    public string State { get; set; }
    public Dictionary<string, string> Headers { get; set; }
}