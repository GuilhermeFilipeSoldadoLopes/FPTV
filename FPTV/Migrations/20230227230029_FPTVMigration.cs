using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPTV.Migrations
{
    public partial class FPTVMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MatchesCSId",
                table: "Stream",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MatchesValId",
                table: "Stream",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MatchesCS",
                columns: table => new
                {
                    MatchesCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeginAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    TimeType = table.Column<int>(type: "int", nullable: false),
                    HaveStats = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfGames = table.Column<int>(type: "int", nullable: false),
                    WinnerTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tier = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    LiveSupported = table.Column<bool>(type: "bit", nullable: false),
                    LeagueName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeagueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    EventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeginAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    TimeType = table.Column<int>(type: "int", nullable: false),
                    HaveStats = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfGames = table.Column<int>(type: "int", nullable: false),
                    WinnerTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tier = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    LiveSupported = table.Column<bool>(type: "bit", nullable: false),
                    LeagueName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeagueId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LeagueLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchesVal", x => x.MatchesValId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stream_MatchesCSId",
                table: "Stream",
                column: "MatchesCSId");

            migrationBuilder.CreateIndex(
                name: "IX_Stream_MatchesValId",
                table: "Stream",
                column: "MatchesValId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchVal_MatchesValId",
                table: "MatchVal",
                column: "MatchesValId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchCS_MatchesCSId",
                table: "MatchCS",
                column: "MatchesCSId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchCS_MatchesCS_MatchesCSId",
                table: "MatchCS",
                column: "MatchesCSId",
                principalTable: "MatchesCS",
                principalColumn: "MatchesCSId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MatchVal_MatchesVal_MatchesValId",
                table: "MatchVal",
                column: "MatchesValId",
                principalTable: "MatchesVal",
                principalColumn: "MatchesValId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stream_MatchesCS_MatchesCSId",
                table: "Stream",
                column: "MatchesCSId",
                principalTable: "MatchesCS",
                principalColumn: "MatchesCSId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stream_MatchesVal_MatchesValId",
                table: "Stream",
                column: "MatchesValId",
                principalTable: "MatchesVal",
                principalColumn: "MatchesValId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchCS_MatchesCS_MatchesCSId",
                table: "MatchCS");

            migrationBuilder.DropForeignKey(
                name: "FK_MatchVal_MatchesVal_MatchesValId",
                table: "MatchVal");

            migrationBuilder.DropForeignKey(
                name: "FK_Stream_MatchesCS_MatchesCSId",
                table: "Stream");

            migrationBuilder.DropForeignKey(
                name: "FK_Stream_MatchesVal_MatchesValId",
                table: "Stream");

            migrationBuilder.DropTable(
                name: "MatchesCS");

            migrationBuilder.DropTable(
                name: "MatchesVal");

            migrationBuilder.DropIndex(
                name: "IX_Stream_MatchesCSId",
                table: "Stream");

            migrationBuilder.DropIndex(
                name: "IX_Stream_MatchesValId",
                table: "Stream");

            migrationBuilder.DropIndex(
                name: "IX_MatchVal_MatchesValId",
                table: "MatchVal");

            migrationBuilder.DropIndex(
                name: "IX_MatchCS_MatchesCSId",
                table: "MatchCS");

            migrationBuilder.DropColumn(
                name: "MatchesCSId",
                table: "Stream");

            migrationBuilder.DropColumn(
                name: "MatchesValId",
                table: "Stream");
        }
    }
}
