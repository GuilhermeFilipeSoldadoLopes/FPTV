namespace FPTV.Services.EmailSenderService;

/// <summary>
/// This class contains the method to obtain the SendGrid key
/// </summary>
public class AuthMessageSenderOptions
{
    /// <summary>
    /// Returns the SendGrid key that is stored as a secret
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