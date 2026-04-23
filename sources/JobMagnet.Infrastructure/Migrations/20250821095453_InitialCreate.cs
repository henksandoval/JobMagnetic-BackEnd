using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobMagnet.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                });

            migrationBuilder.CreateTable(
                name: "ContactTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    IconClass = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    FirstName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    MiddleName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    SecondLastName = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateOnly>(type: "date", nullable: true),
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
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApplicationIdentityUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    DeletedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_AspNetUsers_ApplicationIdentityUserId",
                        column: x => x.ApplicationIdentityUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ContactTypeAliases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ContactTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactTypeAliases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ContactTypeAliases_ContactTypes_ContactTypeId",
                        column: x => x.ContactTypeId,
                        principalTable: "ContactTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CareerHistory",
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
                    table.PrimaryKey("PK_CareerHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CareerHistory_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProfileHeaders",
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
                    table.PrimaryKey("PK_ProfileHeaders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileHeaders_Profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalTable: "Profiles",
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
                name: "Talents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AddedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
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
                name: "VanityUrls",
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
                    table.PrimaryKey("PK_VanityUrls", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VanityUrls_Profiles_ProfileId",
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
                name: "AcademicDegree",
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
                    table.PrimaryKey("PK_AcademicDegree", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicDegree_CareerHistory_CareerHistoryId",
                        column: x => x.CareerHistoryId,
                        principalTable: "CareerHistory",
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
                        name: "FK_WorkExperiences_CareerHistory_CareerHistoryId",
                        column: x => x.CareerHistoryId,
                        principalTable: "CareerHistory",
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
                    ProfileHeaderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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
                        name: "FK_ContactInfo_ProfileHeaders_ProfileHeaderId",
                        column: x => x.ProfileHeaderId,
                        principalTable: "ProfileHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProficiencyLevel = table.Column<int>(type: "int", nullable: false),
                    Position = table.Column<int>(type: "int", nullable: false),
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
                name: "WorkHighlight",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WorkExperienceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkHighlight", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkHighlight_WorkExperiences_WorkExperienceId",
                        column: x => x.WorkExperienceId,
                        principalTable: "WorkExperiences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ContactTypes",
                columns: new[] { "Id", "DeletedAt", "IconClass", "IconUrl", "LastModifiedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("0000012d-0000-0000-0000-000000000000"), null, "bx bx-envelope", null, null, "Email" },
                    { new Guid("0000012e-0000-0000-0000-000000000000"), null, "bx bx-mobile", null, null, "Mobile Phone" },
                    { new Guid("0000012f-0000-0000-0000-000000000000"), null, "bx bx-phone", null, null, "Home Phone" },
                    { new Guid("00000130-0000-0000-0000-000000000000"), null, "bx bx-phone-call", null, null, "Work Phone" },
                    { new Guid("00000131-0000-0000-0000-000000000000"), null, "bx bx-globe", null, null, "Website" },
                    { new Guid("00000132-0000-0000-0000-000000000000"), null, "bx bxl-linkedin", null, null, "LinkedIn" },
                    { new Guid("00000133-0000-0000-0000-000000000000"), null, "bx bxl-github", null, null, "GitHub" },
                    { new Guid("00000134-0000-0000-0000-000000000000"), null, "bx bxl-twitter", null, null, "Twitter" },
                    { new Guid("00000135-0000-0000-0000-000000000000"), null, "bx bxl-facebook", null, null, "Facebook" },
                    { new Guid("00000136-0000-0000-0000-000000000000"), null, "bx bxl-instagram", null, null, "Instagram" },
                    { new Guid("00000137-0000-0000-0000-000000000000"), null, "bx bxl-youtube", null, null, "YouTube" },
                    { new Guid("00000138-0000-0000-0000-000000000000"), null, "bx bxl-whatsapp", null, null, "WhatsApp" },
                    { new Guid("00000139-0000-0000-0000-000000000000"), null, "bx bxl-telegram", null, null, "Telegram" },
                    { new Guid("0000013a-0000-0000-0000-000000000000"), null, "bx bxl-snapchat", null, null, "Snapchat" },
                    { new Guid("0000013b-0000-0000-0000-000000000000"), null, "bx bxl-pinterest", null, null, "Pinterest" },
                    { new Guid("0000013c-0000-0000-0000-000000000000"), null, "bx bxl-skype", null, null, "Skype" },
                    { new Guid("0000013d-0000-0000-0000-000000000000"), null, "bx bxl-discord", null, null, "Discord" },
                    { new Guid("0000013e-0000-0000-0000-000000000000"), null, "bx bxl-twitch", null, null, "Twitch" },
                    { new Guid("0000013f-0000-0000-0000-000000000000"), null, "bx bxl-tiktok", null, null, "TikTok" },
                    { new Guid("00000140-0000-0000-0000-000000000000"), null, "bx bxl-reddit", null, null, "Reddit" },
                    { new Guid("00000141-0000-0000-0000-000000000000"), null, "bx bxl-vimeo", null, null, "Vimeo" }
                });

            migrationBuilder.InsertData(
                table: "SkillCategories",
                columns: new[] { "Id", "AddedAt", "DeletedAt", "LastModifiedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("00000001-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "General" },
                    { new Guid("00000002-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Software Development" },
                    { new Guid("00000003-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Bases de Datos" },
                    { new Guid("00000004-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Cloud y DevOps" },
                    { new Guid("00000005-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Arquitectura y Patrones" },
                    { new Guid("00000006-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Testing" },
                    { new Guid("00000007-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Data Science y ML" },
                    { new Guid("00000008-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Metodologías y Gestión" },
                    { new Guid("00000009-0000-0000-0000-000000000000"), new DateTimeOffset(new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, null, "Habilidades Blandas" }
                });

            migrationBuilder.InsertData(
                table: "ContactTypeAliases",
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
                columns: new[] { "Id", "CategoryId", "DeletedAt", "IconUrl", "LastModifiedAt", "Name" },
                values: new object[,]
                {
                    { new Guid("00000065-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/html5", null, "HTML" },
                    { new Guid("00000066-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/css3", null, "CSS" },
                    { new Guid("00000067-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/javascript", null, "JavaScript" },
                    { new Guid("00000068-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/csharp", null, "C#" },
                    { new Guid("00000069-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/typescript", null, "TypeScript" },
                    { new Guid("0000006a-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/angular", null, "Angular" },
                    { new Guid("0000006b-0000-0000-0000-000000000000"), new Guid("00000003-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/postgresql", null, "PostgreSQL" },
                    { new Guid("0000006c-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/react", null, "React" },
                    { new Guid("0000006d-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/bootstrap", null, "Bootstrap" },
                    { new Guid("0000006e-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/vuedotjs", null, "Vue" },
                    { new Guid("0000006f-0000-0000-0000-000000000000"), new Guid("00000004-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/git", null, "Git" },
                    { new Guid("00000070-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/blazor", null, "Blazor" },
                    { new Guid("00000071-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/rabbitmq", null, "RabbitMQ" },
                    { new Guid("00000072-0000-0000-0000-000000000000"), new Guid("00000004-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/docker", null, "Docker" },
                    { new Guid("00000073-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/java", null, "Java" },
                    { new Guid("00000074-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/python", null, "Python" },
                    { new Guid("00000075-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/go", null, "Go" },
                    { new Guid("00000076-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/rust", null, "Rust" },
                    { new Guid("00000077-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/kotlin", null, "Kotlin" },
                    { new Guid("00000078-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/swift", null, "Swift" },
                    { new Guid("00000079-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/php", null, "PHP" },
                    { new Guid("0000007a-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/ruby", null, "Ruby" },
                    { new Guid("0000007b-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/cplusplus", null, "C++" },
                    { new Guid("0000007c-0000-0000-0000-000000000000"), new Guid("00000003-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/azuredatastudio", null, "SQL" },
                    { new Guid("0000007d-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/dotnet", null, ".NET" },
                    { new Guid("0000007e-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/dotnet", null, "ASP.NET Core" },
                    { new Guid("0000007f-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/springboot", null, "Spring Boot" },
                    { new Guid("00000080-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/django", null, "Django" },
                    { new Guid("00000081-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/flask", null, "Flask" },
                    { new Guid("00000082-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/nodedotjs", null, "Node.js" },
                    { new Guid("00000083-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/express", null, "Express.js" },
                    { new Guid("00000084-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/rubyonrails", null, "Ruby on Rails" },
                    { new Guid("00000085-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/laravel", null, "Laravel" },
                    { new Guid("00000086-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/dotnet", null, "Entity Framework Core" },
                    { new Guid("00000087-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/svelte", null, "Svelte" },
                    { new Guid("00000088-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/jquery", null, "jQuery" },
                    { new Guid("00000089-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/sass", null, "Sass" },
                    { new Guid("0000008a-0000-0000-0000-000000000000"), new Guid("00000002-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/tailwindcss", null, "Tailwind CSS" },
                    { new Guid("0000008b-0000-0000-0000-000000000000"), new Guid("00000003-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/microsoftsqlserver", null, "SQL Server" },
                    { new Guid("0000008c-0000-0000-0000-000000000000"), new Guid("00000003-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/mysql", null, "MySQL" },
                    { new Guid("0000008d-0000-0000-0000-000000000000"), new Guid("00000003-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/oracle", null, "Oracle Database" },
                    { new Guid("0000008e-0000-0000-0000-000000000000"), new Guid("00000003-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/mongodb", null, "MongoDB" },
                    { new Guid("0000008f-0000-0000-0000-000000000000"), new Guid("00000003-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/redis", null, "Redis" },
                    { new Guid("00000090-0000-0000-0000-000000000000"), new Guid("00000003-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/apachecassandra", null, "Cassandra" },
                    { new Guid("00000091-0000-0000-0000-000000000000"), new Guid("00000003-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/sqlite", null, "SQLite" },
                    { new Guid("00000092-0000-0000-0000-000000000000"), new Guid("00000003-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/azurecosmosdb", null, "Cosmos DB" },
                    { new Guid("00000093-0000-0000-0000-000000000000"), new Guid("00000003-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/amazondynamodb", null, "DynamoDB" },
                    { new Guid("00000094-0000-0000-0000-000000000000"), new Guid("00000004-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/microsoftazure", null, "Microsoft Azure" },
                    { new Guid("00000095-0000-0000-0000-000000000000"), new Guid("00000004-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/amazonaws", null, "Amazon Web Services" },
                    { new Guid("00000096-0000-0000-0000-000000000000"), new Guid("00000004-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/googlecloud", null, "Google Cloud Platform" },
                    { new Guid("00000097-0000-0000-0000-000000000000"), new Guid("00000004-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/kubernetes", null, "Kubernetes" },
                    { new Guid("00000098-0000-0000-0000-000000000000"), new Guid("00000004-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/terraform", null, "Terraform" },
                    { new Guid("00000099-0000-0000-0000-000000000000"), new Guid("00000004-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/ansible", null, "Ansible" },
                    { new Guid("0000009a-0000-0000-0000-000000000000"), new Guid("00000004-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/jenkins", null, "Jenkins" },
                    { new Guid("0000009b-0000-0000-0000-000000000000"), new Guid("00000004-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/azuredevops", null, "Azure DevOps" },
                    { new Guid("0000009c-0000-0000-0000-000000000000"), new Guid("00000004-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/githubactions", null, "GitHub Actions" },
                    { new Guid("0000009d-0000-0000-0000-000000000000"), new Guid("00000004-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/gitlab", null, "GitLab CI" },
                    { new Guid("0000009e-0000-0000-0000-000000000000"), new Guid("00000004-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/prometheus", null, "Prometheus" },
                    { new Guid("0000009f-0000-0000-0000-000000000000"), new Guid("00000004-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/grafana", null, "Grafana" },
                    { new Guid("000000a0-0000-0000-0000-000000000000"), new Guid("00000005-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Microservices Architecture" },
                    { new Guid("000000a1-0000-0000-0000-000000000000"), new Guid("00000005-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Domain-Driven Design" },
                    { new Guid("000000a2-0000-0000-0000-000000000000"), new Guid("00000005-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "SOLID Principles" },
                    { new Guid("000000a3-0000-0000-0000-000000000000"), new Guid("00000005-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "REST APIs" },
                    { new Guid("000000a4-0000-0000-0000-000000000000"), new Guid("00000005-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/graphql", null, "GraphQL" },
                    { new Guid("000000a5-0000-0000-0000-000000000000"), new Guid("00000005-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Event-Driven Architecture" },
                    { new Guid("000000a6-0000-0000-0000-000000000000"), new Guid("00000005-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "CQRS" },
                    { new Guid("000000a7-0000-0000-0000-000000000000"), new Guid("00000005-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Design Patterns" },
                    { new Guid("000000a8-0000-0000-0000-000000000000"), new Guid("00000006-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Unit Testing" },
                    { new Guid("000000a9-0000-0000-0000-000000000000"), new Guid("00000006-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Integration Testing" },
                    { new Guid("000000aa-0000-0000-0000-000000000000"), new Guid("00000006-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "End-to-End Testing" },
                    { new Guid("000000ab-0000-0000-0000-000000000000"), new Guid("00000006-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/xunit", null, "xUnit" },
                    { new Guid("000000ac-0000-0000-0000-000000000000"), new Guid("00000006-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/nunit", null, "NUnit" },
                    { new Guid("000000ad-0000-0000-0000-000000000000"), new Guid("00000006-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/visualstudio", null, "MSTest" },
                    { new Guid("000000ae-0000-0000-0000-000000000000"), new Guid("00000006-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/selenium", null, "Selenium" },
                    { new Guid("000000af-0000-0000-0000-000000000000"), new Guid("00000006-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/cypress", null, "Cypress" },
                    { new Guid("000000b0-0000-0000-0000-000000000000"), new Guid("00000006-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/playwright", null, "Playwright" },
                    { new Guid("000000b1-0000-0000-0000-000000000000"), new Guid("00000006-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/jest", null, "Jest" },
                    { new Guid("000000b2-0000-0000-0000-000000000000"), new Guid("00000007-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Machine Learning" },
                    { new Guid("000000b3-0000-0000-0000-000000000000"), new Guid("00000007-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Data Analysis" },
                    { new Guid("000000b4-0000-0000-0000-000000000000"), new Guid("00000007-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/pandas", null, "Pandas" },
                    { new Guid("000000b5-0000-0000-0000-000000000000"), new Guid("00000007-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/numpy", null, "NumPy" },
                    { new Guid("000000b6-0000-0000-0000-000000000000"), new Guid("00000007-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/tensorflow", null, "TensorFlow" },
                    { new Guid("000000b7-0000-0000-0000-000000000000"), new Guid("00000007-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/pytorch", null, "PyTorch" },
                    { new Guid("000000b8-0000-0000-0000-000000000000"), new Guid("00000007-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/scikitlearn", null, "Scikit-learn" },
                    { new Guid("000000b9-0000-0000-0000-000000000000"), new Guid("00000007-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/powerbi", null, "Power BI" },
                    { new Guid("000000ba-0000-0000-0000-000000000000"), new Guid("00000007-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/tableau", null, "Tableau" },
                    { new Guid("000000bb-0000-0000-0000-000000000000"), new Guid("00000008-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Agile Methodologies" },
                    { new Guid("000000bc-0000-0000-0000-000000000000"), new Guid("00000008-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/scrumalliance", null, "Scrum" },
                    { new Guid("000000bd-0000-0000-0000-000000000000"), new Guid("00000008-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Kanban" },
                    { new Guid("000000be-0000-0000-0000-000000000000"), new Guid("00000008-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/jira", null, "JIRA" },
                    { new Guid("000000bf-0000-0000-0000-000000000000"), new Guid("00000008-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/confluence", null, "Confluence" },
                    { new Guid("000000c0-0000-0000-0000-000000000000"), new Guid("00000008-0000-0000-0000-000000000000"), null, "https://cdn.simpleicons.org/trello", null, "Trello" },
                    { new Guid("000000c1-0000-0000-0000-000000000000"), new Guid("00000009-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Team Leadership" },
                    { new Guid("000000c2-0000-0000-0000-000000000000"), new Guid("00000009-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Effective Communication" },
                    { new Guid("000000c3-0000-0000-0000-000000000000"), new Guid("00000009-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Problem Solving" },
                    { new Guid("000000c4-0000-0000-0000-000000000000"), new Guid("00000009-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Public Speaking" },
                    { new Guid("000000c5-0000-0000-0000-000000000000"), new Guid("00000009-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Time Management" },
                    { new Guid("000000c6-0000-0000-0000-000000000000"), new Guid("00000009-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Critical Thinking" },
                    { new Guid("000000c7-0000-0000-0000-000000000000"), new Guid("00000009-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Mentoring" },
                    { new Guid("000000c8-0000-0000-0000-000000000000"), new Guid("00000009-0000-0000-0000-000000000000"), null, "https://jobmagnet.com/default-icon.png", null, "Collaboration" }
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
                    { new Guid("000000cd-0000-0000-0000-000000000000"), "Rabbit MQ", new Guid("00000071-0000-0000-0000-000000000000") },
                    { new Guid("000000ce-0000-0000-0000-000000000000"), "HTML5", new Guid("00000065-0000-0000-0000-000000000000") },
                    { new Guid("000000cf-0000-0000-0000-000000000000"), "CSS3", new Guid("00000066-0000-0000-0000-000000000000") },
                    { new Guid("000000d0-0000-0000-0000-000000000000"), "ASP.NET", new Guid("0000007e-0000-0000-0000-000000000000") },
                    { new Guid("000000d1-0000-0000-0000-000000000000"), "Spring", new Guid("0000007f-0000-0000-0000-000000000000") },
                    { new Guid("000000d2-0000-0000-0000-000000000000"), "Node", new Guid("00000082-0000-0000-0000-000000000000") },
                    { new Guid("000000d3-0000-0000-0000-000000000000"), "Express", new Guid("00000083-0000-0000-0000-000000000000") },
                    { new Guid("000000d4-0000-0000-0000-000000000000"), "Rails", new Guid("00000084-0000-0000-0000-000000000000") },
                    { new Guid("000000d5-0000-0000-0000-000000000000"), "EF Core", new Guid("00000086-0000-0000-0000-000000000000") },
                    { new Guid("000000d6-0000-0000-0000-000000000000"), "SCSS", new Guid("00000089-0000-0000-0000-000000000000") },
                    { new Guid("000000d7-0000-0000-0000-000000000000"), "Oracle", new Guid("0000008d-0000-0000-0000-000000000000") },
                    { new Guid("000000d8-0000-0000-0000-000000000000"), "Azure", new Guid("00000094-0000-0000-0000-000000000000") },
                    { new Guid("000000d9-0000-0000-0000-000000000000"), "AWS", new Guid("00000095-0000-0000-0000-000000000000") },
                    { new Guid("000000da-0000-0000-0000-000000000000"), "GCP", new Guid("00000096-0000-0000-0000-000000000000") },
                    { new Guid("000000db-0000-0000-0000-000000000000"), "k8s", new Guid("00000097-0000-0000-0000-000000000000") },
                    { new Guid("000000dc-0000-0000-0000-000000000000"), "GitLab", new Guid("0000009d-0000-0000-0000-000000000000") },
                    { new Guid("000000dd-0000-0000-0000-000000000000"), "Microservicios", new Guid("000000a0-0000-0000-0000-000000000000") },
                    { new Guid("000000de-0000-0000-0000-000000000000"), "DDD", new Guid("000000a1-0000-0000-0000-000000000000") },
                    { new Guid("000000df-0000-0000-0000-000000000000"), "SOLID", new Guid("000000a2-0000-0000-0000-000000000000") },
                    { new Guid("000000e0-0000-0000-0000-000000000000"), "REST", new Guid("000000a3-0000-0000-0000-000000000000") },
                    { new Guid("000000e1-0000-0000-0000-000000000000"), "E2E Testing", new Guid("000000aa-0000-0000-0000-000000000000") },
                    { new Guid("000000e2-0000-0000-0000-000000000000"), "ML", new Guid("000000b2-0000-0000-0000-000000000000") },
                    { new Guid("000000e3-0000-0000-0000-000000000000"), "Agile", new Guid("000000bb-0000-0000-0000-000000000000") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicDegree_CareerHistoryId",
                table: "AcademicDegree",
                column: "CareerHistoryId");

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
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_CareerHistory_ProfileId",
                table: "CareerHistory",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_ContactTypeId",
                table: "ContactInfo",
                column: "ContactTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactInfo_ProfileHeaderId",
                table: "ContactInfo",
                column: "ProfileHeaderId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactTypeAliases_ContactTypeId",
                table: "ContactTypeAliases",
                column: "ContactTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactTypes_Name",
                table: "ContactTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileHeaders_ProfileId",
                table: "ProfileHeaders",
                column: "ProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ProfileId",
                table: "Projects",
                column: "ProfileId");

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
                name: "IX_Talents_ProfileId",
                table: "Talents",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Testimonials_ProfileId",
                table: "Testimonials",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_User_ApplicationIdentityUserId",
                table: "User",
                column: "ApplicationIdentityUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VanityUrls_ProfileId",
                table: "VanityUrls",
                column: "ProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_VanityUrls_ProfileSlugUrl",
                table: "VanityUrls",
                column: "ProfileSlugUrl",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkExperiences_CareerHistoryId",
                table: "WorkExperiences",
                column: "CareerHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkHighlight_WorkExperienceId",
                table: "WorkHighlight",
                column: "WorkExperienceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicDegree");

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
                name: "ContactInfo");

            migrationBuilder.DropTable(
                name: "ContactTypeAliases");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "SkillTypeAliases");

            migrationBuilder.DropTable(
                name: "Talents");

            migrationBuilder.DropTable(
                name: "Testimonials");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "VanityUrls");

            migrationBuilder.DropTable(
                name: "WorkHighlight");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "ProfileHeaders");

            migrationBuilder.DropTable(
                name: "ContactTypes");

            migrationBuilder.DropTable(
                name: "SkillSets");

            migrationBuilder.DropTable(
                name: "SkillTypes");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "WorkExperiences");

            migrationBuilder.DropTable(
                name: "SkillCategories");

            migrationBuilder.DropTable(
                name: "CareerHistory");

            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
