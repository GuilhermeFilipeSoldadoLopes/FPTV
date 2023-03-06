using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPTV.Migrations
{
    public partial class stats_matches_events : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
	        migrationBuilder.CreateTable(
                name: "EventsCS",
                columns: table => new
                {
                    EventCSID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventAPIID = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeType = table.Column<int>(type: "int", nullable: false),
                    Finished = table.Column<bool>(type: "bit", nullable: false),
                    BeginAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MatchesCSID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrizePool = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WinnerTeamID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tier = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventsCS", x => x.EventCSID);
                });

            migrationBuilder.CreateTable(
                name: "EventsVal",
                columns: table => new
                {
                    EventValID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventAPIID = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeType = table.Column<int>(type: "int", nullable: false),
                    Finished = table.Column<bool>(type: "bit", nullable: false),
                    BeginAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MatchesValID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrizePool = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WinnerTeamID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tier = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventsVal", x => x.EventValID);
                });

            migrationBuilder.CreateTable(
                name: "MatchesCS",
                columns: table => new
                {
                    MatchesCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchesCSAPIID = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventAPIID = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeginAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    TimeType = table.Column<int>(type: "int", nullable: false),
                    HaveStats = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfGames = table.Column<int>(type: "int", nullable: false),
                    WinnerTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WinnerTeamAPIId = table.Column<int>(type: "int", nullable: true),
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tier = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    LiveSupported = table.Column<bool>(type: "bit", nullable: false),
                    LeagueName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LeagueLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchesCS", x => x.MatchesCSId);
                });

            migrationBuilder.CreateTable(
                name: "MatchesVal",
                columns: table => new
                {
                    MatchesValId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchesValAPIID = table.Column<int>(type: "int", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventAPIID = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeginAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    TimeType = table.Column<int>(type: "int", nullable: false),
                    HaveStats = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfGames = table.Column<int>(type: "int", nullable: false),
                    WinnerTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WinnerTeamAPIId = table.Column<int>(type: "int", nullable: false),
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tier = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    LiveSupported = table.Column<bool>(type: "bit", nullable: false),
                    LeagueName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    LeagueLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchesVal", x => x.MatchesValId);
                });

            migrationBuilder.CreateTable(
                name: "MatchCS",
                columns: table => new
                {
                    MatchCSId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MatchCSAPIID = table.Column<int>(type: "int", nullable: false),
                    MatchesCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchesCSAPIId = table.Column<int>(type: "int", nullable: false),
                    RoundsScore = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Map = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WinnerTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WinnerTeamAPIId = table.Column<int>(type: "int", nullable: true),
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchCS", x => x.MatchCSId);
                    table.ForeignKey(
                        name: "FK_MatchCS_MatchesCS_MatchesCSId",
                        column: x => x.MatchesCSId,
                        principalTable: "MatchesCS",
                        principalColumn: "MatchesCSId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchVal",
                columns: table => new
                {
                    MatchValId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchValAPIID = table.Column<int>(type: "int", nullable: false),
                    MatchesValId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchesValAPIId = table.Column<int>(type: "int", nullable: false),
                    RoundsScore = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Map = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WinnerTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WinnerTeamAPIId = table.Column<int>(type: "int", nullable: true),
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchVal", x => x.MatchValId);
                    table.ForeignKey(
                        name: "FK_MatchVal_MatchesVal_MatchesValId",
                        column: x => x.MatchesValId,
                        principalTable: "MatchesVal",
                        principalColumn: "MatchesValId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stream",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StreamLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreamLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchesCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MatchesValId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stream", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stream_MatchesCS_MatchesCSId",
                        column: x => x.MatchesCSId,
                        principalTable: "MatchesCS",
                        principalColumn: "MatchesCSId");
                    table.ForeignKey(
                        name: "FK_Stream_MatchesVal_MatchesValId",
                        column: x => x.MatchesValId,
                        principalTable: "MatchesVal",
                        principalColumn: "MatchesValId");
                });

            migrationBuilder.CreateTable(
                name: "MatchPlayerStatsCS",
                columns: table => new
                {
                    MatchCSId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlayerCSId = table.Column<int>(type: "int", nullable: false),
                    Kills = table.Column<int>(type: "int", nullable: false),
                    Deaths = table.Column<int>(type: "int", nullable: false),
                    Assists = table.Column<int>(type: "int", nullable: false),
                    FlashAssist = table.Column<int>(type: "int", nullable: false),
                    ADR = table.Column<float>(type: "real", nullable: false),
                    HeadShots = table.Column<float>(type: "real", nullable: false),
                    KD_Diff = table.Column<float>(type: "real", nullable: false),
                    PlayerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchCSId1 = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchPlayerStatsCS", x => x.MatchCSId);
                    table.ForeignKey(
                        name: "FK_MatchPlayerStatsCS_MatchCS_MatchCSId1",
                        column: x => x.MatchCSId1,
                        principalTable: "MatchCS",
                        principalColumn: "MatchCSId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchTeamsCS",
                columns: table => new
                {
                    MatchCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchCSId1 = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchTeamsCS", x => x.MatchCSId);
                    table.ForeignKey(
                        name: "FK_MatchTeamsCS_MatchCS_MatchCSId1",
                        column: x => x.MatchCSId1,
                        principalTable: "MatchCS",
                        principalColumn: "MatchCSId");
                });

            migrationBuilder.CreateTable(
                name: "MatchPlayerStatsVal",
                columns: table => new
                {
                    MatchValId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerValId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Kills = table.Column<int>(type: "int", nullable: false),
                    Deaths = table.Column<int>(type: "int", nullable: false),
                    Assists = table.Column<int>(type: "int", nullable: false),
                    ADR = table.Column<float>(type: "real", nullable: false),
                    Kast = table.Column<float>(type: "real", nullable: false),
                    HeadShots = table.Column<float>(type: "real", nullable: false),
                    KD_Diff = table.Column<float>(type: "real", nullable: false),
                    PlayerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchValId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchPlayerStatsVal", x => x.MatchValId);
                    table.ForeignKey(
                        name: "FK_MatchPlayerStatsVal_MatchVal_MatchValId1",
                        column: x => x.MatchValId1,
                        principalTable: "MatchVal",
                        principalColumn: "MatchValId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchTeamsVal",
                columns: table => new
                {
                    MatchValId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamValId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchValId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchTeamsVal", x => x.MatchValId);
                    table.ForeignKey(
                        name: "FK_MatchTeamsVal_MatchVal_MatchValId1",
                        column: x => x.MatchValId1,
                        principalTable: "MatchVal",
                        principalColumn: "MatchValId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MatchCS_MatchesCSId",
                table: "MatchCS",
                column: "MatchesCSId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayerStatsCS_MatchCSId1",
                table: "MatchPlayerStatsCS",
                column: "MatchCSId1");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayerStatsVal_MatchValId1",
                table: "MatchPlayerStatsVal",
                column: "MatchValId1");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTeamsCS_MatchCSId1",
                table: "MatchTeamsCS",
                column: "MatchCSId1");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTeamsVal_MatchValId1",
                table: "MatchTeamsVal",
                column: "MatchValId1");

            migrationBuilder.CreateIndex(
                name: "IX_MatchVal_MatchesValId",
                table: "MatchVal",
                column: "MatchesValId");

            migrationBuilder.CreateIndex(
                name: "IX_Stream_MatchesCSId",
                table: "Stream",
                column: "MatchesCSId");

            migrationBuilder.CreateIndex(
                name: "IX_Stream_MatchesValId",
                table: "Stream",
                column: "MatchesValId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ErrorLog");

            migrationBuilder.DropTable(
                name: "EventsCS");

            migrationBuilder.DropTable(
                name: "EventsVal");

            migrationBuilder.DropTable(
                name: "MatchPlayerStatsCS");

            migrationBuilder.DropTable(
                name: "MatchPlayerStatsVal");

            migrationBuilder.DropTable(
                name: "MatchTeamsCS");

            migrationBuilder.DropTable(
                name: "MatchTeamsVal");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "Stream");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "MatchCS");

            migrationBuilder.DropTable(
                name: "MatchVal");

            migrationBuilder.DropTable(
                name: "FavPlayerList");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "MatchesCS");

            migrationBuilder.DropTable(
                name: "MatchesVal");

            migrationBuilder.DropTable(
                name: "FavTeamsList");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
