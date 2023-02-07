using Microsoft.AspNetCore.Identity;

namespace FPTV.Services.EmailSenderService
{
    /// <summary>
    /// This class represents a email confirmation token provider options and is inherit from class DataProtectionTokenProviderOptions
    /// </summary>
    public class EmailConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        /// <summary>
        /// This method will change the email token lifespan period to 4 hours
        /// </summary>
        public EmailConfirmationTokenProviderOptions()
        {
            Name = "EmailDataProtectorTokenProvider";
            TokenLifespan = TimeSpan.FromHours(4);
        }
    }
}
//provavelmente é para sair