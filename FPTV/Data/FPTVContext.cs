using FPTV.Models.BLL.Matches_Stats;
using FPTV.Models.EventModels;
using FPTV.Models.MatchModels;
using FPTV.Models.StatisticsModels;
using FPTV.Models.UserModels;
using FPTV.Models.MatchModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace FPTV.Data
{
    public class FPTVContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<UserBase, Microsoft.AspNetCore.Identity.IdentityRole, string>
    {

        public FPTVContext(DbContextOptions<FPTVContext> options)
            : base(options)
        {
        }

        public DbSet<UserBase> UserBase { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<FavPlayerList> FavPlayerList { get; set; }
        public DbSet<FavTeamsList> FavTeamsList { get; set; }
        public DbSet<EventCS> EventsCS { get; set; }
        public DbSet<EventVal> EventsVal { get; set; }
        public DbSet<MatchesCS> MatchesCS { get; set; }
        public DbSet<MatchesVal> MatchesVal { get; set; }
        public DbSet<Models.MatchModels.Stream> Stream { get; set; }
        public DbSet<MatchPlayerStatsCS> MatchPlayerStatsCS { get; set; }
        public DbSet<MatchPlayerStatsVal> MatchPlayerStatsVal { get; set; }
        public DbSet<MatchTeamsCS> MatchTeamsCS { get; set; }
        public DbSet<MatchTeamsVal> MatchTeamsVal { get; set; }

        public DbSet<MatchCS> MatchCS { get; set; }

        public DbSet<MatchVal> MatchVal { get; set; }

        /*public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<AuthenticationLog> AuthenticationLog { get; set; }
        public DbSet<Token> Token { get; set; }
        public DbSet<Mail> Mail { get; set; }
        public DbSet<AuthenticationRecovery> AuthenticationRecovery { get; set; }
        public DbSet<AuthenticationChange> AuthenticationChanges { get; set; }*/
    }
}