using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPTV.Migrations
{
    public partial class TestMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventAPIID",
                table: "EventsVal",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "MatchesValID",
                table: "EventsVal",
                type: "uniqueidentifier",
                nullable: false);

            migrationBuilder.AddColumn<int>(
                name: "EventAPIID",
                table: "EventsCS",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "MatchesCSID",
                table: "EventsCS",
                type: "uniqueidentifier",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventAPIID",
                table: "EventsVal");

            migrationBuilder.DropColumn(
                name: "MatchesValID",
                table: "EventsVal");

            migrationBuilder.DropColumn(
                name: "EventAPIID",
                table: "EventsCS");

            migrationBuilder.DropColumn(
                name: "MatchesCSID",
                table: "EventsCS");
        }
    }
}
