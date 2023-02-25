using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPTV.Migrations
{
    public partial class @event : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventsCS",
                columns: table => new
                {
                    EventCSID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeType = table.Column<int>(type: "int", nullable: false),
                    Finished = table.Column<bool>(type: "bit", nullable: false),
                    BeginAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TimeType = table.Column<int>(type: "int", nullable: false),
                    Finished = table.Column<bool>(type: "bit", nullable: false),
                    BeginAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PrizePool = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WinnerTeamID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tier = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventsVal", x => x.EventValID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventsCS");

            migrationBuilder.DropTable(
                name: "EventsVal");
        }
    }
}
