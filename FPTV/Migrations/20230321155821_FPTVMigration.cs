using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPTV.Migrations
{
    public partial class FPTVMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MatchesCS_Team_WinnerTeamTeamId",
                table: "MatchesCS");

            migrationBuilder.DropIndex(
                name: "IX_MatchesCS_WinnerTeamTeamId",
                table: "MatchesCS");

            migrationBuilder.DropColumn(
                name: "WinnerTeamTeamId",
                table: "MatchesCS");

            migrationBuilder.AddColumn<Guid>(
                name: "EventCSID",
                table: "Team",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EventValID",
                table: "Team",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MatchesCSId",
                table: "Team",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "MatchesValId",
                table: "Team",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Score",
                columns: table => new
                {
                    ScoreID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamScore = table.Column<int>(type: "int", nullable: false),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MatchesCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MatchesValId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Score", x => x.ScoreID);
                    table.ForeignKey(
                        name: "FK_Score_MatchesCS_MatchesCSId",
                        column: x => x.MatchesCSId,
                        principalTable: "MatchesCS",
                        principalColumn: "MatchesCSId");
                    table.ForeignKey(
                        name: "FK_Score_MatchesVal_MatchesValId",
                        column: x => x.MatchesValId,
                        principalTable: "MatchesVal",
                        principalColumn: "MatchesValId");
                    table.ForeignKey(
                        name: "FK_Score_Team_TeamId",
                        column: x => x.TeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Team_EventCSID",
                table: "Team",
                column: "EventCSID");

            migrationBuilder.CreateIndex(
                name: "IX_Team_EventValID",
                table: "Team",
                column: "EventValID");

            migrationBuilder.CreateIndex(
                name: "IX_Team_MatchesCSId",
                table: "Team",
                column: "MatchesCSId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_MatchesValId",
                table: "Team",
                column: "MatchesValId");

            migrationBuilder.CreateIndex(
                name: "IX_Score_MatchesCSId",
                table: "Score",
                column: "MatchesCSId");

            migrationBuilder.CreateIndex(
                name: "IX_Score_MatchesValId",
                table: "Score",
                column: "MatchesValId");

            migrationBuilder.CreateIndex(
                name: "IX_Score_TeamId",
                table: "Score",
                column: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_EventCS_EventCSID",
                table: "Team",
                column: "EventCSID",
                principalTable: "EventCS",
                principalColumn: "EventCSID");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_EventVal_EventValID",
                table: "Team",
                column: "EventValID",
                principalTable: "EventVal",
                principalColumn: "EventValID");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_MatchesCS_MatchesCSId",
                table: "Team",
                column: "MatchesCSId",
                principalTable: "MatchesCS",
                principalColumn: "MatchesCSId");

            migrationBuilder.AddForeignKey(
                name: "FK_Team_MatchesVal_MatchesValId",
                table: "Team",
                column: "MatchesValId",
                principalTable: "MatchesVal",
                principalColumn: "MatchesValId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Team_EventCS_EventCSID",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_EventVal_EventValID",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_MatchesCS_MatchesCSId",
                table: "Team");

            migrationBuilder.DropForeignKey(
                name: "FK_Team_MatchesVal_MatchesValId",
                table: "Team");

            migrationBuilder.DropTable(
                name: "Score");

            migrationBuilder.DropIndex(
                name: "IX_Team_EventCSID",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_EventValID",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_MatchesCSId",
                table: "Team");

            migrationBuilder.DropIndex(
                name: "IX_Team_MatchesValId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "EventCSID",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "EventValID",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "MatchesCSId",
                table: "Team");

            migrationBuilder.DropColumn(
                name: "MatchesValId",
                table: "Team");

            migrationBuilder.AddColumn<Guid>(
                name: "WinnerTeamTeamId",
                table: "MatchesCS",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_MatchesCS_WinnerTeamTeamId",
                table: "MatchesCS",
                column: "WinnerTeamTeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_MatchesCS_Team_WinnerTeamTeamId",
                table: "MatchesCS",
                column: "WinnerTeamTeamId",
                principalTable: "Team",
                principalColumn: "TeamId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
