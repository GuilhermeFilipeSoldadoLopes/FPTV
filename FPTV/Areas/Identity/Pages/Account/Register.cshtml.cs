// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using FPTV.Data;
using FPTV.Models.UserModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace FPTV.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<UserBase> _signInManager;
        private readonly UserManager<UserBase> _userManager;
        private readonly IUserStore<UserBase> _userStore;
        private readonly IUserEmailStore<UserBase> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly FPTVContext _context;
        private readonly IWebHostEnvironment _env;

        public RegisterModel(
            UserManager<UserBase> userManager,
            IUserStore<UserBase> userStore,
            SignInManager<UserBase> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            FPTVContext context,
            IWebHostEnvironment env)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
            _env = env;
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
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            [Required(ErrorMessage = "Please enter Username")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "The username must be at least 3 and at max 30 characters long")]
            [Display(Name = "Username")]
            public string Username { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Please enter email address")]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Please enter password")]
            [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@#$%._-]).{6,25}$", ErrorMessage = "The password must be at least 6 and at max 25 characters long and should have at least one lower case [a-z], one upper case [A-Z], one number and one special character [@#$%._-].")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required(ErrorMessage = "Please enter confirm password")]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var userName = Input.Username;
                var email = Input.Email;

                if (userName != null && userName != "")
                {
                    var userExists = await _userManager.FindByNameAsync(userName);
                    if (userExists == null)
                    {
                        if (email != null && email != "")
                        {
                            var emailExists = await _userManager.FindByEmailAsync(email);
                            if (emailExists == null)
                            {
                                var user = CreateUser();

                                Profile profile = new();
                                var defaultImage = Path.Combine(_env.WebRootPath, "images", "default-profile-icon-24.jpg");
                                profile.Picture = System.IO.File.ReadAllBytes(defaultImage);
                                profile.User = user;
                                profile.UserId = new Guid(user.Id);
                                profile.RegistrationDate = DateTime.Now;
                                profile.Country = "Portugal";
                                profile.Flag = "pt";
                                user.Profile = profile;
                                _context.Profiles.Add(profile);

                                await _userStore.SetUserNameAsync(user, Input.Username, CancellationToken.None);
                                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                                var result = await _userManager.CreateAsync(user, Input.Password);

                                if (result.Succeeded)
                                {
                                    _logger.LogInformation("User created a new account with password.");

                                    var userId = await _userManager.GetUserIdAsync(user);
                                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                                    var callbackUrl = Url.Page(
                                        "/Account/ConfirmEmail",
                                        pageHandler: null,
                                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                                        protocol: Request.Scheme);

                                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                                        $"{HtmlEncoder.Default.Encode(callbackUrl)}");

                                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                                    {
                                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                                    } else {
                                        await _signInManager.SignInAsync(user, isPersistent: false);
                                        return LocalRedirect(returnUrl);
                                    }
                                }
                                foreach (var error in result.Errors)
                                {
                                    ModelState.AddModelError(string.Empty, error.Description);
                                }
                            }
                        } else {
                            ModelState.AddModelError("CustomErrorEmail", "That email is already registed. Try another one.");
                        }
                    }
                } else {
                    ModelState.AddModelError("CustomErrorUsername", "Already exists a user with that username. Try another one.");
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private UserBase CreateUser()
        {
            try
            {
                return Activator.CreateInstance<UserBase>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(UserBase)}'. " +
                    $"Ensure that '{nameof(UserBase)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<UserBase> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<UserBase>)_userStore;
        }
    }
}

/*
 public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var userName = Input.Username;
                var password = Input.Password;

                if (userName != null && userName != "") 
                {
                    var userExists = await _userManager.FindByNameAsync(userName);
                    if (userExists == null) 
                    {
                        if (password != null && password != "")
                        {
                            if (password.Length <= 20 && password.Length >= 6)
                            {
                                var user = CreateUser();

                                Profile profile = new();
                                var defaultImage = Path.Combine(_env.WebRootPath, "images", "default-profile-icon-24.jpg");
                                profile.Picture = System.IO.File.ReadAllBytes(defaultImage);
                                profile.User = user;
                                profile.UserId = new Guid(user.Id);
                                profile.RegistrationDate = DateTime.Now;
                                profile.Country = "pt";
                                user.Profile = profile;
                                _context.Profiles.Add(profile);

                                await _userStore.SetUserNameAsync(user, Input.Username, CancellationToken.None);
                                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                                var result = await _userManager.CreateAsync(user, Input.Password);

                                if (result.Succeeded)
                                {
                                    _logger.LogInformation("User created a new account with password.");

                                    var userId = await _userManager.GetUserIdAsync(user);
                                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                                    var callbackUrl = Url.Page(
                                        "/Account/ConfirmEmail",
                                        pageHandler: null,
                                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                                        protocol: Request.Scheme);

                                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                                        $"{HtmlEncoder.Default.Encode(callbackUrl)}");

                                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                                    {
                                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                                    }
                                    else
                                    {
                                        await _signInManager.SignInAsync(user, isPersistent: false);
                                        return LocalRedirect(returnUrl);
                                    }
                                }
                                foreach (var error in result.Errors)
                                {
                                    ModelState.AddModelError(string.Empty, error.Description);
                                }
                            } else {
                                ModelState.AddModelError("CustomErrorPassword", "Your password should have between [6, 25] characters. (lowerCase, upperCase and special symbols)");
                            }
                        } else {
                            ModelState.AddModelError("CustomErrorPassword", "Please insert your password.");
                        }
                    } else {
                        ModelState.AddModelError("CustomErrorUsername", "Already exists a user with that UserName. Try another one.");
                    }
                } else {
                    ModelState.AddModelError("CustomErrorUsername", "Please insert your UserName.");
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
 */