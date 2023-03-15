using FPTV.Data;
using FPTV.Models.EventsModels;
using FPTV.Models.MatchesModels;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FPTVUnitTests
{
    public class ApplicationDbContextFixture : IDisposable
    {
        public FPTVContext DbContext { get; private set; }
        //
        protected static Guid matchesCSID = Guid.NewGuid();
        protected static Guid eventCSID = Guid.NewGuid();
        protected static Guid winnerTeamID = Guid.NewGuid();

        protected Dictionary<int, int> score = new Dictionary<int, int>();

        public ApplicationDbContextFixture()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<FPTVContext>()
                    .UseSqlite(connection)
                    .Options;

            DbContext = new FPTVContext(options);

            DbContext.Database.EnsureCreated();

            //
            score.Add(1, 16);
            score.Add(2, 11);

            DbContext.MatchesCS.Add(
                new MatchesCS
                {
                    MatchesCSId = matchesCSID,
                    MatchesCSAPIID = 1,
                    EventId = eventCSID,
                    EventAPIID = 10065,
                    EventName = "Test",
                    BeginAt = DateTime.Now.Subtract(TimeSpan.FromHours(2)),
                    EndAt = DateTime.Now,
                    IsFinished = false,
                    TimeType = TimeType.Past,
                    HaveStats = false,
                    MatchesList = null,
                    NumberOfGames = 1,
                    Score = score,
                    TeamsIDList = null,
                    TeamsAPIIDList = null,
                    WinnerTeamId = winnerTeamID,
                    WinnerTeamAPIId = 1,
                    WinnerTeamName = "SAW",
                    Tier = 'C',
                    LiveSupported = false,
                    StreamList = null,
                    LeagueName = "Test",
                    LeagueId = 1,
                    LeagueLink = null
                });

            DbContext.EventCS.Add(
                new EventCS
                {
                    EventCSID = eventCSID,
                    EventAPIID = 10065,
                    EventName = "Test",
                    LeagueName = "Test",
                    EventLink = "Test",
                    TimeType = TimeType.Past,
                    Finished = false,
                    BeginAt = DateTime.Now.Subtract(TimeSpan.FromHours(2)), 
                    EndAt = DateTime.Now,
                    MatchesCSID = matchesCSID,
                    MatchesCSAPIID = 736079,
                    TeamsList = new List<string> { "Fnatic", "SAW" },
                    PrizePool = "1000000$",
                    WinnerTeamID = winnerTeamID,
                    WinnerTeamAPIID = 1,
                    WinnerTeamName = "SAW",
                    Tier = 'C'
                });

             DbContext.SaveChanges();
        }

        public void Dispose() => DbContext.Dispose();

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

        public Dictionary<int, int> GetScore()
        {
            return score;
        }
    }
}


/*
           //Nivel 2
           // Adicionar Ligas para testes
           DbContext.League.Add(new League { LeagueId = 3, Name = "Liga SABSEG", Country = "Portugal" });

           //Nivel 3
           DbContext.Team.AddRange(
               new Team {
                   TeamId = 8,
                   LeagueId = 3,
                   Name = "Estoril Praia",
                   Initials = "EST",
                   NumberOfTitles = 3,
                   MainColor = "Branco"
               },
               new Team {
                   TeamId = 9,
                   LeagueId = 3,
                   Name = "FC Arouca",
                   Initials = "ARC",
                   NumberOfTitles = 0,
                   MainColor = "Amarelo"
               });
           */