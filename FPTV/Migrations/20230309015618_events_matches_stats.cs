using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPTV.Migrations
{
    public partial class events_matches_stats : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventCS",
                columns: table => new
                {
                    EventCSID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventAPIID = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeagueName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeType = table.Column<int>(type: "int", nullable: false),
                    Finished = table.Column<bool>(type: "bit", nullable: false),
                    BeginAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MatchesCSID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchesCSAPIID = table.Column<int>(type: "int", nullable: false),
                    PrizePool = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WinnerTeamID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WinnerTeamAPIID = table.Column<int>(type: "int", nullable: false),
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tier = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCS", x => x.EventCSID);
                });

            migrationBuilder.CreateTable(
                name: "EventVal",
                columns: table => new
                {
                    EventValID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventAPIID = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeagueName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeType = table.Column<int>(type: "int", nullable: false),
                    Finished = table.Column<bool>(type: "bit", nullable: false),
                    BeginAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MatchesValID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchesValAPIID = table.Column<int>(type: "int", nullable: false),
                    PrizePool = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WinnerTeamID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WinnerTeamAPIID = table.Column<int>(type: "int", nullable: false),
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tier = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventVal", x => x.EventValID);
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
                    WinnerTeamAPIId = table.Column<int>(type: "int", nullable: false),
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tier = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    LiveSupported = table.Column<bool>(type: "bit", nullable: false),
                    LeagueName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeagueId = table.Column<int>(type: "int", nullable: true),
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
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tier = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    LiveSupported = table.Column<bool>(type: "bit", nullable: false),
                    LeagueName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeagueId = table.Column<int>(type: "int", nullable: true),
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
                    MatchCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchCSAPIID = table.Column<int>(type: "int", nullable: false),
                    MatchesCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchesCSAPIId = table.Column<int>(type: "int", nullable: false),
                    RoundsScore = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Map = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WinnerTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WinnerTeamAPIId = table.Column<int>(type: "int", nullable: false),
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    Map = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WinnerTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WinnerTeamAPIId = table.Column<int>(type: "int", nullable: false),
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    StreamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StreamLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StreamLanguage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchesCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MatchesValId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stream", x => x.StreamId);
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
                name: "FavPlayerList",
                columns: table => new
                {
                    FavPlayerListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavPlayerList", x => x.FavPlayerListId);
                    table.ForeignKey(
                        name: "FK_FavPlayerList_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FavTeamsList",
                columns: table => new
                {
                    FavTeamsListId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavTeamsList", x => x.FavTeamsListId);
                    table.ForeignKey(
                        name: "FK_FavTeamsList_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MatchPlayerStatsCS",
                columns: table => new
                {
                    MatchPlayerStatsCSID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchCSAPIID = table.Column<int>(type: "int", nullable: false),
                    PlayerCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlayerCSAPIId = table.Column<int>(type: "int", nullable: false),
                    Kills = table.Column<int>(type: "int", nullable: false),
                    Deaths = table.Column<int>(type: "int", nullable: false),
                    Assists = table.Column<int>(type: "int", nullable: false),
                    FlashAssist = table.Column<int>(type: "int", nullable: false),
                    ADR = table.Column<float>(type: "real", nullable: false),
                    HeadShots = table.Column<float>(type: "real", nullable: false),
                    KD_Diff = table.Column<float>(type: "real", nullable: false),
                    PlayerName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchPlayerStatsCS", x => x.MatchPlayerStatsCSID);
                    table.ForeignKey(
                        name: "FK_MatchPlayerStatsCS_MatchCS_MatchCSId",
                        column: x => x.MatchCSId,
                        principalTable: "MatchCS",
                        principalColumn: "MatchCSId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchTeamsCS",
                columns: table => new
                {
                    MatchCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchCSAPIID = table.Column<int>(type: "int", nullable: false),
                    TeamCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeamCSAPIId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchCSId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchTeamsCS", x => x.MatchCSId);
                    table.ForeignKey(
                        name: "FK_MatchTeamsCS_MatchCS_MatchCSId1",
                        column: x => x.MatchCSId1,
                        principalTable: "MatchCS",
                        principalColumn: "MatchCSId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchPlayerStatsVal",
                columns: table => new
                {
                    MatchPlayerStatsValID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchValId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchValAPIID = table.Column<int>(type: "int", nullable: false),
                    PlayerValId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PlayerValAPIId = table.Column<int>(type: "int", nullable: false),
                    Kills = table.Column<int>(type: "int", nullable: false),
                    Deaths = table.Column<int>(type: "int", nullable: false),
                    Assists = table.Column<int>(type: "int", nullable: false),
                    ADR = table.Column<float>(type: "real", nullable: false),
                    Kast = table.Column<float>(type: "real", nullable: false),
                    HeadShots = table.Column<float>(type: "real", nullable: false),
                    KD_Diff = table.Column<float>(type: "real", nullable: false),
                    PlayerName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchPlayerStatsVal", x => x.MatchPlayerStatsValID);
                    table.ForeignKey(
                        name: "FK_MatchPlayerStatsVal_MatchVal_MatchValId",
                        column: x => x.MatchValId,
                        principalTable: "MatchVal",
                        principalColumn: "MatchValId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchTeamsVal",
                columns: table => new
                {
                    MatchValId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchValAPIID = table.Column<int>(type: "int", nullable: false),
                    TeamValId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeamValAPIId = table.Column<int>(type: "int", nullable: false),
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

            migrationBuilder.CreateTable(
                name: "Team",
                columns: table => new
                {
                    TeamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CouchName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WorldRank = table.Column<int>(type: "int", nullable: false),
                    Winnings = table.Column<int>(type: "int", nullable: false),
                    Losses = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FavTeamsListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Team_FavTeamsList_FavTeamsListId",
                        column: x => x.FavTeamsListId,
                        principalTable: "FavTeamsList",
                        principalColumn: "FavTeamsListId");
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Nacionality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FavPlayerListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeamId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Player", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_Player_FavPlayerList_FavPlayerListId",
                        column: x => x.FavPlayerListId,
                        principalTable: "FavPlayerList",
                        principalColumn: "FavPlayerListId");
                    table.ForeignKey(
                        name: "FK_Player_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FavPlayerList_ProfileId",
                table: "FavPlayerList",
                column: "ProfileId",
                unique: true,
                filter: "[ProfileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FavTeamsList_ProfileId",
                table: "FavTeamsList",
                column: "ProfileId",
                unique: true,
                filter: "[ProfileId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MatchCS_MatchesCSId",
                table: "MatchCS",
                column: "MatchesCSId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayerStatsCS_MatchCSId",
                table: "MatchPlayerStatsCS",
                column: "MatchCSId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayerStatsVal_MatchValId",
                table: "MatchPlayerStatsVal",
                column: "MatchValId");

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
                name: "IX_Player_FavPlayerListId",
                table: "Player",
                column: "FavPlayerListId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_TeamId",
                table: "Player",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Stream_MatchesCSId",
                table: "Stream",
                column: "MatchesCSId");

            migrationBuilder.CreateIndex(
                name: "IX_Stream_MatchesValId",
                table: "Stream",
                column: "MatchesValId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_FavTeamsListId",
                table: "Team",
                column: "FavTeamsListId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventCS");

            migrationBuilder.DropTable(
                name: "EventVal");

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
                name: "Stream");

            migrationBuilder.DropTable(
                name: "MatchCS");

            migrationBuilder.DropTable(
                name: "MatchVal");

            migrationBuilder.DropTable(
                name: "FavPlayerList");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "MatchesCS");

            migrationBuilder.DropTable(
                name: "MatchesVal");

            migrationBuilder.DropTable(
                name: "FavTeamsList");
        }
    }
}
