using FPTV.Services;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FPTV.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;

    public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
                       ILogger<EmailSender> logger)
    {
        Options = optionsAccessor.Value;
        _logger = logger;
    }

    public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        if (string.IsNullOrEmpty(Options.SendGridKey))
        {
            throw new Exception("Null SendGridKey");
        }
        await Execute(Options.SendGridKey, subject, message, toEmail);
    }

    public async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        Console.WriteLine("ApiKey> " + apiKey);
        Console.WriteLine("subject> " + subject);
        Console.WriteLine("message> " + message);
        Console.WriteLine("toEmail> " + toEmail);

        var client = new SendGridClient(apiKey);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("fptv.org@gmail.com", "FPTV - Email Confirmation"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        // Disable click tracking.
        // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
        msg.SetClickTracking(false, false);
        var response = await client.SendEmailAsync(msg);
        _logger.LogInformation(response.IsSuccessStatusCode
                               ? $"Email to {toEmail} queued successfully!"
                               : $"Failure Email to {toEmail}");
    }
}

/*using FPTV.Services;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace FPTV.Services;

public class EmailSender : IEmailSender
{
    private readonly ILogger _logger;

    public EmailSender(ILogger<EmailSender> logger)
    {
        _logger = logger;
    }

    //public AuthMessageSenderOptions Options { get; } //Set with Secret Manager.

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        Console.WriteLine("Email> " + toEmail);
        Console.WriteLine("subject> " + subject);
        Console.WriteLine("message> " + message);
        //if (string.IsNullOrEmpty(sendGridKey))
        //{
            //throw new Exception("Null SendGridKey");
        //}
        await Execute("", subject, message, toEmail);
    }

    public async Task Execute(string apiKey, string subject, string message, string toEmail)
    {
        var sendGridKey = "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";
        var client = new SendGridClient(sendGridKey);
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("fptv.org@gmail.com", "Confirm Email"),
            Subject = subject,
            PlainTextContent = message,
            //HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail, subject));
        var response = await client.SendEmailAsync(msg);

        // A success status code means SendGrid received the email request and will process it.
        // Errors can still occur when SendGrid tries to send the email. 
        // If email is not received, use this URL to debug: https://app.sendgrid.com/email_activity 
        Console.WriteLine(response.IsSuccessStatusCode ? "Email queued successfully!" : "Something went wrong!");

        /*var from = new EmailAddress("fptv.org@gmail.com", "FPTV - Email Confirmation");
        var to = new EmailAddress(toEmail, subject);
        var plainTextContent = message;
        var htmlContent = message;
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
        var response = await client.SendEmailAsync(msg);*/

/*var client = new SendGridClient(apiKey);
var msg = new SendGridMessage()
{
    From = new EmailAddress("fptv.org@gmail.com", "Confirm Email"),
    Subject = subject,
    PlainTextContent = message,
    HtmlContent = message
};
msg.AddTo(new EmailAddress(toEmail));

// Disable click tracking.
// See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
msg.SetClickTracking(false, false);
var response = await client.SendEmailAsync(msg);
_logger.LogInformation(response.IsSuccessStatusCode
                       ? $"Email to {toEmail} queued successfully!"
                       : $"Failure Email to {toEmail}");*/
//}
//}