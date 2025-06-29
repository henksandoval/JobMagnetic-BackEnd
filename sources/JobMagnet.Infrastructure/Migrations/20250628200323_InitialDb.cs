using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobMagnet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IconClass = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondLastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AddedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkillCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactAliases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ContactTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactAliases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactAliases_ContactTypes_ContactTypeId",
                        column: x => x.ContactTypeId,
                        principalTable: "ContactTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlLink = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UrlVideo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PublicProfileIdentifier",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileSlugUrl = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ViewCount = table.Column<long>(type: "bigint", nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PublicProfileIdentifier", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PublicProfileIdentifier_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Resumes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    About = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Overview = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Suffix = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resumes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Resumes_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Overview = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillSets_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Summaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Introduction = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Summaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Summaries_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Talents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Talents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Talents_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Testimonials",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Feedback = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Testimonials", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Testimonials_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IconUrl = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillTypes_SkillCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "SkillCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ContactInfo",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HeadlineId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactInfo_ContactTypes_ContactTypeId",
                        column: x => x.ContactTypeId,
                        principalTable: "ContactTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ContactInfo_Resumes_HeadlineId",
                        column: x => x.HeadlineId,
                        principalTable: "Resumes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Educations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Degree = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstitutionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InstitutionLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CareerHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Educations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Educations_Summaries_CareerHistoryId",
                        column: x => x.CareerHistoryId,
                        principalTable: "Summaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkExperiences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyLocation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CareerHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkExperiences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkExperiences_Summaries_CareerHistoryId",
                        column: x => x.CareerHistoryId,
                        principalTable: "Summaries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProficiencyLevel = table.Column<int>(type: "int", nullable: false),
                    Rank = table.Column<int>(type: "int", nullable: false),
                    SkillSetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_SkillSets_SkillSetId",
                        column: x => x.SkillSetId,
                        principalTable: "SkillSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Skills_SkillTypes_SkillTypeId",
                        column: x => x.SkillTypeId,
                        principalTable: "SkillTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SkillTypeAliases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SkillTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillTypeAliases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SkillTypeAliases_SkillTypes_SkillTypeId",
                        column: x => x.SkillTypeId,
                        principalTable: "SkillTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkResponsibilities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WorkExperienceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkResponsibilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkResponsibilities_WorkExperiences_WorkExperienceId",
                        column: x => x.WorkExperienceId,
                        principalTable: "WorkExperiences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ContactTypes",
                columns: new[] { "Id", "AddedAt", "DeletedAt", "IconClass", "IconUrl", "LastModifiedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("0000012d-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bx-envelope", null, null, "Email" },
                    { new Guid("0000012e-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bx-mobile", null, null, "Mobile Phone" },
                    { new Guid("0000012f-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bx-phone", null, null, "Home Phone" },
                    { new Guid("00000130-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bx-phone-call", null, null, "Work Phone" },
                    { new Guid("00000131-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bx-globe", null, null, "Website" },
                    { new Guid("00000132-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bxl-linkedin", null, null, "LinkedIn" },
                    { new Guid("00000133-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bxl-github", null, null, "GitHub" },
                    { new Guid("00000134-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bxl-twitter", null, null, "Twitter" },
                    { new Guid("00000135-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bxl-facebook", null, null, "Facebook" },
                    { new Guid("00000136-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bxl-instagram", null, null, "Instagram" },
                    { new Guid("00000137-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bxl-youtube", null, null, "YouTube" },
                    { new Guid("00000138-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bxl-whatsapp", null, null, "WhatsApp" },
                    { new Guid("00000139-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bxl-telegram", null, null, "Telegram" },
                    { new Guid("0000013a-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bxl-snapchat", null, null, "Snapchat" },
                    { new Guid("0000013b-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bxl-pinterest", null, null, "Pinterest" },
                    { new Guid("0000013c-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bxl-skype", null, null, "Skype" },
                    { new Guid("0000013d-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bxl-discord", null, null, "Discord" },
                    { new Guid("0000013e-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bxl-twitch", null, null, "Twitch" },
                    { new Guid("0000013f-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bxl-tiktok", null, null, "TikTok" },
                    { new Guid("00000140-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bxl-reddit", null, null, "Reddit" },
                    { new Guid("00000141-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "bx bxl-vimeo", null, null, "Vimeo" }
                });

            migrationBuilder.InsertData(
                table: "SkillCategories",
                columns: new[] { "Id", "AddedAt", "DeletedAt", "LastModifiedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("00000001-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "General" },
                    { new Guid("00000002-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Software Development" }
                });

            migrationBuilder.InsertData(
                table: "ContactAliases",
                columns: new[] { "Id", "Alias", "ContactTypeId" },
                values: new object[,]
                {
                    { new Guid("00000191-0000-0000-0000-000000000000"), "Correo Electrónico", new Guid("0000012d-0000-0000-0000-000000000000") },
                    { new Guid("00000192-0000-0000-0000-000000000000"), "E-mail", new Guid("0000012d-0000-0000-0000-000000000000") },
                    { new Guid("00000193-0000-0000-0000-000000000000"), "Phone", new Guid("0000012e-0000-0000-0000-000000000000") },
                    { new Guid("00000194-0000-0000-0000-000000000000"), "Teléfonos", new Guid("0000012e-0000-0000-0000-000000000000") },
                    { new Guid("00000195-0000-0000-0000-000000000000"), "Teléfono Móvil", new Guid("0000012e-0000-0000-0000-000000000000") },
                    { new Guid("00000196-0000-0000-0000-000000000000"), "Celular", new Guid("0000012e-0000-0000-0000-000000000000") },
                    { new Guid("00000197-0000-0000-0000-000000000000"), "Móvil", new Guid("0000012e-0000-0000-0000-000000000000") },
                    { new Guid("00000198-0000-0000-0000-000000000000"), "Teléfono Fijo", new Guid("0000012f-0000-0000-0000-000000000000") },
                    { new Guid("00000199-0000-0000-0000-000000000000"), "Teléfono de Casa", new Guid("0000012f-0000-0000-0000-000000000000") },
                    { new Guid("0000019a-0000-0000-0000-000000000000"), "Teléfono Casa", new Guid("0000012f-0000-0000-0000-000000000000") },
                    { new Guid("0000019b-0000-0000-0000-000000000000"), "Teléfono Trabajo", new Guid("00000130-0000-0000-0000-000000000000") },
                    { new Guid("0000019c-0000-0000-0000-000000000000"), "Teléfono Oficina", new Guid("00000130-0000-0000-0000-000000000000") },
                    { new Guid("0000019d-0000-0000-0000-000000000000"), "Teléfono de Trabajo", new Guid("00000130-0000-0000-0000-000000000000") },
                    { new Guid("0000019e-0000-0000-0000-000000000000"), "Teléfono de Oficina", new Guid("00000130-0000-0000-0000-000000000000") },
                    { new Guid("0000019f-0000-0000-0000-000000000000"), "Web Site", new Guid("00000131-0000-0000-0000-000000000000") },
                    { new Guid("000001a0-0000-0000-0000-000000000000"), "Web-site", new Guid("00000131-0000-0000-0000-000000000000") },
                    { new Guid("000001a1-0000-0000-0000-000000000000"), "Sitio Web", new Guid("00000131-0000-0000-0000-000000000000") },
                    { new Guid("000001a2-0000-0000-0000-000000000000"), "Página Web", new Guid("00000131-0000-0000-0000-000000000000") },
                    { new Guid("000001a3-0000-0000-0000-000000000000"), "Blog", new Guid("00000131-0000-0000-0000-000000000000") },
                    { new Guid("000001a4-0000-0000-0000-000000000000"), "Portafolio", new Guid("00000131-0000-0000-0000-000000000000") },
                    { new Guid("000001a5-0000-0000-0000-000000000000"), "X", new Guid("00000134-0000-0000-0000-000000000000") },
                    { new Guid("000001a6-0000-0000-0000-000000000000"), "Wasap", new Guid("00000138-0000-0000-0000-000000000000") }
                });

            migrationBuilder.InsertData(
                table: "SkillTypes",
                columns: new[] { "Id", "AddedAt", "CategoryId", "DeletedAt", "IconUrl", "LastModifiedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("00000065-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/html5", null, "HTML" },
                    { new Guid("00000066-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/css3", null, "CSS" },
                    { new Guid("00000067-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/javascript", null, "JavaScript" },
                    { new Guid("00000068-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/dotnet", null, "C#" },
                    { new Guid("00000069-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/typescript", null, "TypeScript" },
                    { new Guid("0000006a-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/angular", null, "Angular" },
                    { new Guid("0000006b-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/postgresql", null, "PostgreSQL" },
                    { new Guid("0000006c-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/react", null, "React" },
                    { new Guid("0000006d-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/bootstrap", null, "Bootstrap" },
                    { new Guid("0000006e-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/vuedotjs", null, "Vue" },
                    { new Guid("0000006f-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/git", null, "Git" },
                    { new Guid("00000070-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/blazor", null, "Blazor" },
                    { new Guid("00000071-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/rabbitmq", null, "RabbitMQ" },
                    { new Guid("00000072-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/docker", null, "Docker" }
                });

            migrationBuilder.InsertData(
                table: "SkillTypeAliases",
                columns: new[] { "Id", "Alias", "SkillTypeId" },
                values: new object[,]
                {
                    { new Guid("000000c9-0000-0000-0000-000000000000"), "JS", new Guid("00000067-0000-0000-0000-000000000000") },
                    { new Guid("000000ca-0000-0000-0000-000000000000"), "TS", new Guid("00000069-0000-0000-0000-000000000000") },
                    { new Guid("000000cb-0000-0000-0000-000000000000"), "Postgres", new Guid("0000006b-0000-0000-0000-000000000000") },
                    { new Guid("000000cc-0000-0000-0000-000000000000"), "Vue.js", new Guid("0000006e-0000-0000-0000-000000000000") },
                    { new Guid("000000cd-0000-0000-0000-000000000000"), "Rabbit MQ", new Guid("00000071-0000-0000-0000-000000000000") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactAliases_ContactTypeId",
                table: "ContactAliases",
                column: "ContactTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_ContactTypeId",
                table: "ContactInfo",
                column: "ContactTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_HeadlineId",
                table: "ContactInfo",
                column: "HeadlineId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactTypes_Name",
                table: "ContactTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Educations_CareerHistoryId",
                table: "Educations",
                column: "CareerHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProfileId",
                table: "Projects",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicProfileIdentifier_ProfileId",
                table: "PublicProfileIdentifier",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_PublicProfileIdentifier_ProfileSlugUrl",
                table: "PublicProfileIdentifier",
                column: "ProfileSlugUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Resumes_ProfileId",
                table: "Resumes",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkillCategories_Name",
                table: "SkillCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SkillSetId",
                table: "Skills",
                column: "SkillSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SkillTypeId",
                table: "Skills",
                column: "SkillTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillSets_ProfileId",
                table: "SkillSets",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SkillTypeAliases_SkillTypeId",
                table: "SkillTypeAliases",
                column: "SkillTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillTypes_CategoryId",
                table: "SkillTypes",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Summaries_ProfileId",
                table: "Summaries",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Talents_ProfileId",
                table: "Talents",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_ProfileId",
                table: "Testimonials",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperiences_CareerHistoryId",
                table: "WorkExperiences",
                column: "CareerHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkResponsibilities_WorkExperienceId",
                table: "WorkResponsibilities",
                column: "WorkExperienceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactAliases");

            migrationBuilder.DropTable(
                name: "ContactInfo");

            migrationBuilder.DropTable(
                name: "Educations");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "PublicProfileIdentifier");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "SkillTypeAliases");

            migrationBuilder.DropTable(
                name: "Talents");

            migrationBuilder.DropTable(
                name: "Testimonials");

            migrationBuilder.DropTable(
                name: "WorkResponsibilities");

            migrationBuilder.DropTable(
                name: "ContactTypes");

            migrationBuilder.DropTable(
                name: "Resumes");

            migrationBuilder.DropTable(
                name: "SkillSets");

            migrationBuilder.DropTable(
                name: "SkillTypes");

            migrationBuilder.DropTable(
                name: "WorkExperiences");

            migrationBuilder.DropTable(
                name: "SkillCategories");

            migrationBuilder.DropTable(
                name: "Summaries");

            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
