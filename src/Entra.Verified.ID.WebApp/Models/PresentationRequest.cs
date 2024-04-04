﻿namespace Entra.Verified.ID.WebApp.Models;

public class PresentationRequest
{
    public bool FaceCheckEnabled { get; set; }
    public string PurposeOfPresentation { get; set; }

    public string BankOperation { get; set; }
    public string Receiver { get; set; }
}