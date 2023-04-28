namespace FPTV.Services.EmailSenderService;

/// <summary>
/// Represents the options for configuring the AuthMessageSender service.
/// </summary>
public class AuthMessageSenderOptions
{
    /// <summary>
    /// Property to store the SendGrid API key.
    /// </summary>
    public string? SendGridKey { get; set; }
}

/*public class AuthMessageSenderOptions
{
    public string? SendGridKey
    {
        get { return "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"; }
        set { }
    }
}*/