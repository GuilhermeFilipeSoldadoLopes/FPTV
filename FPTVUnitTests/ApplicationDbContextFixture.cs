using FPTV.Data;
using FPTV.Models.EventsModels;
using FPTV.Models.Forum;
using FPTV.Models.MatchesModels;
using FPTV.Models.UserModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FPTVUnitTests
{
    public class ApplicationDbContextFixture : IDisposable
    {
        private readonly IWebHostEnvironment env;

        public FPTVContext DbContext { get; private set; }
        //
        protected static Guid adminID = Guid.NewGuid();
        //
        protected static Guid matchesCSID = Guid.NewGuid();
		//
        protected static Guid eventCSID = Guid.NewGuid();
		//
		protected static Guid team1CSID = Guid.NewGuid();
		protected static Guid team2CSID = Guid.NewGuid();
		protected static List<Team> teamsList1 = new List<Team>();
		protected static List<Team> teamsList2 = new List<Team>();
        protected static List<Team> teamsList3 = new List<Team>();
        protected static Team team1;
        protected static Team team2;
        //
        protected static Guid player1CSID = Guid.NewGuid();
		protected static Guid player2CSID = Guid.NewGuid();
		protected static Guid player3CSID = Guid.NewGuid();
		protected static Guid player4CSID = Guid.NewGuid();
		protected static Guid player5CSID = Guid.NewGuid();
		protected static Guid player6CSID = Guid.NewGuid();
		protected static Guid player7CSID = Guid.NewGuid();
		protected static Guid player8CSID = Guid.NewGuid();
		protected static Guid player9CSID = Guid.NewGuid();
		protected static Guid player10CSID = Guid.NewGuid();
		protected static List<Player> playersList1 = new List<Player>();
		protected static List<Player> playersList2 = new List<Player>();
		//
		protected static Guid winnerTeamID = Guid.NewGuid();
		//
		protected static Guid Score1ID = Guid.NewGuid();
		protected static Guid Score2ID = Guid.NewGuid();
		protected static List<Score> scoreList = new List<Score>();
        //
        protected static int topicID = 1;

        public ApplicationDbContextFixture()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<FPTVContext>()
                    .UseSqlite(connection)
                    .Options;

            DbContext = new FPTVContext(options);

            DbContext.Database.EnsureCreated();

            DbContext.Profiles.Add(CreateAdmin());
			DbContext.SaveChanges();

			DbContext.Team.AddRange(
				new Team
				{
					TeamId = team1CSID,
					TeamAPIID = 1,
					Name = "Test1",
					Players = null,
					CoachName = "Test",
					WorldRank = 1,
					Winnings = 100,
					Losses = 50,
					Image = "/images/iconMenu.jpg",
					Game = GameType.CSGO
				},
				new Team
				{
					TeamId = team2CSID,
					TeamAPIID = 2,
					Name = "Test2",
					Players = null,
					CoachName = "Test",
					WorldRank = 2,
					Winnings = 99,
					Losses = 50,
					Image = "/images/iconMenu.jpg",
					Game = GameType.CSGO
				});

            DbContext.SaveChanges();

            team1 = DbContext.Team.FirstOrDefault(t => t.TeamId == team1CSID);
			teamsList1.Add(team1);
			team2 = DbContext.Team.FirstOrDefault(t => t.TeamId == team2CSID);
			teamsList2.Add(team2);
			teamsList1.Add(team2);
			teamsList2.Add(team1);

            DbContext.SaveChanges();

            DbContext.Player.AddRange(
				new Player
				{
					PlayerId = player1CSID,
					PlayerAPIId = 1,
					Name = "1",
					Age = 20,
					Rating = 1.20F,
					Teams = teamsList1,
					Nationality = "Portuguese",
					Flag = "/images/Flags/1x1/pt.svg",
					Image = "/images/iconMenu.jpg",
					Game = GameType.CSGO
				},
				new Player
				{
					PlayerId = player2CSID,
					PlayerAPIId = 2,
					Name = "2",
					Age = 20,
					Rating = 1.25F,
					Teams = teamsList1,
					Nationality = "Portuguese",
					Flag = "/images/Flags/1x1/pt.svg",
					Image = "/images/iconMenu.jpg",
					Game = GameType.CSGO
				},
				new Player
				{
					PlayerId = player3CSID,
					PlayerAPIId = 3,
					Name = "3",
					Age = 20,
					Rating = 1.30F,
					Teams = teamsList1,
					Nationality = "Portuguese",
					Flag = "/images/Flags/1x1/pt.svg",
					Image = "/images/iconMenu.jpg",
					Game = GameType.CSGO
				},
				new Player
				{
					PlayerId = player4CSID,
					PlayerAPIId = 4,
					Name = "4",
					Age = 20,
					Rating = 1.35F,
					Teams = teamsList1,
					Nationality = "Portuguese",
					Flag = "/images/Flags/1x1/pt.svg",
					Image = "/images/iconMenu.jpg",
					Game = GameType.CSGO
				},
				new Player
				{
					PlayerId = player5CSID,
					PlayerAPIId = 5,
					Name = "5",
					Age = 20,
					Rating = 1.40F,
					Teams = teamsList1,
					Nationality = "Portuguese",
					Flag = "/images/Flags/1x1/pt.svg",
					Image = "/images/iconMenu.jpg",
					Game = GameType.CSGO
				},
				new Player
				{
					PlayerId = player6CSID,
					PlayerAPIId = 6,
					Name = "6",
					Age = 20,
					Rating = 1.45F,
					Teams = teamsList2,
					Nationality = "Portuguese",
					Flag = "/images/Flags/1x1/pt.svg",
					Image = "/images/iconMenu.jpg",
					Game = GameType.CSGO
				},
				new Player
				{
					PlayerId = player7CSID,
					PlayerAPIId = 7,
					Name = "7",
					Age = 20,
					Rating = 1.50F,
					Teams = teamsList2,
					Nationality = "Portuguese",
					Flag = "/images/Flags/1x1/pt.svg",
					Image = "/images/iconMenu.jpg",
					Game = GameType.CSGO
				},
				new Player
				{
					PlayerId = player8CSID,
					PlayerAPIId = 8,
					Name = "8",
					Age = 20,
					Rating = 1.55F,
					Teams = teamsList2,
					Nationality = "Portuguese",
					Flag = "/images/Flags/1x1/pt.svg",
					Image = "/images/iconMenu.jpg",
					Game = GameType.CSGO
				},
				new Player
				{
					PlayerId = player9CSID,
					PlayerAPIId = 9,
					Name = "9",
					Age = 20,
					Rating = 1.60F,
					Teams = teamsList2,
					Nationality = "Portuguese",
					Flag = "/images/Flags/1x1/pt.svg",
					Image = "/images/iconMenu.jpg",
					Game = GameType.CSGO
				},
				new Player
				{
					PlayerId = player10CSID,
					PlayerAPIId = 10,
					Name = "10",
					Age = 20,
					Rating = 1.65F,
					Teams = teamsList2,
					Nationality = "Portuguese",
					Flag = "/images/Flags/1x1/pt.svg",
					Image = "/images/iconMenu.jpg",
					Game = GameType.CSGO
				});

            DbContext.SaveChanges();

            playersList1.Add(DbContext.Player.FirstOrDefault(p => p.PlayerId == player1CSID));
			playersList1.Add(DbContext.Player.FirstOrDefault(p => p.PlayerId == player2CSID));
			playersList1.Add(DbContext.Player.FirstOrDefault(p => p.PlayerId == player3CSID));
			playersList1.Add(DbContext.Player.FirstOrDefault(p => p.PlayerId == player4CSID));
			playersList1.Add(DbContext.Player.FirstOrDefault(p => p.PlayerId == player5CSID));
			playersList2.Add(DbContext.Player.FirstOrDefault(p => p.PlayerId == player6CSID));
			playersList2.Add(DbContext.Player.FirstOrDefault(p => p.PlayerId == player7CSID));
			playersList2.Add(DbContext.Player.FirstOrDefault(p => p.PlayerId == player8CSID));
			playersList2.Add(DbContext.Player.FirstOrDefault(p => p.PlayerId == player9CSID));
			playersList2.Add(DbContext.Player.FirstOrDefault(p => p.PlayerId == player10CSID));

            DbContext.SaveChanges();

			team1.Players = playersList1;
			team2.Players = playersList2;

			DbContext.SaveChanges();

			scoreList.Add(new Score
			{
				ScoreID = Score1ID,
				Team = team1,
				TeamName = "Test1",
				TeamScore = 16
			});

			scoreList.Add(new Score
			{
				ScoreID = Score2ID,
				Team = team2,
				TeamName = "Test2",
				TeamScore = 10
			});

            DbContext.SaveChanges();

            teamsList3 = new List<Team>() { team1, team2};

            DbContext.EventCS.Add(
				new EventCS
				{
					EventCSID = eventCSID,
					EventAPIID = 10065,
					EventName = "Test",
					LeagueName = "Test",
					EventImage = "/images/iconMenu.jpg",
					EventLink = "Test",
					TimeType = TimeType.Past,
					Finished = false,
					BeginAt = DateTime.Now.Subtract(TimeSpan.FromHours(2)),
					EndAt = DateTime.Now,
					MatchesCSAPIID = 736079,
					TeamsList = teamsList3,
					PrizePool = "1000000$",
					WinnerTeamAPIID = 1,
					WinnerTeamName = "Test1",
					Tier = 'C'
				});

            DbContext.SaveChanges();

            DbContext.MatchesCS.Add(
			new MatchesCS
			{
				MatchesCSId = matchesCSID,
				MatchesAPIID = 736079,
				Event = DbContext.EventCS.FirstOrDefault(e => e.EventCSID == eventCSID),
				EventAPIID = 10065,
				EventName = "Test",
				BeginAt = DateTime.Now.Subtract(TimeSpan.FromHours(2)),
				EndAt = DateTime.Now,
				IsFinished = false,
				TimeType = TimeType.Past,
				HaveStats = false,
				MatchesList = null,
				NumberOfGames = 1,
				Scores = scoreList,
				TeamsList = teamsList3,
				TeamsAPIIDList = null,
				WinnerTeamAPIId = 1,
				WinnerTeamName = "Test1",
				Tier = 'C',
				LiveSupported = false,
				StreamList = null,
				LeagueName = "Test",
				LeagueId = 1,
				LeagueLink = null
			});

			DbContext.SaveChanges();
        }

        public void Dispose() => DbContext.Dispose();

		public Profile CreateAdmin()
		{
            var admin = CreateUser();

            Profile profile = new();

            profile.Id = adminID;
			profile.User = admin;
			profile.UserId = new Guid(admin.Id);
			profile.RegistrationDate = DateTime.Now;
			profile.Country = "pt";

			//var adminImage = Path.Combine(env.WebRootPath, "images", "Mods_Image.png");
			//         profile.Picture = System.IO.File.ReadAllBytes(adminImage);

			admin.Profile = profile;
            admin.EmailConfirmed = true;

			return profile;
        }

        private static UserBase CreateUser()
        {
            try {
                return Activator.CreateInstance<UserBase>();
            } catch {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(UserBase)}'.");
            }
        }

        public void CreateNewTopic()
        {
            DbContext.Topics.Add(
            new Topic
            {
                //TopicId = topicID,
				GameType = GameType.CSGO,
				Title = "Test",
                Content = "Test123",
                Date = DateTime.Now,
                ProfileId = adminID,
                Profile = DbContext.Profiles.FirstOrDefault(p => p.Id == adminID),
				Comments = new List<Comment>()
            });

            DbContext.SaveChanges();
        }

        public Guid GetAdminId()
        {
            return adminID;
        }

        public int GetTopicId()
        {
            return topicID;
        }

        public Guid GetMatchesCSId()
		{
			return matchesCSID;
		}

		public Guid GetEventsCSId()
		{
			return eventCSID;
		}

		public Guid GetWinnerTeamId()
		{
			return winnerTeamID;
		}

		public List<Score> GetScore()
		{
			return scoreList;
		}

		public List<Team> GetTeamsList()
		{
			return teamsList1;
		}
    }
}