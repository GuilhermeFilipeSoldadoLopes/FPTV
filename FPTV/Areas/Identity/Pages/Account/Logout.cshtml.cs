// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using FPTV.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FPTV.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Represents the LogoutModel class which is used to handle logout requests.
    /// </summary>
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<UserBase> _signInManager;
        private readonly ILogger<LogoutModel> _logger;

        /// <summary>
        /// Constructor for LogoutModel class.
        /// </summary>
        /// <param name="signInManager">SignInManager object.</param>
        /// <param name="logger">ILogger object.</param>
        /// <returns>
        /// No return value.
        /// </returns>
        public LogoutModel(SignInManager<UserBase> signInManager, ILogger<LogoutModel> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// Logs out the current user and redirects to the specified page.
        /// </summary>
        /// <param name="returnUrl">The URL to redirect to after logging out.</param>
        /// <returns>A redirect to the specified page.</returns>
        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                // This needs to be a redirect so that the browser performs a new
                // request and the identity for the user gets updated.
                return RedirectToPage();
            }
        }
    }
}
