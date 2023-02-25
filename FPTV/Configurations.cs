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
            var userManager = serviceProvider.GetRequiredService<UserManager<UserBase>>();
            string[] roleNames = { "Admin", "Moderator", "User" };

            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist) await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            await CreateAdmin(roleManager, userManager);
            await CreateModerators(roleManager, userManager);
        }

        private static async Task CreateAdmin(RoleManager<IdentityRole> roleManager, UserManager<UserBase> userAdmin)
        {
            var admin = new UserBase
            {
                UserName = "...",
                Email = "Admin@FPTV.org"
            };
            var _user = await userAdmin.FindByEmailAsync(admin.Email);
            if (_user == null)
            {
                var createPowerUser = await userAdmin.CreateAsync(admin, "49gH%s7AD54D4GWL");
                if (createPowerUser.Succeeded)
                    await userAdmin.AddToRoleAsync(admin, "Manager");
            }

        }

        private static async Task CreateModerators(RoleManager<IdentityRole> roleManager, UserManager<UserBase> userManager)
        {
            //André Dias
            var moderator_AD = new UserBase
            {
                UserName = "...",
                Email = "..."
            };
            var _user1 = await userManager.FindByEmailAsync(moderator_AD.Email);
            if (_user1 == null)
            {
                var createPowerUser1 = await userManager.CreateAsync(moderator_AD, "...");
                if (createPowerUser1.Succeeded)
                    await userManager.AddToRoleAsync(moderator_AD, "Manager");
            }


            //Guilherme Lopes
            var moderator_GL = new UserBase
            {
                UserName = "V1rtual",
                Email = "guilherme.lopes20@estudantes.ips.pt"
            };
            var _user2 = await userManager.FindByEmailAsync(moderator_GL.Email);
            if (_user2 == null)
            {

                var createPowerUser2 = await userManager.CreateAsync(moderator_GL, "PassImpenetravel.123");
                if (createPowerUser2.Succeeded)
                    await userManager.AddToRoleAsync(moderator_GL, "Manager");
            }

            //Miguel Rebelo
            var moderator_MR = new UserBase
            {
                UserName = "...",
                Email = "..."
            };
            var _user3 = await userManager.FindByEmailAsync(moderator_MR.Email);
            if (_user3 == null)
            {
                var createPowerUser3 = await userManager.CreateAsync(moderator_MR, "...");
                if (createPowerUser3.Succeeded)
                    await userManager.AddToRoleAsync(moderator_MR, "Manager");
            }

            //Nuno Reis
            var moderator_NR = new UserBase
            {
                UserName = "nuno33",
                Email = "nunoreis294@gmail.com"
            };
            var _user4 = await userManager.FindByEmailAsync(moderator_NR.Email);
            if (_user4 == null)
            {
                var createPowerUser4 = await userManager.CreateAsync(moderator_NR, "...");
                if (createPowerUser4.Succeeded)
                    await userManager.AddToRoleAsync(moderator_NR, "Manager");
            }

            //Rui Plínio
            var moderator_RP = new UserBase
            {
                UserName = "ruiplinio",
                Email = "202002062@estudantes.ips.pt"
            };
            var _user5 = await userManager.FindByEmailAsync(moderator_RP.Email);
            if (_user5 == null)
            {
                var createPowerUser5 = await userManager.CreateAsync(moderator_RP, "...");
                if (createPowerUser5.Succeeded)
                    await userManager.AddToRoleAsync(moderator_RP, "Manager");
            }

            //João Afonso
            var moderator_JA = new UserBase
            {
                UserName = "joaoafonso61",
                Email = "jmrafonso61@gmail.com"
            };
            var _user6 = await userManager.FindByEmailAsync(moderator_JA.Email);
            if (_user6 == null)
            {
                var createPowerUser6 = await userManager.CreateAsync(moderator_JA, "...");
                if (createPowerUser6.Succeeded)
                    await userManager.AddToRoleAsync(moderator_JA, "Manager");
            }
        }
    }
}
