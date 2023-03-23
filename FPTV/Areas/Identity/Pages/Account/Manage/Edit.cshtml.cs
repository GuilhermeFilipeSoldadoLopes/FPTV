// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FPTV.Models.UserModels;
using FPTV.Data;
using RestSharp;
using FPTV.Models.MatchesModels;
using Newtonsoft.Json.Linq;
using System.Linq;
using AngleSharp.Common;

namespace FPTV.Areas.Identity.Pages.Account.Manage
{
    public class EditModel : PageModel
    {
        private readonly UserManager<UserBase> _userManager;
        private readonly SignInManager<UserBase> _signInManager;
        private readonly FPTVContext _context;

        public EditModel(
            UserManager<UserBase> userManager,
            SignInManager<UserBase> signInManager,
            FPTVContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

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
            [Display(Name = "Bio")]
            public string Bio { get; set; }
            [Display(Name = "Username")]
            public string Username { get; set; }
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
            [Display(Name = "Profile Picture")]
            public byte[] ProfilePicture { get; set; }
            [Display(Name = "Country")]
            public string Country { get; set; }
        }

        private async Task LoadAsync(UserBase user, Profile profile)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var profilePicture = profile.Picture;
            var country = profile.Country;
            var biography = profile.Biography;

            Username = userName;

            var client = new RestClient("https://restcountries.com/v3.1/alpha/" + country);
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            var json = client.Execute(request).Content;

            List<string> countries = new List<string>();

            var countriesArray = JArray.Parse(json);

            foreach (var item in countriesArray.Cast<JObject>())
            {
                var countryObject = (JObject)item.GetValue("name");
                var countryName = countryObject.GetValue("common");

                if (countryName != null)
                    countries.Add(countryName.Value<string>());
            }

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Username = userName,
                ProfilePicture = profilePicture,
                Country = countries[0],
                Bio = biography
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }
            var profile = _context.Profiles.Single(p => p.Id == user.ProfileId);

            var client = new RestClient("https://restcountries.com/v3.1/all");
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            var json = client.Execute(request).Content;

            if (json == null)
            {
                return null;
            }

            List<string> countries = new List<string>();

            var countriesArray = JArray.Parse(json);

            foreach (var item in countriesArray.Cast<JObject>())
            {
                var countryCode = item.GetValue("cca2");
                var country = (JObject)item.GetValue("name");
                var countryName = country.GetValue("common");

                if (countryName != null && countryCode.ToString().ToLower() != profile.Country)
                    countries.Add(countryName.Value<string>());
            }

            ViewData["Countries"] = countries.OrderBy(c => c).ToList();
            ViewData["FavPlayerListCSGO"] = _context.FavPlayerList.Where(fpl => fpl.ProfileId == profile.Id).ToList();
            ViewData["FavTeamsListCSGO"] = _context.FavTeamsList.Where(ftl => ftl.ProfileId == profile.Id).ToList();
            ViewData["FavPlayerListValorant"] = _context.FavPlayerList.Where(fpl => fpl.ProfileId == profile.Id).ToList();
            ViewData["FavTeamsListValorant"] = _context.FavTeamsList.Where(ftl => ftl.ProfileId == profile.Id).ToList();

            await LoadAsync(user, profile);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Console.WriteLine("\n\n\n\nEntrou\n\n\n\n");
            var user = await _userManager.GetUserAsync(User);
            var profile = _context.Profiles.Single(p => p.Id == user.ProfileId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user, profile);
                return Page();
            }

            var userName = user.UserName;
            if (Input.Username != userName)
            {
                user.UserName = Input.Username;
                await _userManager.UpdateAsync(user);
            }

            var formCountry = Request.Form["Country"].ToString();

            var client = new RestClient("https://restcountries.com/v3.1/name/" + formCountry);
            var request = new RestRequest("", Method.Get);
            request.AddHeader("accept", "application/json");
            var json = client.Execute(request).Content;

            if (json == null)
            {
                return null;
            }

            List<string> countries = new List<string>();

            var countriesArray = JArray.Parse(json);

            foreach (var item in countriesArray.Cast<JObject>())
            {
                var countryCode = item.GetValue("cca2");

                if (countryCode != null)
                    countries.Add(countryCode.Value<string>());
            }

            var country = countries[0];
            if (Input.Country != country)
            {
                profile.Country = country;
                await _context.SaveChangesAsync();
            }

            var biography = profile.Biography;
            if (Input.Bio != biography)
            {
                profile.Biography = Input.Bio;
                await _context.SaveChangesAsync();
            }

            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                using (var dataStream = new MemoryStream())
                {
                    await file.CopyToAsync(dataStream);
                    profile.Picture = dataStream.ToArray();
                }
                await _context.SaveChangesAsync();
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage("Index");
        }
    }
}