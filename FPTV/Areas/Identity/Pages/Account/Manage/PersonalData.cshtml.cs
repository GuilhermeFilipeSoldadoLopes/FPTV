// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
using System;
using System.Threading.Tasks;
using FPTV.Models.UserModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace FPTV.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// This class represents the PersonalDataModel which is used to store personal data.
    /// </summary>
    public class PersonalDataModel : PageModel
    {
        private readonly UserManager<UserBase> _userManager;
        private readonly ILogger<PersonalDataModel> _logger;

        /// <summary>
        /// Constructor for PersonalDataModel class.
        /// </summary>
        /// <param name="userManager">UserManager object.</param>
        /// <param name="logger">ILogger object.</param>
        /// <returns>
        /// No return value.
        /// </returns>
        public PersonalDataModel(
                    UserManager<UserBase> userManager,
                    ILogger<PersonalDataModel> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        /// <summary>
        /// Gets the current user from the UserManager and returns the page.
        /// </summary>
        /// <returns>The page for the current user.</returns>
        public async Task<IActionResult> OnGet()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            return Page();
        }
    }
}
