using Microsoft.AspNetCore.Identity;

namespace FPTV.Services.EmailSenderService
{
    /// <summary>
    /// Represents the options used to configure the EmailConfirmationTokenProvider.
    /// </summary>
    public class EmailConfirmationTokenProviderOptions : DataProtectionTokenProviderOptions
    {
        /// <summary>
        /// Constructor for EmailConfirmationTokenProviderOptions class. Sets the Name to "EmailDataProtectorTokenProvider" and TokenLifespan to 4 hours.
        /// </summary>
        /// <returns>
        /// An instance of EmailConfirmationTokenProviderOptions class.
        /// </returns>
        public EmailConfirmationTokenProviderOptions()
        {
            Name = "EmailDataProtectorTokenProvider";
            TokenLifespan = TimeSpan.FromHours(4);
        }
    }
}