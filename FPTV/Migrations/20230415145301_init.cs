using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FPTV.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Biography = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Picture = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLog",
                columns: table => new
                {
                    ErrorLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Error = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLog", x => x.ErrorLogId);
                    table.ForeignKey(
                        name: "FK_ErrorLog_Profiles_UserId",
                        column: x => x.UserId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
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
                name: "Topics",
                columns: table => new
                {
                    TopicId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameType = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Topics", x => x.TopicId);
                    table.ForeignKey(
                        name: "FK_Topics_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TopicId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Comments_Topics_TopicId",
                        column: x => x.TopicId,
                        principalTable: "Topics",
                        principalColumn: "TopicId");
                });

            migrationBuilder.CreateTable(
                name: "Reactions",
                columns: table => new
                {
                    ReactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReactionEmoji = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CommentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reactions", x => x.ReactionId);
                    table.ForeignKey(
                        name: "FK_Reactions_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId");
                    table.ForeignKey(
                        name: "FK_Reactions_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "EventCS",
                columns: table => new
                {
                    EventCSID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventAPIID = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeagueName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeType = table.Column<int>(type: "int", nullable: false),
                    Finished = table.Column<bool>(type: "bit", nullable: false),
                    BeginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MatchesCSAPIID = table.Column<int>(type: "int", nullable: false),
                    PrizePool = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WinnerTeamTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WinnerTeamAPIID = table.Column<int>(type: "int", nullable: true),
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tier = table.Column<string>(type: "nvarchar(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventCS", x => x.EventCSID);
                });

            migrationBuilder.CreateTable(
                name: "MatchesCS",
                columns: table => new
                {
                    MatchesCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchesAPIID = table.Column<int>(type: "int", nullable: false),
                    EventCSID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EventAPIID = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BeginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    TimeType = table.Column<int>(type: "int", nullable: false),
                    HaveStats = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfGames = table.Column<int>(type: "int", nullable: true),
                    WinnerTeamAPIId = table.Column<int>(type: "int", nullable: true),
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tier = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    LiveSupported = table.Column<bool>(type: "bit", nullable: false),
                    LeagueName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeagueId = table.Column<int>(type: "int", nullable: true),
                    LeagueLink = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchesCS", x => x.MatchesCSId);
                    table.ForeignKey(
                        name: "FK_MatchesCS_EventCS_EventCSID",
                        column: x => x.EventCSID,
                        principalTable: "EventCS",
                        principalColumn: "EventCSID");
                });

            migrationBuilder.CreateTable(
                name: "EventVal",
                columns: table => new
                {
                    EventValID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventAPIID = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LeagueName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EventLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeType = table.Column<int>(type: "int", nullable: false),
                    Finished = table.Column<bool>(type: "bit", nullable: false),
                    BeginAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    MatchesValAPIID = table.Column<int>(type: "int", nullable: false),
                    PrizePool = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WinnerTeamTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WinnerTeamAPIID = table.Column<int>(type: "int", nullable: false),
                    WinnerTeamName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tier = table.Column<string>(type: "nvarchar(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventVal", x => x.EventValID);
                });

            migrationBuilder.CreateTable(
                name: "MatchesVal",
                columns: table => new
                {
                    MatchesValId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchesAPIID = table.Column<int>(type: "int", nullable: false),
                    EventValID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EventAPIID = table.Column<int>(type: "int", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeginAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    TimeType = table.Column<int>(type: "int", nullable: false),
                    HaveStats = table.Column<bool>(type: "bit", nullable: false),
                    NumberOfGames = table.Column<int>(type: "int", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_MatchesVal_EventVal_EventValID",
                        column: x => x.EventValID,
                        principalTable: "EventVal",
                        principalColumn: "EventValID");
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
                name: "Team",
                columns: table => new
                {
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeamAPIID = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoachName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorldRank = table.Column<int>(type: "int", nullable: true),
                    Winnings = table.Column<int>(type: "int", nullable: true),
                    Losses = table.Column<int>(type: "int", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Game = table.Column<int>(type: "int", nullable: true),
                    EventCSID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EventValID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FavTeamsListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MatchesCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MatchesValId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Team", x => x.TeamId);
                    table.ForeignKey(
                        name: "FK_Team_EventCS_EventCSID",
                        column: x => x.EventCSID,
                        principalTable: "EventCS",
                        principalColumn: "EventCSID");
                    table.ForeignKey(
                        name: "FK_Team_EventVal_EventValID",
                        column: x => x.EventValID,
                        principalTable: "EventVal",
                        principalColumn: "EventValID");
                    table.ForeignKey(
                        name: "FK_Team_FavTeamsList_FavTeamsListId",
                        column: x => x.FavTeamsListId,
                        principalTable: "FavTeamsList",
                        principalColumn: "FavTeamsListId");
                    table.ForeignKey(
                        name: "FK_Team_MatchesCS_MatchesCSId",
                        column: x => x.MatchesCSId,
                        principalTable: "MatchesCS",
                        principalColumn: "MatchesCSId");
                    table.ForeignKey(
                        name: "FK_Team_MatchesVal_MatchesValId",
                        column: x => x.MatchesValId,
                        principalTable: "MatchesVal",
                        principalColumn: "MatchesValId");
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
                    WinnerTeamTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_MatchCS_Team_WinnerTeamTeamId",
                        column: x => x.WinnerTeamTeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId");
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
                    WinnerTeamTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_MatchVal_Team_WinnerTeamTeamId",
                        column: x => x.WinnerTeamTeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId");
                });

            migrationBuilder.CreateTable(
                name: "Player",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerAPIId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Age = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<float>(type: "real", nullable: false),
                    Nationality = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Flag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Game = table.Column<int>(type: "int", nullable: false),
                    FavPlayerListId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "MatchTeamsCS",
                columns: table => new
                {
                    MatchCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchCSAPIID = table.Column<int>(type: "int", nullable: false),
                    TeamCSTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_MatchTeamsCS_Team_TeamCSTeamId",
                        column: x => x.TeamCSTeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId");
                });

            migrationBuilder.CreateTable(
                name: "MatchTeamsVal",
                columns: table => new
                {
                    MatchValId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchValAPIID = table.Column<int>(type: "int", nullable: false),
                    TeamValTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.ForeignKey(
                        name: "FK_MatchTeamsVal_Team_TeamValTeamId",
                        column: x => x.TeamValTeamId,
                        principalTable: "Team",
                        principalColumn: "TeamId");
                });

            migrationBuilder.CreateTable(
                name: "MatchPlayerStatsCS",
                columns: table => new
                {
                    MatchPlayerStatsCSID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchCSId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchAPIID = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerAPIId = table.Column<int>(type: "int", nullable: false),
                    Kills = table.Column<int>(type: "int", nullable: false),
                    Deaths = table.Column<int>(type: "int", nullable: false),
                    Assists = table.Column<int>(type: "int", nullable: false),
                    FlashAssist = table.Column<int>(type: "int", nullable: false),
                    ADR = table.Column<double>(type: "float", nullable: false),
                    HeadShots = table.Column<double>(type: "float", nullable: false),
                    KD_Diff = table.Column<double>(type: "float", nullable: false),
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
                    table.ForeignKey(
                        name: "FK_MatchPlayerStatsCS_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MatchPlayerStatsVal",
                columns: table => new
                {
                    MatchPlayerStatsValID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MatchValId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MatchAPIID = table.Column<int>(type: "int", nullable: false),
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlayerAPIId = table.Column<int>(type: "int", nullable: false),
                    Kills = table.Column<int>(type: "int", nullable: false),
                    Deaths = table.Column<int>(type: "int", nullable: false),
                    Assists = table.Column<int>(type: "int", nullable: false),
                    ADR = table.Column<double>(type: "float", nullable: false),
                    Kast = table.Column<float>(type: "real", nullable: false),
                    HeadShots = table.Column<double>(type: "float", nullable: false),
                    KD_Diff = table.Column<double>(type: "float", nullable: false),
                    PlayerName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MatchPlayerStatsVal", x => x.MatchPlayerStatsValID);
                    table.ForeignKey(
                        name: "FK_MatchPlayerStatsVal_MatchVal_MatchValId",
                        column: x => x.MatchValId,
                        principalTable: "MatchVal",
                        principalColumn: "MatchValId");
                    table.ForeignKey(
                        name: "FK_MatchPlayerStatsVal_Player_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Player",
                        principalColumn: "PlayerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ProfileId",
                table: "AspNetUsers",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_ProfileId",
                table: "Comments",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_TopicId",
                table: "Comments",
                column: "TopicId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorLog_UserId",
                table: "ErrorLog",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EventCS_WinnerTeamTeamId",
                table: "EventCS",
                column: "WinnerTeamTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_EventVal_WinnerTeamTeamId",
                table: "EventVal",
                column: "WinnerTeamTeamId");

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
                name: "IX_MatchCS_WinnerTeamTeamId",
                table: "MatchCS",
                column: "WinnerTeamTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchesCS_EventCSID",
                table: "MatchesCS",
                column: "EventCSID");

            migrationBuilder.CreateIndex(
                name: "IX_MatchesVal_EventValID",
                table: "MatchesVal",
                column: "EventValID");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayerStatsCS_MatchCSId",
                table: "MatchPlayerStatsCS",
                column: "MatchCSId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayerStatsCS_PlayerId",
                table: "MatchPlayerStatsCS",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayerStatsVal_MatchValId",
                table: "MatchPlayerStatsVal",
                column: "MatchValId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchPlayerStatsVal_PlayerId",
                table: "MatchPlayerStatsVal",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTeamsCS_MatchCSId1",
                table: "MatchTeamsCS",
                column: "MatchCSId1");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTeamsCS_TeamCSTeamId",
                table: "MatchTeamsCS",
                column: "TeamCSTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTeamsVal_MatchValId1",
                table: "MatchTeamsVal",
                column: "MatchValId1");

            migrationBuilder.CreateIndex(
                name: "IX_MatchTeamsVal_TeamValTeamId",
                table: "MatchTeamsVal",
                column: "TeamValTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchVal_MatchesValId",
                table: "MatchVal",
                column: "MatchesValId");

            migrationBuilder.CreateIndex(
                name: "IX_MatchVal_WinnerTeamTeamId",
                table: "MatchVal",
                column: "WinnerTeamTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_FavPlayerListId",
                table: "Player",
                column: "FavPlayerListId");

            migrationBuilder.CreateIndex(
                name: "IX_Player_TeamId",
                table: "Player",
                column: "TeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_CommentId",
                table: "Reactions",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_ProfileId",
                table: "Reactions",
                column: "ProfileId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Stream_MatchesCSId",
                table: "Stream",
                column: "MatchesCSId");

            migrationBuilder.CreateIndex(
                name: "IX_Stream_MatchesValId",
                table: "Stream",
                column: "MatchesValId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_EventCSID",
                table: "Team",
                column: "EventCSID");

            migrationBuilder.CreateIndex(
                name: "IX_Team_EventValID",
                table: "Team",
                column: "EventValID");

            migrationBuilder.CreateIndex(
                name: "IX_Team_FavTeamsListId",
                table: "Team",
                column: "FavTeamsListId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_MatchesCSId",
                table: "Team",
                column: "MatchesCSId");

            migrationBuilder.CreateIndex(
                name: "IX_Team_MatchesValId",
                table: "Team",
                column: "MatchesValId");

            migrationBuilder.CreateIndex(
                name: "IX_Topics_ProfileId",
                table: "Topics",
                column: "ProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventCS_Team_WinnerTeamTeamId",
                table: "EventCS",
                column: "WinnerTeamTeamId",
                principalTable: "Team",
                principalColumn: "TeamId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventVal_Team_WinnerTeamTeamId",
                table: "EventVal",
                column: "WinnerTeamTeamId",
                principalTable: "Team",
                principalColumn: "TeamId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FavTeamsList_Profiles_ProfileId",
                table: "FavTeamsList");

            migrationBuilder.DropForeignKey(
                name: "FK_EventCS_Team_WinnerTeamTeamId",
                table: "EventCS");

            migrationBuilder.DropForeignKey(
                name: "FK_EventVal_Team_WinnerTeamTeamId",
                table: "EventVal");

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
                name: "MatchPlayerStatsCS");

            migrationBuilder.DropTable(
                name: "MatchPlayerStatsVal");

            migrationBuilder.DropTable(
                name: "MatchTeamsCS");

            migrationBuilder.DropTable(
                name: "MatchTeamsVal");

            migrationBuilder.DropTable(
                name: "Reactions");

            migrationBuilder.DropTable(
                name: "Score");

            migrationBuilder.DropTable(
                name: "Stream");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Player");

            migrationBuilder.DropTable(
                name: "MatchCS");

            migrationBuilder.DropTable(
                name: "MatchVal");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "FavPlayerList");

            migrationBuilder.DropTable(
                name: "Topics");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "Team");

            migrationBuilder.DropTable(
                name: "FavTeamsList");

            migrationBuilder.DropTable(
                name: "MatchesCS");

            migrationBuilder.DropTable(
                name: "MatchesVal");

            migrationBuilder.DropTable(
                name: "EventCS");

            migrationBuilder.DropTable(
                name: "EventVal");
        }
    }
}
