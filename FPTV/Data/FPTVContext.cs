using FPTV.Models.AuthenticationModels.DAL;
using FPTV.Models.UserModels.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Reflection.Emit;

namespace FPTV.Data
{
    public class FPTVContext : IdentityDbContext
    {
        internal readonly object ProfilePicture;

        public FPTVContext(DbContextOptions<FPTVContext> options)
            : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            IdentityRole adminRole = new IdentityRole
            {
                Name = "admin",
                NormalizedName = "admin".ToUpper()
            };
            IdentityRole moderatorRole = new IdentityRole
            {
                Name = "moderator",
                NormalizedName = "moderator".ToUpper()
            };
            IdentityRole userRole = new IdentityRole
            {
                Name = "user",
                NormalizedName = "user".ToUpper()
            };
            IdentityRole guestRole = new IdentityRole
            {
                Name = "guest",
                NormalizedName = "guest".ToUpper()
            };

            builder.Entity<IdentityRole>().HasData(
                adminRole,
                moderatorRole,
                userRole,
            guestRole
            );


            builder.Entity<Comment>()
                   .HasOne(m => m.Topic)
                   .WithMany(m => m.Comments)
                   .HasForeignKey(m => m.TopicId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Reaction>()
                   .HasOne(m => m.Comment)
                   .WithMany(m => m.Reactions)
                   .HasForeignKey(m => m.CommentId)
                   .OnDelete(DeleteBehavior.NoAction);

        }


        public DbSet<Profile> Profile { get; set; }
        public DbSet<FavTeamsList> FavTeamsList { get; set; }
        public DbSet<FavPlayerList> FavPlayerList { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<AuthenticationLog> AuthenticationLog { get; set; }
        public DbSet<Token> Token { get; set; }
        public DbSet<Mail> Mail { get; set; }
        public DbSet<AuthenticationRecovery> AuthenticationRecovery { get; set; }
        public DbSet<AuthenticationChange> AuthenticationChanges { get; set; }
    }
}