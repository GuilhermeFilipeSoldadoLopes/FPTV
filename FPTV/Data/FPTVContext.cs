using FPTV.Models.UserModels;
using Microsoft.EntityFrameworkCore;

namespace FPTV.Data
{
    public class FPTVContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<UserBase, Microsoft.AspNetCore.Identity.IdentityRole, string>
    {

        public FPTVContext(DbContextOptions<FPTVContext> options)
            : base(options)
        {
        }

        public DbSet<UserBase> UserBase { get; set; }
        /*public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<AuthenticationLog> AuthenticationLog { get; set; }
        public DbSet<Token> Token { get; set; }
        public DbSet<Mail> Mail { get; set; }
        public DbSet<AuthenticationRecovery> AuthenticationRecovery { get; set; }
        public DbSet<AuthenticationChange> AuthenticationChanges { get; set; }*/
    }
}