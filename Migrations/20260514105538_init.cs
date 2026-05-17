using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyPortfolio.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
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
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
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
                name: "ContactLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Label = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Url = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactLinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Experiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    JobTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EmploymentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CompanyLogoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experiences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ShortSummary = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    LongSummary = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    ProfileImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CvFilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Location = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    YearsOfExperience = table.Column<int>(type: "int", nullable: false),
                    IsAvailableForWork = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: false),
                    ShortDescription = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FullDescription = table.Column<string>(type: "nvarchar(max)", maxLength: 8000, nullable: true),
                    ProblemStatement = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    SolutionOverview = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    TechnicalHighlights = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    MainImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    DemoUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    GitHubUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsFeatured = table.Column<bool>(type: "bit", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SiteSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SiteTitle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MetaDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LogoPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    FaviconPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    PrimaryColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SecondaryColor = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    EnableDarkMode = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SkillCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SkillCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Visitors",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitorKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FirstVisitAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastVisitAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    FirstIpAddressHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastIpAddressHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Browser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    OperatingSystem = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    DeviceType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Visitors", x => x.Id);
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
                name: "ExperienceResponsibilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExperienceId = table.Column<int>(type: "int", nullable: false),
                    Text = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExperienceResponsibilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExperienceResponsibilities_Experiences_ExperienceId",
                        column: x => x.ExperienceId,
                        principalTable: "Experiences",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectImages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Caption = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectImages_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SkillCategoryId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IconPath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Level = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skills_SkillCategories_SkillCategoryId",
                        column: x => x.SkillCategoryId,
                        principalTable: "SkillCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitorSessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitorId = table.Column<long>(type: "bigint", nullable: false),
                    SessionKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    LastActivityAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    IpAddressHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Referrer = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VisitorSessions_Visitors_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectSkills",
                columns: table => new
                {
                    ProjectId = table.Column<int>(type: "int", nullable: false),
                    SkillId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectSkills", x => new { x.ProjectId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_ProjectSkills_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ClickEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitorId = table.Column<long>(type: "bigint", nullable: false),
                    VisitorSessionId = table.Column<long>(type: "bigint", nullable: false),
                    EventType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ComponentName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TargetType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TargetId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TargetText = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    PagePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    MetadataJson = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClickEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClickEvents_VisitorSessions_VisitorSessionId",
                        column: x => x.VisitorSessionId,
                        principalTable: "VisitorSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClickEvents_Visitors_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PageViews",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VisitorId = table.Column<long>(type: "bigint", nullable: false),
                    VisitorSessionId = table.Column<long>(type: "bigint", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FullUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PageTitle = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    Referrer = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IpAddressHash = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageViews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageViews_VisitorSessions_VisitorSessionId",
                        column: x => x.VisitorSessionId,
                        principalTable: "VisitorSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PageViews_Visitors_VisitorId",
                        column: x => x.VisitorId,
                        principalTable: "Visitors",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "ContactLinks",
                columns: new[] { "Id", "DisplayOrder", "Icon", "IsPublished", "Label", "Type", "Url", "Value" },
                values: new object[,]
                {
                    { 1, 1, "bi bi-envelope", true, "Email", "Email", "mailto:mohammedyonis1997@gmail.com", "mohammedyonis1997@gmail.com" },
                    { 2, 2, "bi bi-whatsapp", true, "WhatsApp", "WhatsApp", "https://wa.me/201141704109", "+201141704109" },
                    { 3, 3, "bi bi-linkedin", true, "LinkedIn", "LinkedIn", "https://www.linkedin.com/in/mohamed-younes-85474013b/", "LinkedIn" },
                    { 4, 4, "bi bi-github", true, "GitHub", "GitHub", "https://github.com/m7mdYounes", "GitHub" }
                });

            migrationBuilder.InsertData(
                table: "Profiles",
                columns: new[] { "Id", "CreatedAt", "CvFilePath", "FullName", "IsAvailableForWork", "Location", "LongSummary", "ProfileImagePath", "ShortSummary", "Title", "UpdatedAt", "YearsOfExperience" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Mohamed Younes Krema", true, "Egypt", "Software engineer focused on backend systems, APIs, SQL Server performance, authentication, authorization, and production-grade web applications.", null, ".NET Backend Engineer building secure, scalable APIs and backend systems using ASP.NET Core, SQL Server, and clean architecture.", ".NET Backend Engineer", null, 3 });

            migrationBuilder.InsertData(
                table: "SiteSettings",
                columns: new[] { "Id", "EnableDarkMode", "FaviconPath", "LogoPath", "MetaDescription", "PrimaryColor", "SecondaryColor", "SiteTitle", "UpdatedAt" },
                values: new object[] { 1, true, null, null, "Portfolio of Mohamed Younes Krema, .NET Backend Engineer specializing in ASP.NET Core, SQL Server, APIs, and clean architecture.", "#0ea5e9", "#111827", "Mohamed Younes | .NET Backend Engineer", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "SkillCategories",
                columns: new[] { "Id", "DisplayOrder", "Name" },
                values: new object[,]
                {
                    { 1, 1, "Backend" },
                    { 2, 2, "Database" },
                    { 3, 3, "Architecture" },
                    { 4, 4, "Security" },
                    { 5, 5, "Tools" },
                    { 6, 6, "Frontend" }
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "DisplayOrder", "IconPath", "IsPublished", "Level", "Name", "SkillCategoryId" },
                values: new object[,]
                {
                    { 1, 1, null, true, "Advanced", "C#", 1 },
                    { 2, 2, null, true, "Advanced", "ASP.NET Core", 1 },
                    { 3, 3, null, true, "Advanced", "ASP.NET Core MVC", 1 },
                    { 4, 4, null, true, "Advanced", "Web API", 1 },
                    { 5, 5, null, true, "Advanced", "Entity Framework Core", 1 },
                    { 6, 1, null, true, "Advanced", "SQL Server", 2 },
                    { 7, 2, null, true, "Advanced", "Query Optimization", 2 },
                    { 8, 3, null, true, "Advanced", "Database Design", 2 },
                    { 9, 1, null, true, "Advanced", "Clean Architecture", 3 },
                    { 10, 2, null, true, "Advanced", "SOLID Principles", 3 },
                    { 11, 3, null, true, "Advanced", "Repository Pattern", 3 },
                    { 12, 1, null, true, "Advanced", "JWT", 4 },
                    { 13, 2, null, true, "Advanced", "RBAC", 4 },
                    { 14, 1, null, true, "Advanced", "Git", 5 },
                    { 15, 2, null, true, "Advanced", "Swagger", 5 },
                    { 16, 3, null, true, "Advanced", "Postman", 5 },
                    { 17, 1, null, true, "Intermediate", "HTML", 6 },
                    { 18, 2, null, true, "Intermediate", "CSS", 6 },
                    { 19, 3, null, true, "Intermediate", "Bootstrap", 6 },
                    { 20, 4, null, true, "Intermediate", "JavaScript", 6 }
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
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ClickEvents_ComponentName",
                table: "ClickEvents",
                column: "ComponentName");

            migrationBuilder.CreateIndex(
                name: "IX_ClickEvents_ComponentName_CreatedAt",
                table: "ClickEvents",
                columns: new[] { "ComponentName", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ClickEvents_CreatedAt",
                table: "ClickEvents",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_ClickEvents_EventType",
                table: "ClickEvents",
                column: "EventType");

            migrationBuilder.CreateIndex(
                name: "IX_ClickEvents_EventType_CreatedAt",
                table: "ClickEvents",
                columns: new[] { "EventType", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ClickEvents_PagePath",
                table: "ClickEvents",
                column: "PagePath");

            migrationBuilder.CreateIndex(
                name: "IX_ClickEvents_PagePath_CreatedAt",
                table: "ClickEvents",
                columns: new[] { "PagePath", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_ClickEvents_VisitorId",
                table: "ClickEvents",
                column: "VisitorId");

            migrationBuilder.CreateIndex(
                name: "IX_ClickEvents_VisitorSessionId",
                table: "ClickEvents",
                column: "VisitorSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ContactLinks_DisplayOrder",
                table: "ContactLinks",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_ContactLinks_IsPublished",
                table: "ContactLinks",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_ContactLinks_IsPublished_DisplayOrder",
                table: "ContactLinks",
                columns: new[] { "IsPublished", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactLinks_Type",
                table: "ContactLinks",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceResponsibilities_ExperienceId",
                table: "ExperienceResponsibilities",
                column: "ExperienceId");

            migrationBuilder.CreateIndex(
                name: "IX_ExperienceResponsibilities_ExperienceId_DisplayOrder",
                table: "ExperienceResponsibilities",
                columns: new[] { "ExperienceId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_DisplayOrder",
                table: "Experiences",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_IsPublished",
                table: "Experiences",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Experiences_IsPublished_DisplayOrder",
                table: "Experiences",
                columns: new[] { "IsPublished", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_PageViews_CreatedAt",
                table: "PageViews",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_PageViews_Path",
                table: "PageViews",
                column: "Path");

            migrationBuilder.CreateIndex(
                name: "IX_PageViews_Path_CreatedAt",
                table: "PageViews",
                columns: new[] { "Path", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PageViews_VisitorId",
                table: "PageViews",
                column: "VisitorId");

            migrationBuilder.CreateIndex(
                name: "IX_PageViews_VisitorId_CreatedAt",
                table: "PageViews",
                columns: new[] { "VisitorId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_PageViews_VisitorSessionId",
                table: "PageViews",
                column: "VisitorSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_FullName",
                table: "Profiles",
                column: "FullName");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectImages_ProjectId",
                table: "ProjectImages",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectImages_ProjectId_DisplayOrder",
                table: "ProjectImages",
                columns: new[] { "ProjectId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_DisplayOrder",
                table: "Projects",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_IsFeatured",
                table: "Projects",
                column: "IsFeatured");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_IsPublished",
                table: "Projects",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_IsPublished_DisplayOrder",
                table: "Projects",
                columns: new[] { "IsPublished", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_IsPublished_IsFeatured_DisplayOrder",
                table: "Projects",
                columns: new[] { "IsPublished", "IsFeatured", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Slug",
                table: "Projects",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSkills_ProjectId",
                table: "ProjectSkills",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectSkills_SkillId",
                table: "ProjectSkills",
                column: "SkillId");

            migrationBuilder.CreateIndex(
                name: "IX_SkillCategories_DisplayOrder",
                table: "SkillCategories",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_SkillCategories_Name",
                table: "SkillCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Skills_DisplayOrder",
                table: "Skills",
                column: "DisplayOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_IsPublished",
                table: "Skills",
                column: "IsPublished");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SkillCategoryId",
                table: "Skills",
                column: "SkillCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SkillCategoryId_IsPublished_DisplayOrder",
                table: "Skills",
                columns: new[] { "SkillCategoryId", "IsPublished", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SkillCategoryId_Name",
                table: "Skills",
                columns: new[] { "SkillCategoryId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_FirstVisitAt",
                table: "Visitors",
                column: "FirstVisitAt");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_LastVisitAt",
                table: "Visitors",
                column: "LastVisitAt");

            migrationBuilder.CreateIndex(
                name: "IX_Visitors_VisitorKey",
                table: "Visitors",
                column: "VisitorKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitorSessions_LastActivityAt",
                table: "VisitorSessions",
                column: "LastActivityAt");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorSessions_SessionKey",
                table: "VisitorSessions",
                column: "SessionKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VisitorSessions_StartedAt",
                table: "VisitorSessions",
                column: "StartedAt");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorSessions_VisitorId",
                table: "VisitorSessions",
                column: "VisitorId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorSessions_VisitorId_LastActivityAt",
                table: "VisitorSessions",
                columns: new[] { "VisitorId", "LastActivityAt" });
        }

        /// <inheritdoc />
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
                name: "ClickEvents");

            migrationBuilder.DropTable(
                name: "ContactLinks");

            migrationBuilder.DropTable(
                name: "ExperienceResponsibilities");

            migrationBuilder.DropTable(
                name: "PageViews");

            migrationBuilder.DropTable(
                name: "Profiles");

            migrationBuilder.DropTable(
                name: "ProjectImages");

            migrationBuilder.DropTable(
                name: "ProjectSkills");

            migrationBuilder.DropTable(
                name: "SiteSettings");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Experiences");

            migrationBuilder.DropTable(
                name: "VisitorSessions");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Visitors");

            migrationBuilder.DropTable(
                name: "SkillCategories");
        }
    }
}
