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
using System.Diagnostics.Metrics;
using Microsoft.EntityFrameworkCore;

namespace FPTV.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<UserBase> _userManager;
        private readonly SignInManager<UserBase> _signInManager;
        private readonly FPTVContext _context;

        public IndexModel(
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
            [Display(Name = "CountryImage")]
            public string CountryImage { get; set; }

            [Display(Name = "RegistrationDate")]
            public string Date { get; set; }
        }

        private async Task LoadAsync(UserBase user, Profile profile)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var profilePicture = profile.Picture;
            var country = profile.Country;
            var biography = profile.Biography;
            var date = profile.RegistrationDate.Date;

			Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                Username = userName,
                ProfilePicture = profilePicture,
                Country = country,
                Bio = biography,
                CountryImage = "/images/Flags/4x3/" + profile.Country + ".svg",
                Date = ("Member since: " + date.Date.ToShortDateString())
			};
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            var profile = _context.Profiles.Include(p => p.PlayerList.Players).Include(p => p.TeamsList.Teams).Single(p => p.Id == user.ProfileId);

            var players = profile.PlayerList.Players.ToList();
            var teams = profile.TeamsList.Teams.ToList();
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
            ViewData["Topics"] = _context.Topics.Where(t => t.ProfileId == profile.Id).ToList();

            await LoadAsync(user, profile);
            return Page();
        }
    }
}
