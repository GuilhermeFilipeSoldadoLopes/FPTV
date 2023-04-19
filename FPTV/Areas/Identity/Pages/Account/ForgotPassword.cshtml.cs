// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AngleSharp.Dom;
using FPTV.Models.UserModels;
using FPTV.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace FPTV.Areas.Identity.Pages.Account
{
    /// <summary>
    /// Represents the model for the Forgot Password page.
    /// </summary>
    public class ForgotPasswordModel : PageModel
    {
        private readonly UserManager<UserBase> _userManager;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Constructor for ForgotPasswordModel class.
        /// </summary>
        /// <param name="userManager">UserManager object.</param>
        /// <param name="emailSender">IEmailSender object.</param>
        /// <returns>
        /// ForgotPasswordModel object.
        /// </returns>
        public ForgotPasswordModel(UserManager<UserBase> userManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _emailSender = emailSender;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }
        }

        /// <summary>
        /// Generates a password reset token and sends an email to the user with a link to reset their password.
        /// </summary>
        /// <returns>Redirects to the ForgotPasswordConfirmation page.</returns>
        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                ////testar
                //var user = await _userManager.FindByEmailAsync(Input.Email);
                //var result = await _userManager.ConfirmEmailAsync(user, code);
                //if (user == null && result.Succeeded)
                //{
                //    // Don't reveal that the user does not exist or is not confirmed
                //    return RedirectToPage("./ForgotPasswordConfirmation");
                //}

                //var result = await _userManager.ConfirmEmailAsync(user, code);


                //var user = await _userManager.FindByEmailAsync(Input.Email);
                //var result = await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                //if (result.Succeeded)
                //{

                // For more information on how to enable account confirmation and password reset please
                // visit https://go.microsoft.com/fwlink/?LinkID=532713
                var user = await _userManager.FindByEmailAsync(Input.Email);

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ResetPassword",
                    pageHandler: null,
                    values: new { area = "Identity", code = code },
                    protocol: Request.Scheme);

                await _emailSender.SendEmailAsync(
                    Input.Email,
                    "Reset your Password",
                    $"{HtmlEncoder.Default.Encode(callbackUrl)}");

                return RedirectToPage("./ForgotPasswordConfirmation");
            }

            return Page();
        }
    }
}
