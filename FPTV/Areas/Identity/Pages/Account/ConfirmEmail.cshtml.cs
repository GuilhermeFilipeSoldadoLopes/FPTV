// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FPTV.Models.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace FPTV.Areas.Identity.Pages.Account
{
    /// <summary>
    /// This class is used to confirm the email address of the user.
    /// </summary>
    public class ConfirmEmailModel : PageModel
    {
        private readonly UserManager<UserBase> _userManager;

        /// <summary>
        /// Constructor for ConfirmEmailModel class. 
        /// </summary>
        /// <param name="userManager">UserManager object</param>
        /// <returns>
        /// No return value. 
        /// </returns>
        public ConfirmEmailModel(UserManager<UserBase> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        /// Confirms the email of a user with the given userId and code.
        /// </summary>
        /// <param name="userId">The userId of the user to confirm.</param>
        /// <param name="code">The code used to confirm the user.</param>
        /// <returns>The page with a status message indicating the result of the confirmation.</returns>
        public async Task<IActionResult> OnGetAsync(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            StatusMessage = result.Succeeded ? "Thank you for confirming your email." : "Error confirming your email.";
            return Page();
        }
    }
}
