using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FPTV.Services.EmailSenderService
{
    /// <summary>
    /// CustomEmailConfirmationTokenProvider is a class that provides a data protector token for a given user of type TUser. 
    /// </summary>
    public class CustomEmailConfirmationTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        /// <summary>
        /// Constructor for CustomEmailConfirmationTokenProvider class.
        /// </summary>
        /// <param name="dataProtectionProvider">Data protection provider.</param>
        /// <param name="options">Options for EmailConfirmationTokenProvider.</param>
        /// <param name="logger">Logger for DataProtectorTokenProvider.</param>
        /// <returns>
        /// An instance of CustomEmailConfirmationTokenProvider.
        /// </returns>
        public CustomEmailConfirmationTokenProvider(
                IDataProtectionProvider dataProtectionProvider,
                IOptions<EmailConfirmationTokenProviderOptions> options,
                ILogger<DataProtectorTokenProvider<TUser>> logger)
                                               : base(dataProtectionProvider, options, logger)
        {

        }
    }
}
