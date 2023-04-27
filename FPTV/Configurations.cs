using System.Runtime.Intrinsics.X86;
using FPTV.Areas.Identity.Pages.Account;
using FPTV.Data;
using FPTV.Models.UserModels;
using FPTV.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FPTV
{

    //cria as roles e é criado o user com a role de Admin e os Moderadores
    public static class Configurations
    {
        /// <summary>
        /// Creates the roles for the application.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="_context">The context.</param>
        /// <param name="_userStore">The user store.</param>
        /// <param name="_emailStore">The email store.</param>
        /// <param name="env">The environment.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public static async Task CreateRoles(IServiceProvider serviceProvider, FPTVContext _context, IUserStore<UserBase> _userStore, IUserEmailStore<UserBase> _emailStore, IWebHostEnvironment env)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<UserBase>>();
            string[] roleNames = { "Admin", "Moderator", "User" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist) await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await CreateAdmin(roleManager, userManager, _context, _userStore, _emailStore, env);
            await CreateModerators(roleManager, userManager, _context, _userStore, _emailStore, env);
        }

        /// <summary>
        /// Creates an admin user with the specified credentials and adds it to the Admin role.
        /// </summary>
        /// <returns>
        /// A <see cref="Task"/> representing the asynchronous operation.
        /// </returns>
        private static async Task CreateAdmin(RoleManager<IdentityRole> roleManager, UserManager<UserBase> userManager, FPTVContext _context, IUserStore<UserBase> _userStore, IUserEmailStore<UserBase> _emailStore, IWebHostEnvironment env)
        {
            var admin = CreateUser();

            Profile profile = new();
            var adminImage = Path.Combine(env.WebRootPath, "images", "Mods_Image.png");
            profile.Picture = System.IO.File.ReadAllBytes(adminImage);
            profile.User = admin;
            profile.UserId = new Guid(admin.Id);
            profile.RegistrationDate = DateTime.Now;
            profile.Country = "Portugal";
            profile.Flag = "pt";
            admin.Profile = profile;
            _context.Profiles.Add(profile);

            admin.EmailConfirmed = true;

            await _userStore.SetUserNameAsync(admin, "Admin", CancellationToken.None);
            await _emailStore.SetEmailAsync(admin, "Admin@FPTV.org", CancellationToken.None);

            var createPowerUser = await userManager.CreateAsync(admin, "49gH%s7AD54D4GWL");
            if (createPowerUser.Succeeded)
                await userManager.AddToRoleAsync(admin, "Admin");
        }
        /// <summary>
        /// Creates moderators for the application.
        /// </summary>
        /// <param name="roleManager">Role manager.</param>
        /// <param name="userManager">User manager.</param>
        /// <param name="_context">Database context.</param>
        /// <param name="_userStore">User store.</param>
        /// <param name="_emailStore">Email store.</param>
        /// <param name="env">Web host environment.</param>
        /// <returns>
        /// Task representing the asynchronous operation.
        /// </returns>
        private static async Task CreateModerators(RoleManager<IdentityRole> roleManager, UserManager<UserBase> userManager, FPTVContext _context, IUserStore<UserBase> _userStore, IUserEmailStore<UserBase> _emailStore, IWebHostEnvironment env)
        {
            //Imagem de perfil dos moderadores
            string moderatorImage = Path.Combine(env.WebRootPath, "images", "Mods_Image.png");

            //André Dias
            await CreateModeratorAD(moderatorImage, roleManager, userManager, _context, _userStore, _emailStore, env);

            //Guilherme Lopes
            await CreateModeratorGL(moderatorImage, roleManager, userManager, _context, _userStore, _emailStore, env);

            //Miguel Rebelo
            await CreateModeratorMR(moderatorImage, roleManager, userManager, _context, _userStore, _emailStore, env);

            //Nuno Reis
            await CreateModeratorNR(moderatorImage, roleManager, userManager, _context, _userStore, _emailStore, env);

            //Rui Plínio
            await CreateModeratorRP(moderatorImage, roleManager, userManager, _context, _userStore, _emailStore, env);

            //João Afonso
            await CreateModeratorJA(moderatorImage, roleManager, userManager, _context, _userStore, _emailStore, env);
        }

        /// <summary>
        /// Creates a Moderator user with the name André Dias and adds it to the Moderator role.
        /// </summary>
        /// <returns>
        /// A Task that represents the asynchronous operation.
        /// </returns>
        private static async Task CreateModeratorAD(string moderatorImage, RoleManager<IdentityRole> roleManager, UserManager<UserBase> userManager, FPTVContext _context, IUserStore<UserBase> _userStore, IUserEmailStore<UserBase> _emailStore, IWebHostEnvironment env)
        {
            //André Dias
            var moderator_AD = CreateUser();

            Profile profile1 = new();

            profile1.Picture = System.IO.File.ReadAllBytes(moderatorImage);
            profile1.User = moderator_AD;
            profile1.UserId = new Guid(moderator_AD.Id);
            profile1.RegistrationDate = DateTime.Now;
            profile1.Country = "Portugal";
            profile1.Flag = "pt";
            moderator_AD.Profile = profile1;
            _context.Profiles.Add(profile1);

            moderator_AD.EmailConfirmed = true;

            await _userStore.SetUserNameAsync(moderator_AD, "AndreDias", CancellationToken.None);
            await _emailStore.SetEmailAsync(moderator_AD, "201901690@estudantes.ips.pt", CancellationToken.None);

            var createPowerUser0 = await userManager.CreateAsync(moderator_AD, "ModA.1");
            if (createPowerUser0.Succeeded)
                await userManager.AddToRoleAsync(moderator_AD, "Moderator");
        }

        /// <summary>
        /// Creates a Moderator user with the name "V1rtual" and the email "202002400@estudantes.ips.pt"
        /// </summary>
        /// <returns>
        /// The Moderator user created.
        /// </returns>
        private static async Task CreateModeratorGL(string moderatorImage, RoleManager<IdentityRole> roleManager, UserManager<UserBase> userManager, FPTVContext _context, IUserStore<UserBase> _userStore, IUserEmailStore<UserBase> _emailStore, IWebHostEnvironment env)
        {
            //Guilherme Lopes
            var moderator_GL = CreateUser();

            Profile profile2 = new();

            profile2.Picture = System.IO.File.ReadAllBytes(moderatorImage);
            profile2.User = moderator_GL;
            profile2.UserId = new Guid(moderator_GL.Id);
            profile2.RegistrationDate = DateTime.Now;
            profile2.Country = "Portugal";
            profile2.Flag = "pt";
            moderator_GL.Profile = profile2;
            _context.Profiles.Add(profile2);

            moderator_GL.EmailConfirmed = true;

            await _userStore.SetUserNameAsync(moderator_GL, "V1rtual", CancellationToken.None);
            await _emailStore.SetEmailAsync(moderator_GL, "202002400@estudantes.ips.pt", CancellationToken.None);

            var createPowerUser1 = await userManager.CreateAsync(moderator_GL, "PassImpenetravel.123");
            if (createPowerUser1.Succeeded)
                await userManager.AddToRoleAsync(moderator_GL, "Moderator");
        }

        /// <summary>
        /// Creates a Moderator user with the name Miguel Rebelo and adds him to the Moderator role.
        /// </summary>
        /// <returns>
        /// A Task that represents the asynchronous operation.
        /// </returns>
        private static async Task CreateModeratorMR(string moderatorImage, RoleManager<IdentityRole> roleManager, UserManager<UserBase> userManager, FPTVContext _context, IUserStore<UserBase> _userStore, IUserEmailStore<UserBase> _emailStore, IWebHostEnvironment env)
        {
            //Miguel Rebelo
            var moderator_MR = CreateUser();

            Profile profile3 = new();

            profile3.Picture = System.IO.File.ReadAllBytes(moderatorImage);
            profile3.User = moderator_MR;
            profile3.UserId = new Guid(moderator_MR.Id);
            profile3.RegistrationDate = DateTime.Now;
            profile3.Country = "Portugal";
            profile3.Flag = "pt";
            moderator_MR.Profile = profile3;
            _context.Profiles.Add(profile3);

            moderator_MR.EmailConfirmed = true;

            await _userStore.SetUserNameAsync(moderator_MR, "miguelfnr", CancellationToken.None);
            await _emailStore.SetEmailAsync(moderator_MR, "202000568@estudantes.ips.pt", CancellationToken.None);

            var createPowerUser2 = await userManager.CreateAsync(moderator_MR, "EuSouFixe12345.");
            if (createPowerUser2.Succeeded)
                await userManager.AddToRoleAsync(moderator_MR, "Moderator");
        }

        /// <summary>
        /// Creates a Moderator user with the name Nuno Reis.
        /// </summary>
        /// <returns>
        /// Returns a Moderator user with the name Nuno Reis.
        /// </returns>
        private static async Task CreateModeratorNR(string moderatorImage, RoleManager<IdentityRole> roleManager, UserManager<UserBase> userManager, FPTVContext _context, IUserStore<UserBase> _userStore, IUserEmailStore<UserBase> _emailStore, IWebHostEnvironment env)
        {
            //Nuno Reis
            var moderator_NR = CreateUser();

            Profile profile4 = new();

            profile4.Picture = System.IO.File.ReadAllBytes(moderatorImage);
            profile4.User = moderator_NR;
            profile4.UserId = new Guid(moderator_NR.Id);
            profile4.RegistrationDate = DateTime.Now;
            profile4.Country = "Portugal";
            profile4.Flag = "pt";
            moderator_NR.Profile = profile4;
            _context.Profiles.Add(profile4);

            moderator_NR.EmailConfirmed = true;

            await _userStore.SetUserNameAsync(moderator_NR, "nuno33", CancellationToken.None);
            await _emailStore.SetEmailAsync(moderator_NR, "202000753@estudantes.ips.pt", CancellationToken.None);

            var createPowerUser3 = await userManager.CreateAsync(moderator_NR, "PASSDOnuno.123");
            if (createPowerUser3.Succeeded)
                await userManager.AddToRoleAsync(moderator_NR, "Moderator");
        }

        /// <summary>
        /// Creates a Moderator Role with the given parameters.
        /// </summary>
        /// <param name="moderatorImage">The image of the Moderator.</param>
        /// <param name="roleManager">The RoleManager.</param>
        /// <param name="userManager">The UserManager.</param>
        /// <param name="_context">The FPTVContext.</param>
        /// <param name="_userStore">The IUserStore.</param>
        /// <param name="_emailStore">The IUserEmailStore.</param>
        /// <param name="env">The IWebHostEnvironment.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        private static async Task CreateModeratorRP(string moderatorImage, RoleManager<IdentityRole> roleManager, UserManager<UserBase> userManager, FPTVContext _context, IUserStore<UserBase> _userStore, IUserEmailStore<UserBase> _emailStore, IWebHostEnvironment env)
        {
            //Rui Plínio
            var moderator_RP = CreateUser();

            Profile profile5 = new();
            profile5.Picture = System.IO.File.ReadAllBytes(moderatorImage);
            profile5.User = moderator_RP;
            profile5.UserId = new Guid(moderator_RP.Id);
            profile5.RegistrationDate = DateTime.Now;
            profile5.Country = "Portugal";
            profile5.Flag = "pt";
            moderator_RP.Profile = profile5;
            _context.Profiles.Add(profile5);

            moderator_RP.EmailConfirmed = true;

            await _userStore.SetUserNameAsync(moderator_RP, "ruiplinio", CancellationToken.None);
            await _emailStore.SetEmailAsync(moderator_RP, "202002062@estudantes.ips.pt", CancellationToken.None);

            var createPowerUser4 = await userManager.CreateAsync(moderator_RP, "password.FPTV23 ");
            if (createPowerUser4.Succeeded)
                await userManager.AddToRoleAsync(moderator_RP, "Moderator");
        }

        /// <summary>
        /// Creates a Moderator user with the name João Afonso and adds him to the Moderator role.
        /// </summary>
        /// <returns>
        /// A Task that represents the asynchronous operation.
        /// </returns>
        private static async Task CreateModeratorJA(string moderatorImage, RoleManager<IdentityRole> roleManager, UserManager<UserBase> userManager, FPTVContext _context, IUserStore<UserBase> _userStore, IUserEmailStore<UserBase> _emailStore, IWebHostEnvironment env)
        {
            //João Afonso
            var moderator_JA = CreateUser();

            Profile profile6 = new();
            profile6.Picture = System.IO.File.ReadAllBytes(moderatorImage);
            profile6.User = moderator_JA;
            profile6.UserId = new Guid(moderator_JA.Id);
            profile6.RegistrationDate = DateTime.Now;
            profile6.Country = "Portugal";
            profile6.Flag = "pt";
            moderator_JA.Profile = profile6;
            _context.Profiles.Add(profile6);

            moderator_JA.EmailConfirmed = true;

            await _userStore.SetUserNameAsync(moderator_JA, "IzZy", CancellationToken.None);
            await _emailStore.SetEmailAsync(moderator_JA, "202000813@estudantes.ips.pt", CancellationToken.None);

            var createPowerUser5 = await userManager.CreateAsync(moderator_JA, "Joao123.");
            if (createPowerUser5.Succeeded)
                await userManager.AddToRoleAsync(moderator_JA, "Moderator");
        }

        /// <summary>
        /// Creates an instance of the UserBase class.
        /// </summary>
        /// <returns>An instance of the UserBase class.</returns>
        private static UserBase CreateUser()
        {
            try
            {
                return Activator.CreateInstance<UserBase>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(UserBase)}'.");
            }
        }
    }
}
