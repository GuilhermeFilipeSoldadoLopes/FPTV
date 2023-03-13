using FPTV.Data;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FPTVUnitTests
{
    public class ApplicationDbContextFixture : IDisposable
    {
        public FPTVContext DbContext { get; private set; }

        public ApplicationDbContextFixture()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();
            var options = new DbContextOptionsBuilder<FPTVContext>()
                    .UseSqlite(connection)
                    .Options;
            DbContext = new FPTVContext(options);

            DbContext.Database.EnsureCreated();

           
            DbContext.SaveChanges();
        }

        public void Dispose() => DbContext.Dispose();
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