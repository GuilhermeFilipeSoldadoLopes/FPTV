// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using FPTV.Models.UserModels;
using FPTV.Data;
using RestSharp;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using static Microsoft.SqlServer.Management.Dmf.ExpressionNodeFunction;
using System.Text.RegularExpressions;

namespace FPTV.Areas.Identity.Pages.Account.Manage
{
    /// <summary>
    /// This class is used to handle the edit page of the application.
    /// </summary>
    public class EditModel : PageModel
    {
        private readonly UserManager<UserBase> _userManager;
        private readonly SignInManager<UserBase> _signInManager;
        private readonly FPTVContext _context;

        /// <summary>
        /// Constructor for EditModel class.
        /// </summary>
        /// <param name="userManager">UserManager object.</param>
        /// <param name="signInManager">SignInManager object.</param>
        /// <param name="context">FPTVContext object.</param>
        /// <returns>
        /// An instance of EditModel class.
        /// </returns>
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

            [Required(ErrorMessage = "Please enter Username")]
            [StringLength(30, MinimumLength = 3, ErrorMessage = "The username must be at least 3 and at max 30 characters long")]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Display(Name = "Profile Picture")]
            public byte[] ProfilePicture { get; set; }

            [Display(Name = "Country")]
            public string Country { get; set; }
        }

        /// <summary>
        /// Loads the user's profile information into the InputModel.
        /// </summary>
        /// <param name="user">The user whose profile information is being loaded.</param>
        /// <param name="profile">The profile information of the user.</param>
        /// <returns>
        /// The user's profile information loaded into the InputModel.
        /// </returns>
        private async Task LoadAsync(UserBase user, Profile profile)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var profilePicture = profile.Picture;
            var country = profile.Country;
            var biography = profile.Biography;

            Username = userName;

            Input = new InputModel
            {
                Username = userName,
                ProfilePicture = profilePicture,
                Country = country,
                Bio = biography
            };
        }

        /// <summary>
        /// Loads the user, profile, and list of countries to the page. Also loads the favorite players and teams for CSGO and Valorant.
        /// </summary>
        /// <returns>Page result</returns>
        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var profile = _context.Profiles.Include(p => p.PlayerList.Players).Include(p => p.TeamsList.Teams).Single(p => p.Id == user.ProfileId);

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
                var country = (JObject)item.GetValue("name");
                var countryName = country.GetValue("common");

                var name = countryName.ToString() == null ? "undefined" : countryName.Value<string>();

                if (countryName != null && name != profile.Country)
                    countries.Add(countryName.Value<string>());
            }

            var csPlayers = new List<Player>();
            var csTeams = new List<Team>();
            var valPlayers = new List<Player>();
            var valTeams = new List<Team>();

            if (profile.PlayerList == null)
            {
                profile.PlayerList = new FavPlayerList();
                profile.PlayerList.Profile = profile;
                profile.PlayerList.ProfileId = user.ProfileId;
                profile.PlayerList.Players = new List<Player>();
            }
            else
            {
                var players = profile.PlayerList.Players.ToList();

                foreach (var item in players)
                {
                    if (item.Game == GameType.CSGO)
                    {
                        csPlayers.Add(item);
                    }
                    else
                    {
                        valPlayers.Add(item);
                    }
                }
            }

            if (profile.TeamsList == null)
            {
                profile.TeamsList = new FavTeamsList();
                profile.TeamsList.Profile = profile;
                profile.TeamsList.ProfileId = user.ProfileId;
                profile.TeamsList.Teams = new List<Team>();
            }
            else
            {
                var teams = profile.TeamsList.Teams.ToList();

                foreach (var item in teams)
                {
                    if (item.Game == GameType.CSGO)
                    {
                        csTeams.Add(item);
                    }
                    else
                    {
                        valTeams.Add(item);
                    }
                }
            }

            _context.SaveChanges();

            ViewData["FavCSPlayerList"] = csPlayers;
            ViewData["FavCSTeamsList"] = csTeams;
            ViewData["FavValPlayerList"] = valPlayers;
            ViewData["FavValTeamsList"] = valTeams;
            ViewData["Countries"] = countries.OrderBy(c => c).ToList();

            await LoadAsync(user, profile);
            return Page();
        }

        /// <summary>
        /// Updates the user profile with the given information.
        /// </summary>
        /// <returns>
        /// Redirects to the Index page with a status message.
        /// </returns>
        public async Task<IActionResult> OnPostAsync()
        {
            //Console.WriteLine("\n\n\n\nEntrou\n\n\n\n");
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

            var country = Request.Form["Country"].ToString();
            if (profile.Country != country && profile.Country != null)
            {
                profile.Country = country;

                var client = new RestClient("https://restcountries.com/v3.1/name/" + country);
                var request = new RestRequest("", Method.Get);
                request.AddHeader("accept", "application/json");
                var json = client.Execute(request).Content;

                List<string> countries = new List<string>();

                var countriesArray = JArray.Parse(json);

                foreach (var item in countriesArray.Cast<JObject>())
                {
                    var countryCode = item.GetValue("cca2");

                    if (countryCode != null)
                        countries.Add(countryCode.Value<string>());
                }

                profile.Flag = countries[0];

                await _context.SaveChangesAsync();
            }

            var biography = profile.Biography;
            if (Input.Bio != biography && Input.Bio != null)
            {
                profile.Biography = Input.Bio;
                await _context.SaveChangesAsync();
            }

            if (Request.Form.Files.Count > 0)
            {
                IFormFile file = Request.Form.Files.FirstOrDefault();
                const int ImageMinimumBytes = 512;

                //-------------------------------------------
                //  Check the image mime types
                //-------------------------------------------
                if (file.ContentType.ToLower() != "image/jpg" &&
                            file.ContentType.ToLower() != "image/jpeg" &&
                            file.ContentType.ToLower() != "image/pjpeg" &&
                            file.ContentType.ToLower() != "image/gif" &&
                            file.ContentType.ToLower() != "image/x-png" &&
                            file.ContentType.ToLower() != "image/png")
                { }
                else
                {
                    //-------------------------------------------
                    //  Check the image extension
                    //-------------------------------------------
                    if (Path.GetExtension(file.FileName).ToLower() != ".jpg"
                        && Path.GetExtension(file.FileName).ToLower() != ".png"
                        && Path.GetExtension(file.FileName).ToLower() != ".gif"
                        && Path.GetExtension(file.FileName).ToLower() != ".jpeg")
                    { }
                    else
                    {
                        //-------------------------------------------
                        //  Attempt to read the file and check the first bytes
                        //-------------------------------------------
                        try
                        {
                            if (!file.OpenReadStream().CanRead)
                            { }
                            else
                            {
                                //------------------------------------------
                                //check whether the image size exceeding the limit or not
                                //------------------------------------------ 
                                if (file.Length < ImageMinimumBytes)
                                { }
                                else
                                {
                                    byte[] buffer = new byte[ImageMinimumBytes];
                                    file.OpenReadStream().Read(buffer, 0, ImageMinimumBytes);
                                    string content = System.Text.Encoding.UTF8.GetString(buffer);
                                    if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                                        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                                    { }
                                    else
                                    {
                                        //-------------------------------------------
                                        //  Try to instantiate new Bitmap, if .NET will throw exception
                                        //  we can assume that it's not a valid image
                                        //-------------------------------------------

                                        try
                                        {
                                            using (var bitmap = new System.Drawing.Bitmap(file.OpenReadStream()))
                                            {
                                            }
                                        }
                                        catch (Exception)
                                        { }
                                        finally
                                        {
                                            file.OpenReadStream().Position = 0;
                                        }

                                        using (var dataStream = new MemoryStream())
                                        {
                                            await file.CopyToAsync(dataStream);
                                            profile.Picture = dataStream.ToArray();
                                        }
                                        await _context.SaveChangesAsync();
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        { }
                    }
                }
            }

            var userName = user.UserName;

            if (Input.Username != userName)
            {
                if (Input.Username != null && Input.Username != "")
                {
                    var userExists = await _userManager.FindByNameAsync(Input.Username);
                    if (userExists == null)
                    {
                        user.UserName = Input.Username;
                        await _userManager.UpdateAsync(user);
                    }
                    else
                    {
                        await OnGetAsync();
                        StatusMessage = "Your profile has not been updated. Already exists a user with that username. Try another one.";
                        Username = userName;
                        Input.Username = userName;
                        return Page();
                    }
                }
                else
                {
                    await OnGetAsync();
                    StatusMessage = "Your profile has not been updated. Your username is same as before";
                    Username = userName;
                    Input.Username = userName;
                    return Page();
                }
            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage("Index");
        }
    }
}