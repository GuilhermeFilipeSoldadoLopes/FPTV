using FPTV.Models.UserModels;
using Microsoft.AspNetCore.Identity;

namespace FPTV
{
    //cria as roles e é criado o user com a role de Admin 
    public static class Configurations //possivelmente ja nao vai ser utilizado
    {
        public static async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userAdmin = serviceProvider.GetRequiredService<UserManager<Admin>>();
            var userModerator = serviceProvider.GetRequiredService<UserManager<Moderator>>();
            var userUser = serviceProvider.GetRequiredService<UserManager<Profile>>();

            string[] roleNames = { "Admin", "Moderator", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist) await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            var admin = new Admin
            {
                Name = "AdminAccount",
                UserName = "...",
                Email = "Admin@FPTV.org"
            };
            var _user = await userAdmin.FindByEmailAsync(admin.Email);
            if (_user != null) return;
            var createPowerUser = await userAdmin.CreateAsync(admin, "49gH%s7AD54D4GWL");
            if (createPowerUser.Succeeded)
                await userAdmin.AddToRoleAsync(admin, "Manager");
        }
    }
}
