using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace FPTV.Services.EmailSenderService
{
    /// <summary>
    /// This class represents a costum email confirmation token provider and is inherit from class DataProtectorTokenProvider
    /// </summary>
    /// <typeparam name="TUser"></typeparam>
    public class CustomEmailConfirmationTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        /// <summary>
        /// This is the contructor of class CustomEmailConfirmationTokenProvider that inherit from class DataProtectorTokenProvider
        /// </summary>
        /// <param name="dataProtectionProvider"></param>
        /// <param name="options"></param>
        /// <param name="logger"></param>
        public CustomEmailConfirmationTokenProvider(IDataProtectionProvider dataProtectionProvider, IOptions<EmailConfirmationTokenProviderOptions> options, ILogger<DataProtectorTokenProvider<TUser>> logger)
            : base(dataProtectionProvider, options, logger)
        {

        }
    }
}
