using FPTV.Models.BLL.Matches_Stats;
using FPTV.Models.StatisticsModels;
using FPTV.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using FPTV.Models.MatchesModels;
using FPTV.Models.EventsModels;

namespace FPTV.Data
{
    public class FPTVContext : Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityDbContext<UserBase, Microsoft.AspNetCore.Identity.IdentityRole, string>
    {

        public FPTVContext(DbContextOptions<FPTVContext> options)
            : base(options)
        {
        }

        //Events
		public DbSet<EventCS> EventCS { get; set; }
		public DbSet<EventVal> EventVal { get; set; }

		//Matches
		public DbSet<MatchesCS> MatchesCS { get; set; }
		public DbSet<MatchesVal> MatchesVal { get; set; }
		public DbSet<Models.MatchesModels.Stream> Stream { get; set; }

		//Statistics
		public DbSet<MatchCS> MatchCS { get; set; }
		public DbSet<MatchVal> MatchVal { get; set; }
		public DbSet<MatchPlayerStatsCS> MatchPlayerStatsCS { get; set; }
		public DbSet<MatchPlayerStatsVal> MatchPlayerStatsVal { get; set; }
		public DbSet<MatchTeamsCS> MatchTeamsCS { get; set; }
		public DbSet<MatchTeamsVal> MatchTeamsVal { get; set; }

        //User
		public DbSet<UserBase> UserBase { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<FavPlayerList> FavPlayerList { get; set; }
        public DbSet<FavTeamsList> FavTeamsList { get; set; }
		public DbSet<Player> Player { get; set; }
		public DbSet<Team> Team { get; set; }


        //ToReview
		/*public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<AuthenticationLog> AuthenticationLog { get; set; }
        public DbSet<Token> Token { get; set; }
        public DbSet<Mail> Mail { get; set; }
        public DbSet<AuthenticationRecovery> AuthenticationRecovery { get; set; }
        public DbSet<AuthenticationChange> AuthenticationChanges { get; set; }*/
	}
}