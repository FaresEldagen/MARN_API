using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class InitialSchemaWithPropertyFeedback : Migration
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
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Language = table.Column<int>(type: "int", nullable: false),
                    ProfileImage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<int>(type: "int", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrontIdPhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackIdPhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArabicAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ArabicFullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NationalIDNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountStatus = table.Column<int>(type: "int", nullable: false),
                    StatusBeforeBan = table.Column<int>(type: "int", nullable: true),
                    StripeAccountId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StripePayoutsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    StripeChargesEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImagesDeletionJob = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "UserDevices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FcmToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDevices", x => x.Id);
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
                name: "AdminAnalyticsReports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GeneratedByAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Scope = table.Column<int>(type: "int", nullable: false),
                    Format = table.Column<int>(type: "int", nullable: false),
                    RequestedPeriod = table.Column<int>(type: "int", nullable: false),
                    FromUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ToUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Grouping = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    StoredFilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ContentType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FileSizeBytes = table.Column<long>(type: "bigint", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminAnalyticsReports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminAnalyticsReports_AspNetUsers_GeneratedByAdminId",
                        column: x => x.GeneratedByAdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "assistantSessions",
                columns: table => new
                {
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assistantSessions", x => x.SessionId);
                    table.ForeignKey(
                        name: "FK_assistantSessions_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    SentAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsHiddenByModeration = table.Column<bool>(type: "bit", nullable: false),
                    HiddenAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HiddenByAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HiddenReason = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    SenderId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReceiverId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_AspNetUsers_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    TitleKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BodyKey = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    LocalizationArgumentsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionType = table.Column<int>(type: "int", nullable: true),
                    ActionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ProofOfOwnership = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MaxOccupants = table.Column<int>(type: "int", nullable: false),
                    IsShared = table.Column<bool>(type: "bit", nullable: false),
                    Bedrooms = table.Column<int>(type: "int", nullable: false),
                    Beds = table.Column<int>(type: "int", nullable: false),
                    Bathrooms = table.Column<int>(type: "int", nullable: false),
                    SquareMeters = table.Column<double>(type: "float", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RentalUnit = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ImagesDeletionJob = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Properties_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReporterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReviewerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ReportableType = table.Column<int>(type: "int", nullable: false),
                    ReportableId = table.Column<long>(type: "bigint", nullable: true),
                    ReportableGuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ReviewerNote = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    ActionTaken = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ReviewedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_ReporterId",
                        column: x => x.ReporterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reports_AspNetUsers_ReviewerId",
                        column: x => x.ReviewerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RoommatePreferences",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoommatePreferencesEnabled = table.Column<bool>(type: "bit", nullable: false),
                    Governorate = table.Column<int>(type: "int", nullable: false),
                    SearchStatus = table.Column<int>(type: "int", nullable: false),
                    Smoking = table.Column<bool>(type: "bit", nullable: true),
                    SmokingImportance = table.Column<int>(type: "int", nullable: false),
                    Pets = table.Column<bool>(type: "bit", nullable: true),
                    PetsImportance = table.Column<int>(type: "int", nullable: false),
                    SleepSchedule = table.Column<int>(type: "int", nullable: false),
                    SleepImportance = table.Column<int>(type: "int", nullable: false),
                    EducationLevel = table.Column<int>(type: "int", nullable: false),
                    EducationImportance = table.Column<int>(type: "int", nullable: false),
                    FieldOfStudy = table.Column<int>(type: "int", nullable: false),
                    FieldOfStudyImportance = table.Column<int>(type: "int", nullable: false),
                    NoiseTolerance = table.Column<int>(type: "int", nullable: true),
                    NoiseToleranceImportance = table.Column<int>(type: "int", nullable: false),
                    GuestsFrequency = table.Column<int>(type: "int", nullable: false),
                    GuestsFrequencyImportance = table.Column<int>(type: "int", nullable: false),
                    WorkSchedule = table.Column<int>(type: "int", nullable: false),
                    WorkScheduleImportance = table.Column<int>(type: "int", nullable: false),
                    SharingLevel = table.Column<int>(type: "int", nullable: false),
                    SharingLevelImportance = table.Column<int>(type: "int", nullable: false),
                    BudgetRangeMin = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BudgetRangeMax = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BudgetImportance = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoommatePreferences", x => x.Id);
                    table.CheckConstraint("CK_RoommatePreference_Budget", "[BudgetRangeMax] IS NULL OR [BudgetRangeMin] IS NULL OR [BudgetRangeMax] >= [BudgetRangeMin]");
                    table.CheckConstraint("CK_RoommatePreference_ImportanceRanges", "[SmokingImportance] BETWEEN 1 AND 5 AND [PetsImportance] BETWEEN 1 AND 5 AND [SleepImportance] BETWEEN 1 AND 5 AND [EducationImportance] BETWEEN 1 AND 5 AND [FieldOfStudyImportance] BETWEEN 1 AND 5 AND [NoiseToleranceImportance] BETWEEN 1 AND 5 AND [GuestsFrequencyImportance] BETWEEN 1 AND 5 AND [WorkScheduleImportance] BETWEEN 1 AND 5 AND [SharingLevelImportance] BETWEEN 1 AND 5 AND [BudgetImportance] BETWEEN 1 AND 5");
                    table.CheckConstraint("CK_RoommatePreference_NoiseTolerance", "[NoiseTolerance] IS NULL OR [NoiseTolerance] BETWEEN 1 AND 5");
                    table.ForeignKey(
                        name: "FK_RoommatePreferences_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserActivities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropertyId = table.Column<long>(type: "bigint", nullable: true),
                    UserActivityType = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserActivities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserActivities_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "assistantMessages",
                columns: table => new
                {
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ToolOnly = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImagePathsJson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_assistantMessages", x => x.MessageId);
                    table.CheckConstraint("CK_assistantMessages_Role", "[Role] IN ('user', 'assistant', 'tool')");
                    table.ForeignKey(
                        name: "FK_assistantMessages_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_assistantMessages_assistantSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "assistantSessions",
                        principalColumn: "SessionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BookingRequests",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    RenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentFrequency = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BookingRequests", x => x.Id);
                    table.CheckConstraint("CK_BookingRequest_Dates", "[EndDate] > [StartDate]");
                    table.ForeignKey(
                        name: "FK_BookingRequests_AspNetUsers_RenterId",
                        column: x => x.RenterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BookingRequests_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Contracts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    RenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    PaymentFrequency = table.Column<int>(type: "int", nullable: false),
                    TotalContractAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LeaseStartDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LeaseEndDate = table.Column<DateOnly>(type: "date", nullable: false),
                    SignedByRenterAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Hash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    OtsFilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MerkleRoot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnchoringStatus = table.Column<int>(type: "int", nullable: false),
                    AnchoredAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contracts", x => x.Id);
                    table.CheckConstraint("CK_Contract_Dates", "[LeaseEndDate] IS NULL OR [LeaseStartDate] IS NULL OR [LeaseEndDate] > [LeaseStartDate]");
                    table.ForeignKey(
                        name: "FK_Contracts_AspNetUsers_RenterId",
                        column: x => x.RenterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contracts_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PropertyAmenities",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    Amenity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyAmenities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyAmenities_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyFeedbacks",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsHiddenByModeration = table.Column<bool>(type: "bit", nullable: false),
                    HiddenAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HiddenByAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HiddenReason = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyFeedbacks", x => x.Id);
                    table.CheckConstraint("CK_PropertyFeedback_Rating", "[Rating] >= 1 AND [Rating] <= 5");
                    table.ForeignKey(
                        name: "FK_PropertyFeedbacks_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertyFeedbacks_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyMedia",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyMedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyMedia_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyRules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    Rule = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyRules_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SavedProperties",
                columns: table => new
                {
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SavedProperties", x => new { x.PropertyId, x.UserId });
                    table.ForeignKey(
                        name: "FK_SavedProperties_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SavedProperties_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminActionLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportId = table.Column<long>(type: "bigint", nullable: true),
                    ActionType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TargetType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TargetGuidId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TargetLongId = table.Column<long>(type: "bigint", nullable: true),
                    Reason = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    MetadataJson = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminActionLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminActionLogs_AspNetUsers_AdminId",
                        column: x => x.AdminId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AdminActionLogs_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "PaymentSchedules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<long>(type: "bigint", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaymentIntentId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentSchedules_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentScheduleId = table.Column<long>(type: "bigint", nullable: false),
                    AmountTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PlatformFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OwnerAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PaymentIntentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AvailableAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Payments_PaymentSchedules_PaymentScheduleId",
                        column: x => x.PaymentScheduleId,
                        principalTable: "PaymentSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), null, "Renter", "RENTER" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), null, "Owner", "OWNER" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), null, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountStatus", "ArabicAddress", "ArabicFullName", "BackIdPhoto", "Bio", "ConcurrencyStamp", "Country", "CreatedAt", "DateOfBirth", "DeletedAt", "Email", "EmailConfirmed", "FirstName", "FrontIdPhoto", "Gender", "ImagesDeletionJob", "Language", "LastName", "LockoutEnabled", "LockoutEnd", "NationalIDNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "StatusBeforeBan", "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), 0, 1, "مدينة نصر، القاهرة", "خالد قيد الانتظار", "/images/idCards/pending-renter-back.jpg", "Fresh graduate looking to relocate for work in Nasr City.", "SCENARIO-PENDING-CONCURRENCY-STAMP", 1, new DateTime(2026, 5, 10, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "pending.renter@example.com", true, "Khaled", "/images/idCards/pending-renter-front.jpg", 1, null, 1, "Pending", false, null, "30001010101010", "PENDING.RENTER@EXAMPLE.COM", "PENDING.RENTER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", "+201298765430", false, "/images/profiles/pending-renter.png", "SCENARIO-PENDING-SECURITY-STAMP", null, null, false, false, false, "pending.renter@example.com" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), 0, 4, "شبرا، القاهرة", "سيد محظور", "/images/idCards/banned-renter-back.jpg", "Banned user account for terms of service violations.", "SCENARIO-BANNED-CONCURRENCY-STAMP", 1, new DateTime(2026, 3, 5, 14, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "banned.renter@example.com", true, "Sayed", "/images/idCards/banned-renter-front.jpg", 1, null, 1, "Banned", false, null, "30002020202020", "BANNED.RENTER@EXAMPLE.COM", "BANNED.RENTER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", "+201598765429", false, "/images/profiles/banned-renter.png", "SCENARIO-BANNED-SECURITY-STAMP", null, null, false, false, false, "banned.renter@example.com" },
                    { new Guid("10000000-0000-0000-0000-000000000003"), 0, 2, "العجوزة، الجيزة", "رامي محذوف", "/images/idCards/deleted-renter-back.jpg", "Inactive account deleted by the user.", "SCENARIO-DELETED-CONCURRENCY-STAMP", 1, new DateTime(2026, 2, 20, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 1, 9, 0, 0, 0, DateTimeKind.Utc), "deleted.renter@example.com", true, "Ramy", "/images/idCards/deleted-renter-front.jpg", 1, null, 1, "Deleted", false, null, "30003030303030", "DELETED.RENTER@EXAMPLE.COM", "DELETED.RENTER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", "+201098765428", false, "/images/profiles/deleted-renter.png", "SCENARIO-DELETED-SECURITY-STAMP", null, null, false, false, false, "deleted.renter@example.com" },
                    { new Guid("10000000-0000-0000-0000-000000000004"), 0, 2, "الشيخ زايد، الجيزة", "نور حديث", "/images/idCards/recent-renter-back.jpg", "Looking for a roommate in Sheikh Zayed area. Friendly and outgoing.", "SCENARIO-RECENT-CONCURRENCY-STAMP", 1, new DateTime(2026, 5, 25, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2000, 4, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, "recent.renter@example.com", true, "Nour", "/images/idCards/recent-renter-front.jpg", 2, null, 0, "Recent", false, null, "30004040404040", "RECENT.RENTER@EXAMPLE.COM", "RECENT.RENTER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", "+201198765427", false, "/images/profiles/recent-renter.png", "SCENARIO-RECENT-SECURITY-STAMP", null, null, false, false, false, "recent.renter@example.com" },
                    { new Guid("11111111-1111-1111-1111-111111111111"), 0, 2, "المعادي، القاهرة", "كريم حسن", "/images/idCards/user-cairo-mid-back.jpg", "Student at Cairo University, loves football and reading.", "SEED-RENTER-A-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "renter.a@example.com", true, "Karim", "/images/idCards/user-cairo-mid-front.jpg", 1, null, 0, "Hassan", false, null, "30101010101010", "RENTER.A@EXAMPLE.COM", "RENTER.A@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", "+201012345671", false, "/images/profiles/user-cairo-mid.png", "SEED-RENTER-A-SECURITY-STAMP", null, null, false, false, false, "renter.a@example.com" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 0, 2, "سيدي جابر، الإسكندرية", "مريم فؤاد", "/images/idCards/user-alex-low-back.jpg", "Graphic designer looking for a shared apartment in Alexandria.", "SEED-RENTER-B-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2002, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, "renter.b@example.com", true, "Mariam", "/images/idCards/user-alex-low-front.jpg", 2, null, 0, "Fouad", false, null, "30202020202020", "RENTER.B@EXAMPLE.COM", "RENTER.B@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", "+201112345672", false, "/images/profiles/user-alex-low.png", "SEED-RENTER-B-SECURITY-STAMP", null, null, false, false, false, "renter.b@example.com" },
                    { new Guid("30000000-0000-0000-0000-000000000001"), 0, 2, "الدقي، الجيزة", "منى أدمن", "/images/idCards/assistant-admin-back.jpg", "Assistant Administrator managing compliance and user verifications.", "SCENARIO-SECOND-ADMIN-CONCURRENCY-STAMP", 1, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), new DateTime(1995, 6, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, "assistant.admin@marn.com", true, "Mona", "/images/idCards/assistant-admin-front.jpg", 2, null, 0, "Admin", false, null, "29502020202020", "ASSISTANT.ADMIN@MARN.COM", "ASSISTANT.ADMIN@MARN.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", "+201198765431", true, "/images/profiles/assistant-admin.png", "SCENARIO-SECOND-ADMIN-SECURITY-STAMP", null, null, false, false, false, "assistant.admin@marn.com" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 0, 2, "الزقازيق، الشرقية", "أحمد نبيل", "/images/idCards/user-delta-multi-back.jpg", "Software engineer looking for a room in Delta region near transport.", "SEED-RENTER-C-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2003, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "renter.c@example.com", true, "Ahmed", "/images/idCards/user-delta-multi-front.jpg", 1, null, 1, "Nabil", false, null, "30303030303030", "RENTER.C@EXAMPLE.COM", "RENTER.C@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", "+201212345673", false, "/images/profiles/user-delta-multi.png", "SEED-RENTER-C-SECURITY-STAMP", null, null, false, false, false, "renter.c@example.com" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 0, 2, "التجمع الخامس، القاهرة", "محمود فهمي", "/images/idCards/owner-x-back.jpg", "Property owner offering premium apartments in Fifth Settlement, Cairo.", "SEED-OWNER-X-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1980, 10, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, "owner.x@example.com", true, "Mahmoud", "/images/idCards/owner-x-front.jpg", 1, null, 1, "Fahmy", false, null, "28010101010101", "OWNER.X@EXAMPLE.COM", "OWNER.X@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", "+201123456786", false, "/images/profiles/owner-x.png", "SEED-OWNER-X-SECURITY-STAMP", null, null, false, false, false, "owner.x@example.com" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 0, 2, "سموحة، الإسكندرية", "هبة يوسف", "/images/idCards/owner-y-back.jpg", "Real estate investor with multiple listings in Alexandria and Giza.", "SEED-OWNER-Y-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1985, 2, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "owner.y@example.com", true, "Heba", "/images/idCards/owner-y-front.jpg", 2, null, 0, "Youssef", false, null, "28502020202020", "OWNER.Y@EXAMPLE.COM", "OWNER.Y@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", "+201223456787", false, "/images/profiles/owner-y.png", "SEED-OWNER-Y-SECURITY-STAMP", null, null, false, false, false, "owner.y@example.com" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), 0, 2, "المهندسين، الجيزة", "طارق زكي", "/images/idCards/owner-z-back.jpg", "Providing high-quality rental options in Mohandessin and Dokki.", "SEED-OWNER-Z-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1970, 3, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, "owner.z@example.com", true, "Tarek", "/images/idCards/owner-z-front.jpg", 1, null, 1, "Zaki", false, null, "27003030303030", "OWNER.Z@EXAMPLE.COM", "OWNER.Z@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", "+201523456788", false, "/images/profiles/owner-z.png", "SEED-OWNER-Z-SECURITY-STAMP", null, null, false, false, false, "owner.z@example.com" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), 0, 2, "القاهرة الجديدة، القاهرة", "سارة عادل", "/images/idCards/user-family-high-back.jpg", "Marketing specialist, quiet and clean, looking for a roommate in New Cairo.", "SEED-RENTER-D-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2004, 4, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, "renter.d@example.com", true, "Sara", "/images/idCards/user-family-high-front.jpg", 2, null, 0, "Adel", false, null, "30404040404040", "RENTER.D@EXAMPLE.COM", "RENTER.D@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", "+201512345674", false, "/images/profiles/user-family-high.png", "SEED-RENTER-D-SECURITY-STAMP", null, null, false, false, false, "renter.d@example.com" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), 0, 2, "رأس البر، دمياط", "عمر سمير", "/images/idCards/user-coastal-flex-back.jpg", "Engineering student looking for a cozy place in Damietta.", "SEED-RENTER-E-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2005, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "renter.e@example.com", true, "Omar", "/images/idCards/user-coastal-flex-front.jpg", 1, null, 1, "Samir", false, null, "30505050505050", "RENTER.E@EXAMPLE.COM", "RENTER.E@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", "+201023456785", false, "/images/profiles/user-coastal-flex.png", "SEED-RENTER-E-SECURITY-STAMP", null, null, false, false, false, "renter.e@example.com" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), 0, 2, "مصر الجديدة، القاهرة", "ياسر أدمن", "/images/idCards/admin-back.jpg", "Lead System Administrator for MARN platform.", "SEED-ADMIN-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "admin@marn.com", true, "Yasser", "/images/idCards/admin-front.jpg", 1, null, 1, "Admin", false, null, "29010101010101", "ADMIN@MARN.COM", "ADMIN@MARN.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", "+201098765432", true, "/images/profiles/admin.png", "SEED-ADMIN-SECURITY-STAMP", null, null, false, false, false, "admin@marn.com" }
                });

            migrationBuilder.InsertData(
                table: "UserDevices",
                columns: new[] { "Id", "FcmToken", "LastUpdated", "UserId" },
                values: new object[] { new Guid("dddddddd-dddd-dddd-dddd-dddddddddd01"), "fcm-token-renter-a-device-1", new DateTime(2025, 3, 24, 0, 0, 0, 0, DateTimeKind.Utc), "11111111-1111-1111-1111-111111111111" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000001") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000002") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000003") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000004") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("30000000-0000-0000-0000-000000000001") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("30000000-0000-0000-0000-000000000001") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("66666666-6666-6666-6666-666666666666") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("66666666-6666-6666-6666-666666666666") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-777777777777") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("88888888-8888-8888-8888-888888888888") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("99999999-9999-9999-9999-999999999999") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("99999999-9999-9999-9999-999999999999") }
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "Content", "HiddenAt", "HiddenByAdminId", "HiddenReason", "IsHiddenByModeration", "ReadAt", "ReceiverId", "SenderId", "SentAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "XB+UQj6hKk23omCXxH8uwFxZpOCQjhe1tRbMbKMHUIKitggz1H61tTuCsIyQwnDRBEWtEIP3n24n1DyxJMAPTuWIvOprIjOmfp48oVxQa6M=", null, null, null, false, new DateTime(2025, 3, 20, 10, 30, 0, 0, DateTimeKind.Utc), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "E8jOydWqRhQPRv/E1P+cXgNPhEczTZ62c8OsZm62YoKZnffb6X6KXosOMw92CvheYLt5FO58PHhnweOYeJRQ6A==", null, null, null, false, null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2025, 3, 20, 11, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000101"), "XB+UQj6hKk23omCXxH8uwFxZpOCQjhe1tRbMbKMHUIKitggz1H61tTuCsIyQwnDRBEWtEIP3n24n1DyxJMAPTuWIvOprIjOmfp48oVxQa6M=", new DateTime(2026, 4, 13, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999999"), "Seeded moderation example for admin dashboard testing.", true, null, new Guid("44444444-4444-4444-4444-444444444444"), new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2026, 4, 12, 19, 30, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "ActionId", "ActionType", "Body", "BodyKey", "CreatedAt", "Data", "LocalizationArgumentsJson", "ReadAt", "Title", "TitleKey", "Type", "UserId", "UserType" },
                values: new object[,]
                {
                    { 6001L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Karim\"]", new DateTime(2025, 1, 1, 0, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Karim!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("11111111-1111-1111-1111-111111111111"), 0 },
                    { 6002L, null, null, "The owner of \"Dokki Modern Loft\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Dokki Modern Loft\"]", new DateTime(2023, 12, 19, 12, 0, 0, 0, DateTimeKind.Utc), "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE", 6, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6003L, null, 4, "Your payment of 22500 egp for \"Dokki Modern Loft\" has been successful.\nThis payment is for the due date 2024-03-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2024, 4, 5, 14, 0, 0, 0, DateTimeKind.Utc), null, "[\"22500\",\"egp\",\"Dokki Modern Loft\",\"2024-03-31\"]", new DateTime(2024, 4, 5, 14, 10, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6004L, null, 4, "Your payment of 22500 egp for \"Dokki Modern Loft\" has been successful.\nThis payment is for the due date 2024-06-30.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2024, 6, 30, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"22500\",\"egp\",\"Dokki Modern Loft\",\"2024-06-30\"]", new DateTime(2024, 6, 30, 11, 5, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6005L, null, 4, "Your payment of 22500 egp for \"Dokki Modern Loft\" has been successful.\nThis payment is for the due date 2024-09-30.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2024, 10, 3, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"22500\",\"egp\",\"Dokki Modern Loft\",\"2024-09-30\"]", new DateTime(2024, 10, 3, 10, 15, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6006L, null, 4, "Your payment of 22500 egp for \"Dokki Modern Loft\" has been successful.\nThis payment is for the due date 2024-12-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2024, 12, 31, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"22500\",\"egp\",\"Dokki Modern Loft\",\"2024-12-31\"]", new DateTime(2024, 12, 31, 9, 12, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6007L, "44444444-4444-4444-4444-444444444444", 2, "You have a new message from Mahmoud Fahmy", "NOTIFICATION_NEW_MESSAGE_BODY", new DateTime(2025, 3, 20, 11, 0, 0, 0, DateTimeKind.Utc), "{\"SenderId\":\"44444444-4444-4444-4444-444444444444\",\"SenderName\":\"Mahmoud Fahmy\",\"Content\":\"Hello Karim! Welcome to the property.\"}", "[\"Mahmoud Fahmy\"]", new DateTime(2025, 3, 20, 11, 5, 0, 0, DateTimeKind.Utc), "New Message", "NOTIFICATION_NEW_MESSAGE_TITLE", 1, new Guid("11111111-1111-1111-1111-111111111111"), 0 },
                    { 6008L, null, null, "The owner of \"Agouza Shared House\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2025, 5, 24, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Agouza Shared House\"]", new DateTime(2025, 5, 24, 10, 0, 0, 0, DateTimeKind.Utc), "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE", 6, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6009L, null, null, "The owner of \"Zamalek Riverside Apartment\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2025, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Zamalek Riverside Apartment\"]", new DateTime(2025, 12, 27, 8, 0, 0, 0, DateTimeKind.Utc), "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE", 6, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6010L, null, 4, "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" has been successful.\nThis payment is for the due date 2026-01-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 1, 29, 12, 0, 0, 0, DateTimeKind.Utc), null, "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-01-31\"]", new DateTime(2026, 1, 29, 12, 10, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6011L, null, 4, "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" has been successful.\nThis payment is for the due date 2026-02-28.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 2, 28, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-02-28\"]", new DateTime(2026, 2, 28, 10, 5, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6012L, null, 4, "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" has been successful.\nThis payment is for the due date 2026-03-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 4, 5, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-03-31\"]", new DateTime(2026, 4, 5, 9, 15, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6013L, null, 4, "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" has been successful.\nThis payment is for the due date 2026-04-30.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 5, 5, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-04-30\"]", new DateTime(2026, 5, 5, 10, 8, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6014L, null, 4, "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" has been successful.\nThis payment is for the due date 2026-05-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 6, 5, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-05-31\"]", new DateTime(2026, 6, 5, 9, 12, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6015L, null, null, "Your payment of 5000 egp for \"Zamalek Riverside Apartment\" is now available and can be paid.\n7 day(s) left until the due date 2026-06-30.", "NOTIFICATION_UPCOMING_PAYMENT_BODY", new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"5000\",\"egp\",\"Zamalek Riverside Apartment\",\"7\",\"2026-06-30\"]", null, "Upcoming Payment Available", "NOTIFICATION_UPCOMING_PAYMENT_TITLE", 10, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6101L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mariam\"]", new DateTime(2025, 1, 2, 0, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Mariam!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("22222222-2222-2222-2222-222222222222"), 0 },
                    { 6102L, null, null, "The owner of \"Sheikh Zayed Luxury Villa\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2026, 4, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Sheikh Zayed Luxury Villa\"]", new DateTime(2026, 4, 20, 10, 0, 0, 0, DateTimeKind.Utc), "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE", 6, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6103L, null, 4, "Your payment of 15000 egp for \"Sheikh Zayed Luxury Villa\" has been successful.\nThis payment is for the due date 2025-05-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2025, 5, 31, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"15000\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-05-31\"]", new DateTime(2025, 5, 31, 10, 10, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6104L, null, 4, "Your payment of 15000 egp for \"Sheikh Zayed Luxury Villa\" has been successful.\nThis payment is for the due date 2025-06-30.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2025, 6, 30, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"15000\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-06-30\"]", new DateTime(2025, 6, 30, 11, 15, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6105L, null, 4, "Your payment of 15000 egp for \"Sheikh Zayed Luxury Villa\" has been successful.\nThis payment is for the due date 2025-07-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2025, 8, 4, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"15000\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-07-31\"]", new DateTime(2025, 8, 4, 9, 20, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6106L, null, 4, "Your payment of 15000 egp for \"Sheikh Zayed Luxury Villa\" has been successful.\nThis payment is for the due date 2025-08-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2025, 8, 29, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"15000\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-08-31\"]", new DateTime(2025, 8, 29, 10, 8, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6107L, null, null, "The owner of \"Agouza Shared House\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2026, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Agouza Shared House\"]", new DateTime(2026, 1, 27, 9, 0, 0, 0, DateTimeKind.Utc), "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE", 6, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6108L, null, 4, "Your payment of 4000 egp for \"Agouza Shared House\" has been successful.\nThis payment is for the due date 2026-02-28.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 2, 22, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"4000\",\"egp\",\"Agouza Shared House\",\"2026-02-28\"]", new DateTime(2026, 2, 22, 10, 12, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6109L, null, 4, "Your payment of 4000 egp for \"Agouza Shared House\" has been successful.\nThis payment is for the due date 2026-03-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 3, 31, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"4000\",\"egp\",\"Agouza Shared House\",\"2026-03-31\"]", new DateTime(2026, 3, 31, 11, 10, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6110L, null, 4, "Your payment of 4000 egp for \"Agouza Shared House\" has been successful.\nThis payment is for the due date 2026-04-30.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 5, 8, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"4000\",\"egp\",\"Agouza Shared House\",\"2026-04-30\"]", new DateTime(2026, 5, 8, 9, 15, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6111L, null, 4, "Your payment of 4000 egp for \"Agouza Shared House\" has been successful.\nThis payment is for the due date 2026-05-31.", "NOTIFICATION_PAYMENT_SUCCESSFUL_BODY", new DateTime(2026, 5, 31, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"4000\",\"egp\",\"Agouza Shared House\",\"2026-05-31\"]", new DateTime(2026, 5, 31, 10, 5, 0, 0, DateTimeKind.Utc), "Payment Successful", "NOTIFICATION_PAYMENT_SUCCESSFUL_TITLE", 13, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6112L, null, null, "Your payment of 4000 egp for \"Agouza Shared House\" is now available and can be paid.\n7 day(s) left until the due date 2026-06-30.", "NOTIFICATION_UPCOMING_PAYMENT_BODY", new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"4000\",\"egp\",\"Agouza Shared House\",\"7\",\"2026-06-30\"]", null, "Upcoming Payment Available", "NOTIFICATION_UPCOMING_PAYMENT_TITLE", 10, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6113L, null, null, "The owner of \"Dokki Modern Loft\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2025, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Dokki Modern Loft\"]", new DateTime(2025, 12, 28, 9, 0, 0, 0, DateTimeKind.Utc), "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE", 6, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6114L, null, 4, "An admin has cancelled contract #1000006 for \"Sheikh Zayed Luxury Villa\".", "NOTIFICATION_ADMIN_CONTRACT_CANCELLED_BODY", new DateTime(2026, 4, 26, 12, 0, 0, 0, DateTimeKind.Utc), null, "[\"1000006\",\"Sheikh Zayed Luxury Villa\"]", new DateTime(2026, 4, 26, 12, 30, 0, 0, DateTimeKind.Utc), "Contract Cancelled", "NOTIFICATION_CONTRACT_CANCELLED_TITLE", 7, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6201L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Ahmed\"]", new DateTime(2025, 1, 3, 0, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Ahmed!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("33333333-3333-3333-3333-333333333333"), 0 },
                    { 6301L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Sara\"]", new DateTime(2025, 1, 4, 0, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Sara!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("77777777-7777-7777-7777-777777777777"), 0 },
                    { 6401L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Omar\"]", new DateTime(2025, 1, 5, 0, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Omar!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("88888888-8888-8888-8888-888888888888"), 0 },
                    { 6402L, null, null, "The owner of \"Mohandeseen Studio Flat\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2025, 11, 28, 12, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mohandeseen Studio Flat\"]", new DateTime(2025, 11, 28, 13, 0, 0, 0, DateTimeKind.Utc), "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE", 6, new Guid("88888888-8888-8888-8888-888888888888"), 1 },
                    { 6501L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mahmoud\"]", new DateTime(2025, 1, 1, 0, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Mahmoud!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("44444444-4444-4444-4444-444444444444"), 0 },
                    { 6502L, null, 5, "Your Stripe Connect account has been activated and is now ready to withdraw your payments.", "NOTIFICATION_CONNECT_SUCCESS_BODY", new DateTime(2025, 1, 10, 12, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2025, 1, 10, 12, 5, 0, 0, DateTimeKind.Utc), "Connect Account Activated", "NOTIFICATION_CONNECT_SUCCESS_TITLE", 17, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6503L, "1001", 1, "Your property \"Zamalek Riverside Apartment\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Zamalek Riverside Apartment\"]", new DateTime(2024, 2, 1, 1, 0, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6504L, "1002", 1, "Your property \"Dokki Modern Loft\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2023, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Dokki Modern Loft\"]", new DateTime(2023, 2, 2, 1, 0, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6505L, "1003", 1, "Your property \"Mohandeseen Studio Flat\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mohandeseen Studio Flat\"]", new DateTime(2025, 2, 3, 1, 0, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6506L, "1100", 1, "Your property \"Agouza Shared House\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Agouza Shared House\"]", new DateTime(2024, 2, 5, 1, 0, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6507L, null, null, "You have received a new booking request for \"Dokki Modern Loft\" from Karim Hassan.", "NOTIFICATION_BOOKING_REQUEST_BODY", new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Dokki Modern Loft\",\"Karim Hassan\"]", new DateTime(2025, 4, 1, 8, 0, 0, 0, DateTimeKind.Utc), "New Booking Request", "NOTIFICATION_BOOKING_REQUEST_TITLE", 2, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6508L, null, null, "You have received a new booking request for \"Dokki Modern Loft\" from Karim Hassan.", "NOTIFICATION_BOOKING_REQUEST_BODY", new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Dokki Modern Loft\",\"Karim Hassan\"]", new DateTime(2025, 3, 10, 8, 0, 0, 0, DateTimeKind.Utc), "New Booking Request", "NOTIFICATION_BOOKING_REQUEST_TITLE", 2, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6509L, null, null, "You have received a new booking request for \"Mohandeseen Studio Flat\" from Mariam Fouad.", "NOTIFICATION_BOOKING_REQUEST_BODY", new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mohandeseen Studio Flat\",\"Mariam Fouad\"]", new DateTime(2025, 4, 2, 8, 0, 0, 0, DateTimeKind.Utc), "New Booking Request", "NOTIFICATION_BOOKING_REQUEST_TITLE", 2, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6510L, null, null, "You have received a new booking request for \"Mohandeseen Studio Flat\" from Tarek Owner.", "NOTIFICATION_BOOKING_REQUEST_BODY", new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mohandeseen Studio Flat\",\"Tarek Owner\"]", new DateTime(2025, 4, 10, 8, 0, 0, 0, DateTimeKind.Utc), "New Booking Request", "NOTIFICATION_BOOKING_REQUEST_TITLE", 2, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6511L, "1000005", 6, "The renter Karim Hassan has signed the contract for \"Dokki Modern Loft\".", "NOTIFICATION_CONTRACT_SIGNED_BODY", new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Karim Hassan\",\"Dokki Modern Loft\"]", new DateTime(2023, 12, 20, 8, 0, 0, 0, DateTimeKind.Utc), "Contract Signed", "NOTIFICATION_CONTRACT_SIGNED_TITLE", 8, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6512L, "1000003", 6, "The renter Karim Hassan has signed the contract for \"Agouza Shared House\".", "NOTIFICATION_CONTRACT_SIGNED_BODY", new DateTime(2025, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Karim Hassan\",\"Agouza Shared House\"]", new DateTime(2025, 5, 25, 8, 0, 0, 0, DateTimeKind.Utc), "Contract Signed", "NOTIFICATION_CONTRACT_SIGNED_TITLE", 8, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6513L, "1000102", 6, "The renter Omar Samir has signed the contract for \"Mohandeseen Studio Flat\".", "NOTIFICATION_CONTRACT_SIGNED_BODY", new DateTime(2025, 11, 29, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"Omar Samir\",\"Mohandeseen Studio Flat\"]", new DateTime(2025, 11, 29, 11, 0, 0, 0, DateTimeKind.Utc), "Contract Signed", "NOTIFICATION_CONTRACT_SIGNED_TITLE", 8, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6514L, "1000001", 6, "The renter Karim Hassan has signed the contract for \"Zamalek Riverside Apartment\".", "NOTIFICATION_CONTRACT_SIGNED_BODY", new DateTime(2025, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Karim Hassan\",\"Zamalek Riverside Apartment\"]", new DateTime(2025, 12, 28, 8, 0, 0, 0, DateTimeKind.Utc), "Contract Signed", "NOTIFICATION_CONTRACT_SIGNED_TITLE", 8, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6515L, "1000004", 6, "The renter Mariam Fouad has signed the contract for \"Agouza Shared House\".", "NOTIFICATION_CONTRACT_SIGNED_BODY", new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mariam Fouad\",\"Agouza Shared House\"]", new DateTime(2026, 1, 28, 8, 0, 0, 0, DateTimeKind.Utc), "Contract Signed", "NOTIFICATION_CONTRACT_SIGNED_TITLE", 8, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6516L, "1000002", 6, "The renter Mariam Fouad has signed the contract for \"Dokki Modern Loft\".", "NOTIFICATION_CONTRACT_SIGNED_BODY", new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mariam Fouad\",\"Dokki Modern Loft\"]", new DateTime(2025, 12, 29, 8, 0, 0, 0, DateTimeKind.Utc), "Contract Signed", "NOTIFICATION_CONTRACT_SIGNED_TITLE", 8, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6517L, "11111111-1111-1111-1111-111111111111", 2, "You have a new message from Karim Hassan", "NOTIFICATION_NEW_MESSAGE_BODY", new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Utc), "{\"SenderId\":\"11111111-1111-1111-1111-111111111111\",\"SenderName\":\"Karim Hassan\",\"Content\":\"Hello Mahmoud! I am interested in your property.\"}", "[\"Karim Hassan\"]", new DateTime(2025, 3, 20, 10, 5, 0, 0, DateTimeKind.Utc), "New Message", "NOTIFICATION_NEW_MESSAGE_TITLE", 1, new Guid("44444444-4444-4444-4444-444444444444"), 0 },
                    { 6518L, "10000000-0000-0000-0000-000000000002", 2, "You have a new message from Sayed Banned", "NOTIFICATION_NEW_MESSAGE_BODY", new DateTime(2026, 4, 12, 19, 30, 0, 0, DateTimeKind.Utc), "{\"SenderId\":\"10000000-0000-0000-0000-000000000002\",\"SenderName\":\"Sayed Banned\",\"Content\":\"Can you lower the rent?\"}", "[\"Sayed Banned\"]", new DateTime(2026, 4, 12, 19, 35, 0, 0, DateTimeKind.Utc), "New Message", "NOTIFICATION_NEW_MESSAGE_TITLE", 1, new Guid("44444444-4444-4444-4444-444444444444"), 0 },
                    { 6519L, null, 5, "You have received a payment of 20250 egp for \"Dokki Modern Loft\".\nThis payment is for the due date 2024-03-31.\n\nYou can withdraw this amount after 2024-04-15.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2024, 4, 5, 14, 0, 0, 0, DateTimeKind.Utc), null, "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-03-31\",\"2024-04-15\"]", new DateTime(2024, 4, 5, 14, 10, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6520L, null, null, "Your payment of 20250 egp from contract \"Dokki Modern Loft\"\nthat paid at 2024-04-05 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2024, 4, 15, 14, 0, 0, 0, DateTimeKind.Utc), null, "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-04-05\"]", new DateTime(2024, 4, 15, 14, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6521L, null, 5, "You have received a payment of 20250 egp for \"Dokki Modern Loft\".\nThis payment is for the due date 2024-06-30.\n\nYou can withdraw this amount after 2024-07-10.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2024, 6, 30, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-06-30\",\"2024-07-10\"]", new DateTime(2024, 6, 30, 11, 5, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6522L, null, null, "Your payment of 20250 egp from contract \"Dokki Modern Loft\"\nthat paid at 2024-06-30 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2024, 7, 10, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-06-30\"]", new DateTime(2024, 7, 10, 11, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6523L, null, 5, "You have received a payment of 20250 egp for \"Dokki Modern Loft\".\nThis payment is for the due date 2024-09-30.\n\nYou can withdraw this amount after 2024-10-13.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2024, 10, 3, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-09-30\",\"2024-10-13\"]", new DateTime(2024, 10, 3, 10, 15, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6524L, null, null, "Your payment of 20250 egp from contract \"Dokki Modern Loft\"\nthat paid at 2024-10-03 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2024, 10, 13, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-10-03\"]", new DateTime(2024, 10, 13, 10, 20, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6525L, null, 5, "You have received a payment of 20250 egp for \"Dokki Modern Loft\".\nThis payment is for the due date 2024-12-31.\n\nYou can withdraw this amount after 2025-01-10.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2024, 12, 31, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-12-31\",\"2025-01-10\"]", new DateTime(2024, 12, 31, 9, 12, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6526L, null, null, "Your payment of 20250 egp from contract \"Dokki Modern Loft\"\nthat paid at 2024-12-31 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2025, 1, 10, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"20250\",\"egp\",\"Dokki Modern Loft\",\"2024-12-31\"]", new DateTime(2025, 1, 10, 9, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6527L, null, 5, "You have received a payment of 4500 egp for \"Zamalek Riverside Apartment\".\nThis payment is for the due date 2026-01-31.\n\nYou can withdraw this amount after 2026-02-08.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 1, 29, 12, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-01-31\",\"2026-02-08\"]", new DateTime(2026, 1, 29, 12, 10, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6528L, null, null, "Your payment of 4500 egp from contract \"Zamalek Riverside Apartment\"\nthat paid at 2026-01-29 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 2, 8, 12, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-01-29\"]", new DateTime(2026, 2, 8, 12, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6529L, null, 5, "You have received a payment of 4500 egp for \"Zamalek Riverside Apartment\".\nThis payment is for the due date 2026-02-28.\n\nYou can withdraw this amount after 2026-03-10.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 2, 28, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-02-28\",\"2026-03-10\"]", new DateTime(2026, 2, 28, 10, 5, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6530L, null, null, "Your payment of 4500 egp from contract \"Zamalek Riverside Apartment\"\nthat paid at 2026-02-28 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 3, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-02-28\"]", new DateTime(2026, 3, 10, 10, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6531L, null, 5, "You have received a payment of 4500 egp for \"Zamalek Riverside Apartment\".\nThis payment is for the due date 2026-03-31.\n\nYou can withdraw this amount after 2026-04-15.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 4, 5, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-03-31\",\"2026-04-15\"]", new DateTime(2026, 4, 5, 9, 15, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6532L, null, null, "Your payment of 4500 egp from contract \"Zamalek Riverside Apartment\"\nthat paid at 2026-04-05 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 4, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-04-05\"]", new DateTime(2026, 4, 15, 9, 20, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6533L, null, 5, "You have received a payment of 4500 egp for \"Zamalek Riverside Apartment\".\nThis payment is for the due date 2026-04-30.\n\nYou can withdraw this amount after 2026-05-15.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 5, 5, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-04-30\",\"2026-05-15\"]", new DateTime(2026, 5, 5, 10, 8, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6534L, null, null, "Your payment of 4500 egp from contract \"Zamalek Riverside Apartment\"\nthat paid at 2026-05-05 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 5, 15, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-05-05\"]", new DateTime(2026, 5, 15, 10, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6535L, null, 5, "You have received a payment of 4500 egp for \"Zamalek Riverside Apartment\".\nThis payment is for the due date 2026-05-31.\n\nYou can withdraw this amount after 2026-06-15.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 6, 5, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-05-31\",\"2026-06-15\"]", new DateTime(2026, 6, 5, 9, 12, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6536L, null, null, "Your payment of 4500 egp from contract \"Zamalek Riverside Apartment\"\nthat paid at 2026-06-05 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 6, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"4500\",\"egp\",\"Zamalek Riverside Apartment\",\"2026-06-05\"]", new DateTime(2026, 6, 15, 9, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6537L, null, 5, "You have received a payment of 3600 egp for \"Agouza Shared House\".\nThis payment is for the due date 2026-02-28.\n\nYou can withdraw this amount after 2026-03-04.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 2, 22, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-02-28\",\"2026-03-04\"]", new DateTime(2026, 2, 22, 10, 12, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6538L, null, null, "Your payment of 3600 egp from contract \"Agouza Shared House\"\nthat paid at 2026-02-22 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 3, 4, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-02-22\"]", new DateTime(2026, 3, 4, 10, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6539L, null, 5, "You have received a payment of 3600 egp for \"Agouza Shared House\".\nThis payment is for the due date 2026-03-31.\n\nYou can withdraw this amount after 2026-04-10.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 3, 31, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-03-31\",\"2026-04-10\"]", new DateTime(2026, 3, 31, 11, 10, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6540L, null, null, "Your payment of 3600 egp from contract \"Agouza Shared House\"\nthat paid at 2026-03-31 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 4, 10, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-03-31\"]", new DateTime(2026, 4, 10, 11, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6541L, null, 5, "You have received a payment of 3600 egp for \"Agouza Shared House\".\nThis payment is for the due date 2026-04-30.\n\nYou can withdraw this amount after 2026-05-18.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 5, 8, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-04-30\",\"2026-05-18\"]", new DateTime(2026, 5, 8, 9, 15, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6542L, null, null, "Your payment of 3600 egp from contract \"Agouza Shared House\"\nthat paid at 2026-05-08 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 5, 18, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-05-08\"]", new DateTime(2026, 5, 18, 9, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6543L, null, 5, "You have received a payment of 3600 egp for \"Agouza Shared House\".\nThis payment is for the due date 2026-05-31.\n\nYou can withdraw this amount after 2026-06-10.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2026, 5, 31, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-05-31\",\"2026-06-10\"]", new DateTime(2026, 5, 31, 10, 5, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6544L, null, null, "Your payment of 3600 egp from contract \"Agouza Shared House\"\nthat paid at 2026-05-31 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2026, 6, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"3600\",\"egp\",\"Agouza Shared House\",\"2026-05-31\"]", new DateTime(2026, 6, 10, 10, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6601L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Sherif\"]", new DateTime(2025, 1, 2, 0, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Sherif!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("55555555-5555-5555-5555-555555555555"), 0 },
                    { 6602L, "1201", 1, "Your property \"Pending Downtown Apartment\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2026, 5, 3, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"Pending Downtown Apartment\"]", new DateTime(2026, 5, 3, 9, 30, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { 6603L, "1202", 1, "Your property \"Declined Garden House\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Utc), null, "[\"Declined Garden House\"]", new DateTime(2026, 4, 18, 13, 0, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { 6604L, "1203", 1, "Your property \"Soft Deleted Test Studio\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2026, 3, 8, 16, 0, 0, 0, DateTimeKind.Utc), null, "[\"Soft Deleted Test Studio\"]", new DateTime(2026, 3, 8, 17, 0, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { 6605L, "1203", 1, "Your property \"Soft Deleted Test Studio\" has been deleted.", "NOTIFICATION_PROPERTY_DELETED_BODY", new DateTime(2026, 4, 4, 13, 0, 0, 0, DateTimeKind.Utc), null, "[\"Soft Deleted Test Studio\"]", new DateTime(2026, 4, 4, 13, 10, 0, 0, DateTimeKind.Utc), "Property Deleted", "NOTIFICATION_PROPERTY_DELETED_TITLE", 23, new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { 6606L, "1204", 1, "Your property \"Recent Marina Flat\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2026, 5, 5, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"Recent Marina Flat\"]", new DateTime(2026, 5, 5, 10, 30, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { 6607L, "1205", 1, "Your property \"Moderated Riverside Villa\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2026, 5, 7, 15, 0, 0, 0, DateTimeKind.Utc), null, "[\"Moderated Riverside Villa\"]", new DateTime(2026, 5, 7, 15, 30, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { 6608L, "1000103", 6, "The renter Sayed Banned has signed the contract for \"Moderated Riverside Villa\".", "NOTIFICATION_CONTRACT_SIGNED_BODY", new DateTime(2026, 5, 21, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"Sayed Banned\",\"Moderated Riverside Villa\"]", new DateTime(2026, 5, 21, 10, 30, 0, 0, DateTimeKind.Utc), "Contract Signed", "NOTIFICATION_CONTRACT_SIGNED_TITLE", 8, new Guid("55555555-5555-5555-5555-555555555555"), 2 },
                    { 6701L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Tarek\"]", new DateTime(2025, 1, 3, 0, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Tarek!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("66666666-6666-6666-6666-666666666666"), 0 },
                    { 6702L, null, 5, "Your Stripe Connect account has been activated and is now ready to withdraw your payments.", "NOTIFICATION_CONNECT_SUCCESS_BODY", new DateTime(2025, 2, 10, 12, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2025, 2, 10, 12, 5, 0, 0, DateTimeKind.Utc), "Connect Account Activated", "NOTIFICATION_CONNECT_SUCCESS_TITLE", 17, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6703L, "1004", 1, "Your property \"Sheikh Zayed Luxury Villa\" has been submitted successfully and is now pending admin verification. This process may take up to 24 hours. We'll notify you once it's approved.", "NOTIFICATION_PROPERTY_SUBMITTED_BODY", new DateTime(2025, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Sheikh Zayed Luxury Villa\"]", new DateTime(2025, 2, 4, 1, 0, 0, 0, DateTimeKind.Utc), "Property Submitted for Review", "NOTIFICATION_PROPERTY_SUBMITTED_TITLE", 21, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6704L, "1000006", 6, "The renter Mariam Fouad has signed the contract for \"Sheikh Zayed Luxury Villa\".", "NOTIFICATION_CONTRACT_SIGNED_BODY", new DateTime(2026, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"Mariam Fouad\",\"Sheikh Zayed Luxury Villa\"]", new DateTime(2026, 4, 25, 8, 0, 0, 0, DateTimeKind.Utc), "Contract Signed", "NOTIFICATION_CONTRACT_SIGNED_TITLE", 8, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6705L, null, 5, "An admin has cancelled contract #1000006 for \"Sheikh Zayed Luxury Villa\".", "NOTIFICATION_ADMIN_CONTRACT_CANCELLED_BODY", new DateTime(2026, 4, 26, 12, 0, 0, 0, DateTimeKind.Utc), null, "[\"1000006\",\"Sheikh Zayed Luxury Villa\"]", new DateTime(2026, 4, 26, 12, 30, 0, 0, DateTimeKind.Utc), "Contract Cancelled", "NOTIFICATION_CONTRACT_CANCELLED_TITLE", 7, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6706L, null, 5, "You have received a payment of 13500 egp for \"Sheikh Zayed Luxury Villa\".\nThis payment is for the due date 2025-05-31.\n\nYou can withdraw this amount after 2025-06-10.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2025, 5, 31, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-05-31\",\"2025-06-10\"]", new DateTime(2025, 5, 31, 10, 10, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6707L, null, null, "Your payment of 13500 egp from contract \"Sheikh Zayed Luxury Villa\"\nthat paid at 2025-05-31 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2025, 6, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-05-31\"]", new DateTime(2025, 6, 10, 10, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6708L, null, 5, "You have received a payment of 13500 egp for \"Sheikh Zayed Luxury Villa\".\nThis payment is for the due date 2025-06-30.\n\nYou can withdraw this amount after 2025-07-10.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2025, 6, 30, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-06-30\",\"2025-07-10\"]", new DateTime(2025, 6, 30, 11, 15, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6709L, null, null, "Your payment of 13500 egp from contract \"Sheikh Zayed Luxury Villa\"\nthat paid at 2025-06-30 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2025, 7, 10, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-06-30\"]", new DateTime(2025, 7, 10, 11, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6710L, null, 5, "You have received a payment of 13500 egp for \"Sheikh Zayed Luxury Villa\".\nThis payment is for the due date 2025-07-31.\n\nYou can withdraw this amount after 2025-08-14.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2025, 8, 4, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-07-31\",\"2025-08-14\"]", new DateTime(2025, 8, 4, 9, 20, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6711L, null, null, "Your payment of 13500 egp from contract \"Sheikh Zayed Luxury Villa\"\nthat paid at 2025-08-04 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2025, 8, 14, 9, 0, 0, 0, DateTimeKind.Utc), null, "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-08-04\"]", new DateTime(2025, 8, 14, 9, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6712L, null, 5, "You have received a payment of 13500 egp for \"Sheikh Zayed Luxury Villa\".\nThis payment is for the due date 2025-08-31.\n\nYou can withdraw this amount after 2025-09-08.", "NOTIFICATION_PAYMENT_RECEIVED_BODY", new DateTime(2025, 8, 29, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-08-31\",\"2025-09-08\"]", new DateTime(2025, 8, 29, 10, 8, 0, 0, DateTimeKind.Utc), "Payment Received", "NOTIFICATION_PAYMENT_RECEIVED_TITLE", 15, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6713L, null, null, "Your payment of 13500 egp from contract \"Sheikh Zayed Luxury Villa\"\nthat paid at 2025-08-29 is now available for withdrawal.", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_BODY", new DateTime(2025, 9, 8, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"13500\",\"egp\",\"Sheikh Zayed Luxury Villa\",\"2025-08-29\"]", new DateTime(2025, 9, 8, 10, 15, 0, 0, DateTimeKind.Utc), "Payment Available for Withdrawal", "NOTIFICATION_PAYMENT_AVAILABLE_FOR_WITHDRAWAL_TITLE", 16, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6801L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2026, 5, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"Khaled\"]", new DateTime(2026, 5, 10, 10, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Khaled!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("10000000-0000-0000-0000-000000000001"), 0 },
                    { 6802L, null, null, "Your profile has been updated successfully. Our team will review your information, and your account is expected to be verified within approximately 24 hours.\n\nOnce verified, you’ll be able to start renting properties, listing your own, and connecting with compatible roommates.", "NOTIFICATION_PROFILE_UPDATED_BODY", new DateTime(2026, 5, 10, 10, 5, 0, 0, DateTimeKind.Utc), null, null, null, "Profile Updated Successfully!", "NOTIFICATION_PROFILE_UPDATED_TITLE", 0, new Guid("10000000-0000-0000-0000-000000000001"), 0 },
                    { 6901L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2026, 3, 5, 14, 0, 0, 0, DateTimeKind.Utc), null, "[\"Sayed\"]", new DateTime(2026, 3, 5, 14, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Sayed!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("10000000-0000-0000-0000-000000000002"), 0 },
                    { 6902L, null, null, "An admin has banned your account. You can no longer use MARN until the ban is removed. If you believe this is a mistake, please contact support.", "NOTIFICATION_ACCOUNT_BANNED_BODY", new DateTime(2026, 4, 13, 9, 1, 0, 0, DateTimeKind.Utc), null, null, null, "Account Banned", "NOTIFICATION_ACCOUNT_BANNED_TITLE", 0, new Guid("10000000-0000-0000-0000-000000000002"), 0 },
                    { 6903L, null, null, "An admin has banned your account. You can no longer use MARN until the ban is removed. If you believe this is a mistake, please contact support.", "NOTIFICATION_ACCOUNT_BANNED_BODY", new DateTime(2026, 4, 14, 12, 1, 0, 0, DateTimeKind.Utc), null, null, null, "Account Banned", "NOTIFICATION_ACCOUNT_BANNED_TITLE", 0, new Guid("10000000-0000-0000-0000-000000000002"), 0 },
                    { 6904L, null, null, "The owner of \"Moderated Riverside Villa\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2026, 5, 20, 12, 0, 0, 0, DateTimeKind.Utc), null, "[\"Moderated Riverside Villa\"]", new DateTime(2026, 5, 20, 12, 30, 0, 0, DateTimeKind.Utc), "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE", 6, new Guid("10000000-0000-0000-0000-000000000002"), 1 },
                    { 6905L, null, null, "Your payment of 5000 egp for \"Moderated Riverside Villa\" is now available and can be paid.\n7 day(s) left until the due date 2026-06-23.", "NOTIFICATION_UPCOMING_PAYMENT_BODY", new DateTime(2026, 6, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, "[\"5000\",\"egp\",\"Moderated Riverside Villa\",\"7\",\"2026-06-23\"]", null, "Upcoming Payment Available", "NOTIFICATION_UPCOMING_PAYMENT_TITLE", 10, new Guid("10000000-0000-0000-0000-000000000002"), 1 },
                    { 7001L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2026, 2, 20, 11, 0, 0, 0, DateTimeKind.Utc), null, "[\"Ramy\"]", new DateTime(2026, 2, 20, 11, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Ramy!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("10000000-0000-0000-0000-000000000003"), 0 },
                    { 7101L, null, 3, "We’re excited to have you on board! To get started, please complete your profile. This will allow you to explore rental opportunities, list your first property, and connect with suitable roommates.\n\nDon’t forget to set your roommate preferences in your profile to improve your matching experience and find the best fit for you.", "NOTIFICATION_WELCOME_BODY", new DateTime(2026, 5, 25, 16, 0, 0, 0, DateTimeKind.Utc), null, "[\"Nour\"]", new DateTime(2026, 5, 25, 16, 5, 0, 0, DateTimeKind.Utc), "Welcome to Your New Home Journey Nour!", "NOTIFICATION_WELCOME_TITLE", 0, new Guid("10000000-0000-0000-0000-000000000004"), 0 },
                    { 7102L, null, null, "The owner of \"Recent Marina Flat\" has generated a contract for you. Please review and sign it.", "NOTIFICATION_CONTRACT_READY_BODY", new DateTime(2026, 5, 26, 10, 0, 0, 0, DateTimeKind.Utc), null, "[\"Recent Marina Flat\"]", null, "Contract Ready for Signature", "NOTIFICATION_CONTRACT_READY_TITLE", 6, new Guid("10000000-0000-0000-0000-000000000004"), 1 }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Address", "Bathrooms", "Bedrooms", "Beds", "City", "CreatedAt", "DeletedAt", "Description", "ImagesDeletionJob", "IsActive", "IsShared", "Latitude", "Longitude", "MaxOccupants", "OwnerId", "Price", "ProofOfOwnership", "RentalUnit", "SquareMeters", "State", "Status", "Title", "Type", "Views", "ZipCode" },
                values: new object[,]
                {
                    { 1001L, "123 26th of July St, Zamalek, Cairo", 1, 2, 3, "Cairo", new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "A cozy seeded apartment with a wonderful Nile view in Zamalek.", null, true, false, 30.0626, 31.222999999999999, 3, new Guid("44444444-4444-4444-4444-444444444444"), 5000m, "/images/documents/property1001-POO.jpg", 1, 0.0, "CairoGovernorate", 1, "Zamalek Riverside Apartment", 0, 5, "11211" },
                    { 1002L, "45 Tahrir St, Dokki, Giza", 1, 1, 1, "Giza", new DateTime(2023, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "A modern loft in the heart of Dokki used for testing.", null, true, false, 30.038399999999999, 31.211400000000001, 2, new Guid("44444444-4444-4444-4444-444444444444"), 9000m, "/images/documents/property1002-POO.jpg", 1, 0.0, "GizaGovernorate", 1, "Dokki Modern Loft", 0, 3, "12311" },
                    { 1003L, "78 Arab League St, Mohandeseen, Giza", 1, 1, 1, "Giza", new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "A small studio property in Mohandeseen.", null, true, false, 30.055800000000001, 31.200099999999999, 1, new Guid("44444444-4444-4444-4444-444444444444"), 3500m, "/images/documents/property1003-POO.jpg", 1, 0.0, "GizaGovernorate", 1, "Mohandeseen Studio Flat", 4, 1, "12411" },
                    { 1004L, "Beverly Hills, Sheikh Zayed, Giza", 3, 4, 5, "Sheikh Zayed", new DateTime(2025, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, "A luxury villa in Sheikh Zayed owned by the dual-role Owner Z.", null, true, false, 30.052, 30.984999999999999, 6, new Guid("66666666-6666-6666-6666-666666666666"), 35000m, "/images/documents/property1004-POO.jpg", 1, 0.0, "GizaGovernorate", 1, "Sheikh Zayed Luxury Villa", 3, 12, "12588" },
                    { 1100L, "15 Nile Corniche, Agouza, Giza", 2, 3, 4, "Giza", new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "A shared house in Agouza for testing roommate matching logic.", null, true, true, 30.046800000000001, 31.213100000000001, 4, new Guid("44444444-4444-4444-4444-444444444444"), 4000m, "/images/documents/property1100-POO.jpg", 1, 0.0, "GizaGovernorate", 1, "Agouza Shared House", 1, 10, "12611" },
                    { 1201L, "10 Tahrir Square", 1, 1, 1, "Cairo", new DateTime(2026, 5, 3, 9, 0, 0, 0, DateTimeKind.Utc), null, "Ownership documents are uploaded and waiting for admin review.", null, true, false, 30.044, 31.234999999999999, 2, new Guid("55555555-5555-5555-5555-555555555555"), 320m, "/images/documents/property1201-POO.jpg", 0, 85.0, "CairoGovernorate", 0, "Pending Downtown Apartment", 0, 0, "11511" },
                    { 1202L, "88 Palm Street", 2, 3, 4, "Giza", new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Utc), null, "A property with rejected ownership documentation for verification testing.", null, true, false, 30.010999999999999, 31.207999999999998, 5, new Guid("55555555-5555-5555-5555-555555555555"), 11000m, "/images/documents/property1202-POO.jpg", 1, 180.0, "GizaGovernorate", 2, "Declined Garden House", 1, 4, "12511" },
                    { 1203L, "34 Sunset Alley", 1, 1, 1, "Alexandria", new DateTime(2026, 3, 8, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 4, 13, 0, 0, 0, DateTimeKind.Utc), "Soft deleted property used to validate include-deleted admin filters.", null, false, false, 31.199999999999999, 29.918700000000001, 1, new Guid("55555555-5555-5555-5555-555555555555"), 4300m, "/images/documents/property1203-POO.jpg", 1, 55.0, "AlexandriaGovernorate", 1, "Soft Deleted Test Studio", 4, 1, "21511" },
                    { 1204L, "5 Marina Walk", 2, 2, 2, "North Coast", new DateTime(2026, 5, 5, 10, 0, 0, 0, DateTimeKind.Utc), null, "Fresh verified property created this month for dashboard trend checks.", null, true, false, 30.899999999999999, 28.899999999999999, 3, new Guid("55555555-5555-5555-5555-555555555555"), 7800m, "/images/documents/property1204-POO.jpg", 1, 110.0, "MarsaMatruhGovernorate", 1, "Recent Marina Flat", 0, 9, "51711" },
                    { 1205L, "77 Corniche View", 3, 4, 5, "Luxor", new DateTime(2026, 5, 7, 15, 0, 0, 0, DateTimeKind.Utc), null, "Property already deactivated through a seeded moderation outcome.", null, false, false, 25.687200000000001, 32.639600000000002, 6, new Guid("55555555-5555-5555-5555-555555555555"), 16000m, "/images/documents/property1205-POO.jpg", 1, 240.0, "LuxorGovernorate", 1, "Moderated Riverside Villa", 3, 22, "85951" },
                    { 2001L, "15 Tahrir Square", 1, 2, 3, "Cairo", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "A bright apartment near downtown Cairo for monthly stays", null, true, false, 30.0444, 31.235700000000001, 3, new Guid("44444444-4444-4444-4444-444444444444"), 85000m, "/images/documents/property2001-POO.jpg", 2, 118.0, "CairoGovernorate", 1, "Nile View Apartment", 0, 14, "11511" },
                    { 2002L, "22 Al Ahram Street", 2, 3, 4, "Heliopolis", new DateTime(2026, 5, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "Comfortable family flat close to shops and transit", null, true, false, 30.091899999999999, 31.317399999999999, 4, new Guid("66666666-6666-6666-6666-666666666666"), 9800m, "/images/documents/property2002-POO.jpg", 1, 146.0, "CairoGovernorate", 1, "Heliopolis Family Flat", 0, 9, "11757" },
                    { 2003L, "8 Road 9", 1, 1, 1, "Maadi", new DateTime(2026, 5, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "Quiet studio with easy access to Maadi services", null, true, false, 29.9602, 31.256900000000002, 2, new Guid("44444444-4444-4444-4444-444444444444"), 280m, "/images/documents/property2003-POO.jpg", 0, 62.0, "CairoGovernorate", 1, "Maadi Garden Studio", 4, 7, "11431" },
                    { 2004L, "41 Makram Ebeid Street", 1, 1, 3, "Nasr City", new DateTime(2026, 5, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, "Shared loft suited for students and young professionals", null, true, true, 30.0626, 31.330100000000002, 3, new Guid("66666666-6666-6666-6666-666666666666"), 3200m, "/images/documents/property2004-POO.jpg", 1, 88.0, "CairoGovernorate", 0, "Nasr City Shared Loft", 5, 5, "11765" },
                    { 2005L, "10 South 90 Street", 3, 4, 5, "New Cairo", new DateTime(2026, 5, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "Modern villa in a quiet New Cairo neighborhood", null, true, false, 30.030000000000001, 31.469999999999999, 6, new Guid("44444444-4444-4444-4444-444444444444"), 250000m, "/images/documents/property2005-POO.jpg", 2, 285.0, "CairoGovernorate", 1, "New Cairo Corner Villa", 3, 11, "11835" },
                    { 2006L, "33 Corniche Road", 1, 2, 2, "Alexandria", new DateTime(2026, 5, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, "Sea facing apartment near the Alexandria Corniche", null, true, false, 31.200099999999999, 29.918700000000001, 3, new Guid("66666666-6666-6666-6666-666666666666"), 350m, "/images/documents/property2006-POO.jpg", 0, 110.0, "AlexandriaGovernorate", 1, "Corniche Sea Apartment", 0, 18, "21519" },
                    { 2007L, "12 El Horreya Road", 2, 2, 3, "Sidi Gaber", new DateTime(2026, 5, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, "Well placed flat near transport and universities", null, true, false, 31.215599999999998, 29.942, 4, new Guid("44444444-4444-4444-4444-444444444444"), 8100m, "/images/documents/property2007-POO.jpg", 1, 124.0, "AlexandriaGovernorate", 1, "Sidi Gaber Urban Flat", 0, 12, "21615" },
                    { 2008L, "27 Fawzy Moaz Street", 2, 3, 4, "Smouha", new DateTime(2026, 5, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, "Spacious residence overlooking a calm residential area", null, true, false, 31.215, 29.955300000000001, 5, new Guid("66666666-6666-6666-6666-666666666666"), 130000m, "/images/documents/property2008-POO.jpg", 2, 172.0, "AlexandriaGovernorate", 1, "Smouha Park Residence", 1, 10, "21646" },
                    { 2009L, "50 Khaled Ibn Al Walid Street", 1, 1, 1, "Miami", new DateTime(2026, 5, 9, 0, 0, 0, 0, DateTimeKind.Utc), null, "Compact studio within walking distance of the beach", null, true, false, 31.267700000000001, 30.0046, 2, new Guid("44444444-4444-4444-4444-444444444444"), 4900m, "/images/documents/property2009-POO.jpg", 1, 58.0, "AlexandriaGovernorate", 0, "Miami Beach Studio", 4, 16, "21919" },
                    { 2010L, "6 Malek Hefny Street", 3, 4, 6, "Montaza", new DateTime(2026, 5, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, "Large villa near the Montaza district gardens", null, true, false, 31.285399999999999, 30.017299999999999, 7, new Guid("66666666-6666-6666-6666-666666666666"), 24500m, "/images/documents/property2010-POO.jpg", 1, 310.0, "AlexandriaGovernorate", 1, "Montaza Family Villa", 3, 8, "21923" },
                    { 2011L, "14 Talat Harb Street", 1, 2, 2, "Zagazig", new DateTime(2026, 5, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, "Clean apartment close to central Zagazig amenities", null, true, false, 30.587700000000002, 31.501999999999999, 3, new Guid("44444444-4444-4444-4444-444444444444"), 55000m, "/images/documents/property2011-POO.jpg", 2, 102.0, "SharkiaGovernorate", 1, "Zagazig Central Apartment", 0, 6, "44511" },
                    { 2012L, "88 Industrial Zone Road", 1, 1, 1, "Tenth of Ramadan", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, "Practical loft near business zones in 10th of Ramadan", null, true, false, 30.304500000000001, 31.742000000000001, 2, new Guid("66666666-6666-6666-6666-666666666666"), 4700m, "/images/documents/property2012-POO.jpg", 1, 76.0, "SharkiaGovernorate", 0, "Tenth District Loft", 0, 4, "44629" },
                    { 2013L, "19 Saad Zaghloul Street", 2, 3, 4, "Belbeis", new DateTime(2026, 5, 13, 0, 0, 0, 0, DateTimeKind.Utc), null, "Traditional house with a private courtyard and storage", null, true, false, 30.420400000000001, 31.562200000000001, 5, new Guid("44444444-4444-4444-4444-444444444444"), 72000m, "/images/documents/property2013-POO.jpg", 2, 180.0, "SharkiaGovernorate", 1, "Belbeis Courtyard House", 1, 3, "44621" },
                    { 2014L, "9 El Geish Street", 2, 2, 4, "Minya Al Qamh", new DateTime(2026, 5, 14, 0, 0, 0, 0, DateTimeKind.Utc), null, "Shared home designed for longer affordable stays", null, true, true, 30.422799999999999, 31.369700000000002, 4, new Guid("66666666-6666-6666-6666-666666666666"), 2600m, "/images/documents/property2014-POO.jpg", 1, 130.0, "SharkiaGovernorate", 1, "Minya Al Qamh Shared Home", 5, 2, "44661" },
                    { 2015L, "31 Port Said Street", 1, 2, 3, "Abu Hammad", new DateTime(2026, 5, 15, 0, 0, 0, 0, DateTimeKind.Utc), null, "Bright flat near local markets and key roads", null, true, false, 30.536899999999999, 31.683499999999999, 3, new Guid("44444444-4444-4444-4444-444444444444"), 5100m, "/images/documents/property2015-POO.jpg", 1, 97.0, "SharkiaGovernorate", 1, "Abu Hammad Riverside Flat", 0, 5, "44671" },
                    { 2016L, "18 El Galaa Street", 1, 2, 2, "Damietta", new DateTime(2026, 5, 16, 0, 0, 0, 0, DateTimeKind.Utc), null, "Modern apartment near the city center and port", null, true, false, 31.416499999999999, 31.813300000000002, 3, new Guid("66666666-6666-6666-6666-666666666666"), 300m, "/images/documents/property2016-POO.jpg", 0, 108.0, "DamiettaGovernorate", 1, "Damietta Port Apartment", 0, 7, "34511" },
                    { 2017L, "5 Nile Street", 1, 1, 1, "Ras El Bar", new DateTime(2026, 5, 17, 0, 0, 0, 0, DateTimeKind.Utc), null, "Compact studio ideal for short coastal stays", null, true, false, 31.508500000000002, 31.840399999999999, 2, new Guid("44444444-4444-4444-4444-444444444444"), 3800m, "/images/documents/property2017-POO.jpg", 0, 54.0, "DamiettaGovernorate", 1, "Ras El Bar Summer Studio", 4, 13, "34711" },
                    { 2018L, "44 Al Gamea Street", 2, 3, 3, "New Damietta", new DateTime(2026, 5, 18, 0, 0, 0, 0, DateTimeKind.Utc), null, "Contemporary flat in a newer planned district", null, true, false, 31.445599999999999, 31.676400000000001, 4, new Guid("66666666-6666-6666-6666-666666666666"), 420m, "/images/documents/property2018-POO.jpg", 0, 138.0, "DamiettaGovernorate", 0, "New Damietta Corner Flat", 0, 4, "34517" },
                    { 2019L, "11 Mostafa Kamel Street", 2, 3, 4, "Kafr Saad", new DateTime(2026, 5, 19, 0, 0, 0, 0, DateTimeKind.Utc), null, "Well sized family house with a practical layout", null, true, false, 31.355399999999999, 31.676300000000001, 5, new Guid("44444444-4444-4444-4444-444444444444"), 7300m, "/images/documents/property2019-POO.jpg", 1, 192.0, "DamiettaGovernorate", 1, "Kafr Saad Family House", 1, 3, "34614" },
                    { 2020L, "7 Omar Ibn El Khattab Street", 3, 4, 5, "Faraskur", new DateTime(2026, 5, 20, 0, 0, 0, 0, DateTimeKind.Utc), null, "Large home with generous indoor living space", null, true, false, 31.329000000000001, 31.715900000000001, 6, new Guid("66666666-6666-6666-6666-666666666666"), 96000m, "/images/documents/property2020-POO.jpg", 2, 230.0, "DamiettaGovernorate", 1, "Faraskur Riverside Home", 1, 2, "34631" },
                    { 3001L, "Dummy Address 1", 1, 1, 2, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 1", null, true, false, 30.010000000000002, 31.010000000000002, 2, new Guid("44444444-4444-4444-4444-444444444444"), 1000m, "/images/documents/dummy.jpg", 1, 80.0, "CairoGovernorate", 1, "Dummy Property 1", 0, 0, "11111" },
                    { 3002L, "Dummy Address 2", 1, 1, 1, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 2", null, true, false, 30.02, 31.02, 1, new Guid("44444444-4444-4444-4444-444444444444"), 1500m, "/images/documents/dummy.jpg", 1, 50.0, "GizaGovernorate", 1, "Dummy Property 2", 4, 0, "11112" },
                    { 3003L, "Dummy Address 3", 2, 3, 4, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 3", null, true, false, 30.030000000000001, 31.030000000000001, 4, new Guid("44444444-4444-4444-4444-444444444444"), 5000m, "/images/documents/dummy.jpg", 1, 150.0, "AlexandriaGovernorate", 1, "Dummy Property 3", 1, 0, "11113" },
                    { 3004L, "Dummy Address 4", 3, 4, 5, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 4", null, true, false, 30.039999999999999, 31.039999999999999, 6, new Guid("44444444-4444-4444-4444-444444444444"), 10000m, "/images/documents/dummy.jpg", 1, 250.0, "SharkiaGovernorate", 1, "Dummy Property 4", 3, 0, "11114" },
                    { 3005L, "Dummy Address 5", 1, 1, 2, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 5", null, true, true, 30.050000000000001, 31.050000000000001, 2, new Guid("44444444-4444-4444-4444-444444444444"), 800m, "/images/documents/dummy.jpg", 1, 60.0, "DamiettaGovernorate", 1, "Dummy Property 5", 5, 0, "11115" },
                    { 3006L, "Dummy Address 6", 1, 2, 3, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 6", null, true, false, 30.059999999999999, 31.059999999999999, 3, new Guid("44444444-4444-4444-4444-444444444444"), 1200m, "/images/documents/dummy.jpg", 1, 90.0, "CairoGovernorate", 1, "Dummy Property 6", 0, 0, "11116" },
                    { 3007L, "Dummy Address 7", 1, 1, 1, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 7", null, true, false, 30.07, 31.07, 1, new Guid("44444444-4444-4444-4444-444444444444"), 1100m, "/images/documents/dummy.jpg", 1, 45.0, "GizaGovernorate", 1, "Dummy Property 7", 4, 0, "11117" },
                    { 3008L, "Dummy Address 8", 2, 3, 4, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 8", null, true, false, 30.079999999999998, 31.079999999999998, 5, new Guid("44444444-4444-4444-4444-444444444444"), 4800m, "/images/documents/dummy.jpg", 1, 160.0, "AlexandriaGovernorate", 1, "Dummy Property 8", 1, 0, "11118" },
                    { 3009L, "Dummy Address 9", 4, 5, 6, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 9", null, true, false, 30.09, 31.09, 8, new Guid("44444444-4444-4444-4444-444444444444"), 15000m, "/images/documents/dummy.jpg", 1, 300.0, "SharkiaGovernorate", 1, "Dummy Property 9", 3, 0, "11119" },
                    { 3010L, "Dummy Address 10", 1, 1, 3, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 10", null, true, true, 30.100000000000001, 31.100000000000001, 3, new Guid("44444444-4444-4444-4444-444444444444"), 900m, "/images/documents/dummy.jpg", 1, 70.0, "DamiettaGovernorate", 1, "Dummy Property 10", 5, 0, "11120" },
                    { 3011L, "Dummy Address 11", 1, 1, 2, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 11", null, true, false, 30.109999999999999, 31.109999999999999, 2, new Guid("55555555-5555-5555-5555-555555555555"), 1300m, "/images/documents/dummy.jpg", 1, 85.0, "CairoGovernorate", 1, "Dummy Property 11", 0, 0, "11121" },
                    { 3012L, "Dummy Address 12", 1, 1, 2, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 12", null, true, false, 30.120000000000001, 31.120000000000001, 2, new Guid("55555555-5555-5555-5555-555555555555"), 1600m, "/images/documents/dummy.jpg", 1, 55.0, "GizaGovernorate", 1, "Dummy Property 12", 4, 0, "11122" },
                    { 3013L, "Dummy Address 13", 2, 3, 5, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 13", null, true, false, 30.129999999999999, 31.129999999999999, 6, new Guid("55555555-5555-5555-5555-555555555555"), 5200m, "/images/documents/dummy.jpg", 1, 170.0, "AlexandriaGovernorate", 1, "Dummy Property 13", 1, 0, "11123" },
                    { 3014L, "Dummy Address 14", 4, 4, 6, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 14", null, true, false, 30.140000000000001, 31.140000000000001, 8, new Guid("55555555-5555-5555-5555-555555555555"), 12000m, "/images/documents/dummy.jpg", 1, 280.0, "SharkiaGovernorate", 1, "Dummy Property 14", 3, 0, "11124" },
                    { 3015L, "Dummy Address 15", 1, 1, 2, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 15", null, true, true, 30.149999999999999, 31.149999999999999, 2, new Guid("55555555-5555-5555-5555-555555555555"), 850m, "/images/documents/dummy.jpg", 1, 65.0, "DamiettaGovernorate", 1, "Dummy Property 15", 5, 0, "11125" },
                    { 3016L, "Dummy Address 16", 2, 3, 4, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 16", null, true, false, 30.16, 31.16, 4, new Guid("55555555-5555-5555-5555-555555555555"), 2500m, "/images/documents/dummy.jpg", 1, 120.0, "CairoGovernorate", 1, "Dummy Property 16", 0, 0, "11126" },
                    { 3017L, "Dummy Address 17", 1, 1, 1, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 17", null, true, false, 30.170000000000002, 31.170000000000002, 1, new Guid("55555555-5555-5555-5555-555555555555"), 1000m, "/images/documents/dummy.jpg", 1, 40.0, "GizaGovernorate", 1, "Dummy Property 17", 4, 0, "11127" },
                    { 3018L, "Dummy Address 18", 2, 3, 4, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 18", null, true, false, 30.18, 31.18, 5, new Guid("55555555-5555-5555-5555-555555555555"), 4600m, "/images/documents/dummy.jpg", 1, 155.0, "AlexandriaGovernorate", 1, "Dummy Property 18", 1, 0, "11128" },
                    { 3019L, "Dummy Address 19", 5, 5, 8, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 19", null, true, false, 30.190000000000001, 31.190000000000001, 10, new Guid("55555555-5555-5555-5555-555555555555"), 18000m, "/images/documents/dummy.jpg", 1, 350.0, "SharkiaGovernorate", 1, "Dummy Property 19", 3, 0, "11129" },
                    { 3020L, "Dummy Address 20", 2, 2, 4, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 20", null, true, true, 30.199999999999999, 31.199999999999999, 4, new Guid("55555555-5555-5555-5555-555555555555"), 950m, "/images/documents/dummy.jpg", 1, 80.0, "DamiettaGovernorate", 1, "Dummy Property 20", 5, 0, "11130" },
                    { 3021L, "Dummy Address 21", 1, 2, 3, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 21", null, true, false, 30.210000000000001, 31.210000000000001, 3, new Guid("66666666-6666-6666-6666-666666666666"), 1800m, "/images/documents/dummy.jpg", 1, 95.0, "CairoGovernorate", 1, "Dummy Property 21", 0, 0, "11131" },
                    { 3022L, "Dummy Address 22", 1, 1, 2, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 22", null, true, false, 30.219999999999999, 31.219999999999999, 2, new Guid("66666666-6666-6666-6666-666666666666"), 1400m, "/images/documents/dummy.jpg", 1, 48.0, "GizaGovernorate", 1, "Dummy Property 22", 4, 0, "11132" },
                    { 3023L, "Dummy Address 23", 2, 3, 4, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 23", null, true, false, 30.23, 31.23, 5, new Guid("66666666-6666-6666-6666-666666666666"), 4900m, "/images/documents/dummy.jpg", 1, 165.0, "AlexandriaGovernorate", 1, "Dummy Property 23", 1, 0, "11133" },
                    { 3024L, "Dummy Address 24", 3, 4, 6, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 24", null, true, false, 30.239999999999998, 31.239999999999998, 8, new Guid("66666666-6666-6666-6666-666666666666"), 11000m, "/images/documents/dummy.jpg", 1, 270.0, "SharkiaGovernorate", 1, "Dummy Property 24", 3, 0, "11134" },
                    { 3025L, "Dummy Address 25", 1, 2, 3, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 25", null, true, true, 30.25, 31.25, 3, new Guid("66666666-6666-6666-6666-666666666666"), 880m, "/images/documents/dummy.jpg", 1, 72.0, "DamiettaGovernorate", 1, "Dummy Property 25", 5, 0, "11135" },
                    { 3026L, "Dummy Address 26", 1, 1, 2, "Cairo", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 26", null, true, false, 30.260000000000002, 31.260000000000002, 2, new Guid("66666666-6666-6666-6666-666666666666"), 1700m, "/images/documents/dummy.jpg", 1, 78.0, "CairoGovernorate", 1, "Dummy Property 26", 0, 0, "11136" },
                    { 3027L, "Dummy Address 27", 1, 1, 1, "Giza", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 27", null, true, false, 30.27, 31.27, 1, new Guid("66666666-6666-6666-6666-666666666666"), 1250m, "/images/documents/dummy.jpg", 1, 42.0, "GizaGovernorate", 1, "Dummy Property 27", 4, 0, "11137" },
                    { 3028L, "Dummy Address 28", 2, 3, 5, "Alexandria", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 28", null, true, false, 30.280000000000001, 31.280000000000001, 6, new Guid("66666666-6666-6666-6666-666666666666"), 5500m, "/images/documents/dummy.jpg", 1, 180.0, "AlexandriaGovernorate", 1, "Dummy Property 28", 1, 0, "11138" },
                    { 3029L, "Dummy Address 29", 6, 6, 10, "Zagazig", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 29", null, true, false, 30.289999999999999, 31.289999999999999, 12, new Guid("66666666-6666-6666-6666-666666666666"), 22000m, "/images/documents/dummy.jpg", 1, 400.0, "SharkiaGovernorate", 1, "Dummy Property 29", 3, 0, "11139" },
                    { 3030L, "Dummy Address 30", 2, 2, 4, "Damietta", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "Dummy description for property 30", null, true, true, 30.300000000000001, 31.300000000000001, 4, new Guid("66666666-6666-6666-6666-666666666666"), 920m, "/images/documents/dummy.jpg", 1, 85.0, "DamiettaGovernorate", 1, "Dummy Property 30", 5, 0, "11140" }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "ActionTaken", "CreatedAt", "Reason", "ReportableGuidId", "ReportableId", "ReportableType", "ReporterId", "ReviewedAt", "ReviewerId", "ReviewerNote", "Status" },
                values: new object[,]
                {
                    { 1L, null, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Misleading information about the property.", null, 1001L, 1, new Guid("11111111-1111-1111-1111-111111111111"), null, null, null, 0 },
                    { 9101L, null, new DateTime(2026, 5, 11, 9, 30, 0, 0, DateTimeKind.Utc), "Profile details look inconsistent and need manual review.", new Guid("10000000-0000-0000-0000-000000000004"), null, 0, new Guid("11111111-1111-1111-1111-111111111111"), null, null, null, 0 },
                    { 9102L, 2, new DateTime(2026, 5, 8, 10, 0, 0, 0, DateTimeKind.Utc), "Listing contains misleading availability details.", null, 1205L, 1, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 5, 8, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999999"), "Property deactivated until the owner corrects the listing.", 1 },
                    { 9103L, 3, new DateTime(2026, 4, 13, 8, 0, 0, 0, DateTimeKind.Utc), "Abusive language in chat.", new Guid("00000000-0000-0000-0000-000000000101"), null, 2, new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2026, 4, 13, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999999"), "Message hidden and sender banned.", 1 },
                    { 9104L, 4, new DateTime(2026, 4, 14, 10, 0, 0, 0, DateTimeKind.Utc), "Comment includes harassment.", null, 900101L, 3, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 4, 14, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999999"), "Comment hidden and the commenter was banned.", 1 },
                    { 9105L, null, new DateTime(2026, 5, 9, 9, 0, 0, 0, DateTimeKind.Utc), "Suspicious behavior, but without evidence.", new Guid("10000000-0000-0000-0000-000000000004"), null, 0, new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 5, 9, 11, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000001"), "Insufficient evidence after review.", 2 }
                });

            migrationBuilder.InsertData(
                table: "RoommatePreferences",
                columns: new[] { "Id", "BudgetImportance", "BudgetRangeMax", "BudgetRangeMin", "EducationImportance", "EducationLevel", "FieldOfStudy", "FieldOfStudyImportance", "Governorate", "GuestsFrequency", "GuestsFrequencyImportance", "NoiseTolerance", "NoiseToleranceImportance", "Pets", "PetsImportance", "RoommatePreferencesEnabled", "SearchStatus", "SharingLevel", "SharingLevelImportance", "SleepImportance", "SleepSchedule", "Smoking", "SmokingImportance", "UserId", "WorkSchedule", "WorkScheduleImportance" },
                values: new object[,]
                {
                    { 1L, 3, 6000m, 3000m, 3, 2, 1, 3, 0, 2, 3, 3, 3, true, 3, true, 0, 3, 3, 3, 1, false, 3, new Guid("11111111-1111-1111-1111-111111111111"), 2, 3 },
                    { 2L, 3, 4500m, 2000m, 3, 2, 5, 3, 0, 4, 3, 5, 3, false, 3, true, 0, 3, 3, 3, 2, true, 3, new Guid("22222222-2222-2222-2222-222222222222"), 5, 3 },
                    { 3L, 3, 3500m, 2000m, 3, 2, 3, 3, 0, 4, 3, 2, 3, false, 3, true, 0, 2, 3, 3, 1, false, 3, new Guid("33333333-3333-3333-3333-333333333333"), 5, 3 },
                    { 4L, 3, 5500m, 4000m, 3, 3, 1, 3, 0, 2, 3, 4, 3, true, 3, true, 1, 3, 3, 3, 3, false, 3, new Guid("77777777-7777-7777-7777-777777777777"), 2, 3 },
                    { 5L, 3, 10000m, 7000m, 3, 1, 5, 3, 0, 4, 3, 5, 3, false, 3, true, 0, 1, 3, 3, 2, true, 3, new Guid("88888888-8888-8888-8888-888888888888"), 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "UserActivities",
                columns: new[] { "Id", "CreatedAt", "Metadata", "PropertyId", "UserActivityType", "UserId" },
                values: new object[,]
                {
                    { 4001L, new DateTime(2026, 5, 21, 9, 0, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"cairo apartment\",\"governorate\":\"CairoGovernorate\",\"minPrice\":5000,\"maxPrice\":9000,\"type\":\"Apartment\",\"rentalUnit\":\"Monthly\",\"minBedrooms\":2,\"latitude\":30.0444,\"longitude\":31.2357,\"radiusKm\":15,\"page\":1,\"pageSize\":20}", null, "search", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 4002L, new DateTime(2026, 5, 21, 9, 2, 0, 0, DateTimeKind.Utc), null, 2001L, "view", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 4003L, new DateTime(2026, 5, 21, 9, 5, 0, 0, DateTimeKind.Utc), null, 2003L, "save", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 4004L, new DateTime(2026, 5, 21, 9, 9, 0, 0, DateTimeKind.Utc), null, 2002L, "booking", new Guid("11111111-1111-1111-1111-111111111111") },
                    { 4005L, new DateTime(2026, 5, 22, 10, 0, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"alex studio\",\"governorate\":\"AlexandriaGovernorate\",\"maxPrice\":6000,\"type\":\"Studio\",\"rentalUnit\":\"Monthly\",\"latitude\":31.2001,\"longitude\":29.9187,\"radiusKm\":12,\"page\":1,\"pageSize\":20}", null, "search", new Guid("22222222-2222-2222-2222-222222222222") },
                    { 4006L, new DateTime(2026, 5, 22, 10, 3, 0, 0, DateTimeKind.Utc), null, 2009L, "view", new Guid("22222222-2222-2222-2222-222222222222") },
                    { 4007L, new DateTime(2026, 5, 22, 10, 5, 0, 0, DateTimeKind.Utc), null, 2009L, "save", new Guid("22222222-2222-2222-2222-222222222222") },
                    { 4008L, new DateTime(2026, 5, 22, 10, 11, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"alex apartment\",\"governorate\":\"AlexandriaGovernorate\",\"minPrice\":5000,\"maxPrice\":8000,\"type\":\"Apartment\",\"rentalUnit\":\"Monthly\",\"page\":1,\"pageSize\":20}", null, "search", new Guid("22222222-2222-2222-2222-222222222222") },
                    { 4009L, new DateTime(2026, 5, 23, 11, 0, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"zagazig apartment\",\"governorate\":\"SharkiaGovernorate\",\"minPrice\":4500,\"maxPrice\":7000,\"rentalUnit\":\"Monthly\",\"minBedrooms\":2,\"latitude\":30.5877,\"longitude\":31.5020,\"radiusKm\":20,\"page\":1,\"pageSize\":20}", null, "search", new Guid("33333333-3333-3333-3333-333333333333") },
                    { 4010L, new DateTime(2026, 5, 23, 11, 2, 0, 0, DateTimeKind.Utc), null, 2011L, "view", new Guid("33333333-3333-3333-3333-333333333333") },
                    { 4011L, new DateTime(2026, 5, 23, 11, 10, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"damietta apartment\",\"governorate\":\"DamiettaGovernorate\",\"minPrice\":4500,\"maxPrice\":8000,\"rentalUnit\":\"Monthly\",\"minBedrooms\":2,\"latitude\":31.4165,\"longitude\":31.8133,\"radiusKm\":25,\"page\":1,\"pageSize\":20}", null, "search", new Guid("33333333-3333-3333-3333-333333333333") },
                    { 4012L, new DateTime(2026, 5, 23, 11, 14, 0, 0, DateTimeKind.Utc), null, 2016L, "save", new Guid("33333333-3333-3333-3333-333333333333") },
                    { 4013L, new DateTime(2026, 5, 24, 12, 0, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"cairo villa\",\"governorate\":\"CairoGovernorate\",\"minPrice\":15000,\"maxPrice\":26000,\"type\":\"Villa\",\"rentalUnit\":\"Monthly\",\"minBedrooms\":4,\"minBathrooms\":3,\"latitude\":30.0300,\"longitude\":31.4700,\"radiusKm\":20,\"page\":1,\"pageSize\":20}", null, "search", new Guid("77777777-7777-7777-7777-777777777777") },
                    { 4014L, new DateTime(2026, 5, 24, 12, 4, 0, 0, DateTimeKind.Utc), null, 2005L, "view", new Guid("77777777-7777-7777-7777-777777777777") },
                    { 4015L, new DateTime(2026, 5, 24, 12, 10, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"alex villa\",\"governorate\":\"AlexandriaGovernorate\",\"minPrice\":18000,\"maxPrice\":26000,\"type\":\"Villa\",\"rentalUnit\":\"Monthly\",\"minBedrooms\":4,\"minBathrooms\":3,\"latitude\":31.2854,\"longitude\":30.0173,\"radiusKm\":18,\"page\":1,\"pageSize\":20}", null, "search", new Guid("77777777-7777-7777-7777-777777777777") },
                    { 4016L, new DateTime(2026, 5, 24, 12, 15, 0, 0, DateTimeKind.Utc), null, 2010L, "booking", new Guid("77777777-7777-7777-7777-777777777777") },
                    { 4017L, new DateTime(2026, 5, 25, 13, 0, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"ras el bar studio\",\"governorate\":\"DamiettaGovernorate\",\"maxPrice\":5000,\"type\":\"Studio\",\"rentalUnit\":\"Daily\",\"latitude\":31.5085,\"longitude\":31.8404,\"radiusKm\":10,\"page\":1,\"pageSize\":20}", null, "search", new Guid("88888888-8888-8888-8888-888888888888") },
                    { 4018L, new DateTime(2026, 5, 25, 13, 3, 0, 0, DateTimeKind.Utc), null, 2017L, "view", new Guid("88888888-8888-8888-8888-888888888888") },
                    { 4019L, new DateTime(2026, 5, 25, 13, 12, 0, 0, DateTimeKind.Utc), "{\"keyword\":\"alex coast apartment\",\"governorate\":\"AlexandriaGovernorate\",\"minPrice\":6000,\"maxPrice\":8500,\"type\":\"Apartment\",\"rentalUnit\":\"Monthly\",\"latitude\":31.2156,\"longitude\":29.9420,\"radiusKm\":15,\"page\":1,\"pageSize\":20}", null, "search", new Guid("88888888-8888-8888-8888-888888888888") },
                    { 4020L, new DateTime(2026, 5, 25, 13, 16, 0, 0, DateTimeKind.Utc), null, 2006L, "save", new Guid("88888888-8888-8888-8888-888888888888") }
                });

            migrationBuilder.InsertData(
                table: "AdminActionLogs",
                columns: new[] { "Id", "ActionType", "AdminId", "CreatedAt", "MetadataJson", "Reason", "ReportId", "TargetGuidId", "TargetLongId", "TargetType" },
                values: new object[,]
                {
                    { 8101L, "DeactivateProperty", new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 5, 8, 12, 0, 0, 0, DateTimeKind.Utc), null, "Property deactivated until listing details are corrected.", 9102L, null, 1205L, "Property" },
                    { 8102L, "HideMessage", new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 4, 13, 9, 0, 0, 0, DateTimeKind.Utc), null, "Hidden abusive message.", 9103L, new Guid("00000000-0000-0000-0000-000000000101"), null, "Message" },
                    { 8103L, "BanUser", new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 4, 13, 9, 1, 0, 0, DateTimeKind.Utc), null, "Banned sender after abusive chat message.", 9103L, new Guid("10000000-0000-0000-0000-000000000002"), null, "Message" },
                    { 8104L, "HidePropertyComment", new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 4, 14, 12, 0, 0, 0, DateTimeKind.Utc), null, "Hidden harassing property comment.", 9104L, null, 900101L, "PropertyComment" },
                    { 8105L, "BanUser", new Guid("99999999-9999-9999-9999-999999999999"), new DateTime(2026, 4, 14, 12, 1, 0, 0, DateTimeKind.Utc), null, "Banned commenter after repeated harassment.", 9104L, new Guid("10000000-0000-0000-0000-000000000002"), null, "PropertyComment" }
                });

            migrationBuilder.InsertData(
                table: "BookingRequests",
                columns: new[] { "Id", "CreatedAt", "EndDate", "PaymentFrequency", "PropertyId", "RenterId", "StartDate" },
                values: new object[,]
                {
                    { 5001L, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1002L, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5002L, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, 1002L, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5003L, new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, 1003L, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 5004L, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0, 1003L, new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Contracts",
                columns: new[] { "Id", "AnchoredAt", "AnchoringStatus", "CreatedAt", "FileName", "FilePath", "Hash", "LeaseEndDate", "LeaseStartDate", "MerkleRoot", "OtsFilePath", "PaymentFrequency", "PropertyId", "RenterId", "SignedByRenterAt", "Status", "TotalContractAmount", "TransactionId" },
                values: new object[,]
                {
                    { 1000001L, null, 0, new DateTime(2025, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), "rental-contract-1000001.pdf", "wwwroot/contracts/1000001/1000001.pdf", "3039d56c00f0d4068ebe0b93a771e151c13954c3a18b5668817c573098f63198", new DateOnly(2027, 1, 1), new DateOnly(2026, 1, 1), null, "wwwroot/contracts/1000001/1000001.ots", 1, 1001L, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 1, 60000m, null },
                    { 1000002L, null, 0, new DateTime(2025, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), "rental-contract-1000002.pdf", "wwwroot/contracts/1000002/1000002.pdf", "ff411815aaad5ad467d9b4f65d194bff57438215019590ac11cef7ec788fca39", new DateOnly(2027, 1, 1), new DateOnly(2026, 1, 1), null, "wwwroot/contracts/1000002/1000002.ots", 2, 1002L, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 0, 90000m, null },
                    { 1000003L, null, 0, new DateTime(2025, 5, 24, 0, 0, 0, 0, DateTimeKind.Utc), "rental-contract-1000003.pdf", "wwwroot/contracts/1000003/1000003.pdf", "d7c850ed73db284d3804dbf6fa4e97d7ebebf30e046484d9a0ea2de8459b414d", new DateOnly(2027, 6, 1), new DateOnly(2025, 6, 1), null, "wwwroot/contracts/1000003/1000003.ots", 0, 1100L, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), 1, 96000m, null },
                    { 1000004L, null, 0, new DateTime(2026, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), "rental-contract-1000004.pdf", "wwwroot/contracts/1000004/1000004.pdf", "050a52314d17bad942a9552a176b93f3c706366c14792f5570379d511bae24ba", new DateOnly(2027, 2, 1), new DateOnly(2026, 2, 1), null, "wwwroot/contracts/1000004/1000004.ots", 1, 1100L, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), 1, 48000m, null },
                    { 1000005L, null, 0, new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), "rental-contract-1000005.pdf", "wwwroot/contracts/1000005/1000005.pdf", "037a1152d09ce6cecda1cc548dfce20efe010d53749dd5b7fa5409c2f1632139", new DateOnly(2024, 12, 31), new DateOnly(2024, 1, 1), null, "wwwroot/contracts/1000005/1000005.ots", 2, 1002L, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, 90000m, null },
                    { 1000006L, null, 0, new DateTime(2026, 4, 20, 0, 0, 0, 0, DateTimeKind.Utc), "rental-contract-1000006.pdf", "wwwroot/contracts/1000006/1000006.pdf", "59aa5fa3b0c47d6473f48638de632bd0e9de58332e4e3d77d6cdc3748c03de96", new DateOnly(2027, 5, 1), new DateOnly(2026, 5, 1), null, "wwwroot/contracts/1000006/1000006.ots", 1, 1004L, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), 2, 180000m, null },
                    { 1000101L, null, 0, new DateTime(2026, 5, 8, 13, 0, 0, 0, DateTimeKind.Utc), "seed-contract-1000101.pdf", null, "SEEDHASH1000101PENDINGADMINDASHBOARD", new DateOnly(2026, 7, 31), new DateOnly(2026, 6, 1), null, null, 1, 1204L, new Guid("10000000-0000-0000-0000-000000000004"), null, 0, 15600m, null },
                    { 1000102L, new DateTime(2025, 11, 30, 9, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 11, 28, 12, 0, 0, 0, DateTimeKind.Utc), "seed-contract-1000102.pdf", null, "SEEDHASH1000102REVENUEGRAPHADMINDASHBOARD", new DateOnly(2026, 6, 30), new DateOnly(2025, 12, 1), null, null, 1, 1003L, new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2025, 11, 29, 10, 0, 0, 0, DateTimeKind.Utc), 1, 42000m, null },
                    { 1000103L, new DateTime(2026, 5, 22, 9, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2026, 5, 20, 12, 0, 0, 0, DateTimeKind.Utc), "seed-contract-1000103.pdf", null, "SEEDHASH1000103BANNEDRENTERDASHBOARD", new DateOnly(2026, 12, 1), new DateOnly(2026, 6, 1), null, null, 1, 1205L, new Guid("10000000-0000-0000-0000-000000000002"), new DateTime(2026, 5, 21, 10, 0, 0, 0, DateTimeKind.Utc), 1, 30000m, null }
                });

            migrationBuilder.InsertData(
                table: "PropertyAmenities",
                columns: new[] { "Id", "Amenity", "PropertyId" },
                values: new object[,]
                {
                    { 1L, 0, 1001L },
                    { 2L, 2, 1001L },
                    { 3L, 8, 1001L },
                    { 4L, 0, 1002L },
                    { 5L, 4, 1002L },
                    { 6L, 5, 1002L },
                    { 7L, 0, 1003L },
                    { 8L, 12, 1003L },
                    { 9L, 0, 1004L },
                    { 10L, 2, 1004L },
                    { 11L, 6, 1004L },
                    { 12L, 7, 1004L },
                    { 13L, 1, 1004L },
                    { 14L, 0, 1100L },
                    { 15L, 8, 1100L },
                    { 16L, 12, 1100L },
                    { 17L, 14, 1100L },
                    { 18L, 0, 2001L },
                    { 19L, 2, 2001L },
                    { 20L, 8, 2001L },
                    { 21L, 11, 2001L },
                    { 22L, 14, 2001L },
                    { 23L, 5, 2001L },
                    { 24L, 0, 2002L },
                    { 25L, 2, 2002L },
                    { 26L, 8, 2002L },
                    { 27L, 11, 2002L },
                    { 28L, 12, 2002L },
                    { 29L, 14, 2002L },
                    { 30L, 0, 2003L },
                    { 31L, 2, 2003L },
                    { 32L, 8, 2003L },
                    { 33L, 4, 2003L },
                    { 34L, 10, 2003L },
                    { 35L, 0, 2004L },
                    { 36L, 2, 2004L },
                    { 37L, 8, 2004L },
                    { 38L, 12, 2004L },
                    { 39L, 0, 2005L },
                    { 40L, 2, 2005L },
                    { 41L, 6, 2005L },
                    { 42L, 7, 2005L },
                    { 43L, 1, 2005L },
                    { 44L, 17, 2005L },
                    { 45L, 8, 2005L },
                    { 46L, 0, 2006L },
                    { 47L, 2, 2006L },
                    { 48L, 14, 2006L },
                    { 49L, 11, 2006L },
                    { 50L, 4, 2006L },
                    { 51L, 0, 2007L },
                    { 52L, 2, 2007L },
                    { 53L, 8, 2007L },
                    { 54L, 5, 2007L },
                    { 55L, 14, 2007L },
                    { 56L, 0, 2008L },
                    { 57L, 2, 2008L },
                    { 58L, 8, 2008L },
                    { 59L, 1, 2008L },
                    { 60L, 17, 2008L },
                    { 61L, 0, 2009L },
                    { 62L, 2, 2009L },
                    { 63L, 8, 2009L },
                    { 64L, 14, 2009L },
                    { 65L, 11, 2009L },
                    { 66L, 0, 2010L },
                    { 67L, 2, 2010L },
                    { 68L, 6, 2010L },
                    { 69L, 1, 2010L },
                    { 70L, 14, 2010L },
                    { 71L, 17, 2010L },
                    { 72L, 0, 2011L },
                    { 73L, 8, 2011L },
                    { 74L, 11, 2011L },
                    { 75L, 14, 2011L },
                    { 76L, 0, 2012L },
                    { 77L, 2, 2012L },
                    { 78L, 8, 2012L },
                    { 79L, 11, 2012L },
                    { 80L, 4, 2012L },
                    { 81L, 0, 2013L },
                    { 82L, 8, 2013L },
                    { 83L, 11, 2013L },
                    { 84L, 1, 2013L },
                    { 85L, 0, 2014L },
                    { 86L, 8, 2014L },
                    { 87L, 12, 2014L },
                    { 88L, 11, 2014L },
                    { 89L, 0, 2015L },
                    { 90L, 8, 2015L },
                    { 91L, 14, 2015L },
                    { 92L, 0, 2016L },
                    { 93L, 2, 2016L },
                    { 94L, 8, 2016L },
                    { 95L, 5, 2016L },
                    { 96L, 0, 2017L },
                    { 97L, 14, 2017L },
                    { 98L, 8, 2017L },
                    { 99L, 11, 2017L },
                    { 100L, 0, 2018L },
                    { 101L, 2, 2018L },
                    { 102L, 8, 2018L },
                    { 103L, 14, 2018L },
                    { 104L, 0, 2019L },
                    { 105L, 8, 2019L },
                    { 106L, 11, 2019L },
                    { 107L, 1, 2019L },
                    { 108L, 0, 2020L },
                    { 109L, 8, 2020L },
                    { 110L, 11, 2020L },
                    { 111L, 14, 2020L },
                    { 112L, 1, 2020L },
                    { 113L, 0, 1201L },
                    { 114L, 8, 1201L },
                    { 115L, 5, 1201L },
                    { 116L, 14, 1201L },
                    { 117L, 0, 1202L },
                    { 118L, 8, 1202L },
                    { 119L, 1, 1202L },
                    { 120L, 14, 1202L },
                    { 121L, 0, 1203L },
                    { 122L, 8, 1203L },
                    { 123L, 2, 1203L },
                    { 124L, 0, 1204L },
                    { 125L, 2, 1204L },
                    { 126L, 14, 1204L },
                    { 127L, 11, 1204L },
                    { 128L, 1, 1204L },
                    { 129L, 17, 1204L },
                    { 130L, 0, 1205L },
                    { 131L, 2, 1205L },
                    { 132L, 6, 1205L },
                    { 133L, 1, 1205L },
                    { 134L, 17, 1205L },
                    { 135L, 8, 1205L }
                });

            migrationBuilder.InsertData(
                table: "PropertyFeedbacks",
                columns: new[] { "Id", "Content", "CreatedAt", "HiddenAt", "HiddenByAdminId", "HiddenReason", "IsHiddenByModeration", "PropertyId", "Rating", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 900001L, "Great place! Very clean and quiet.", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, 1001L, 5, null, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 900002L, "Awesome location, but the neighbors were a bit noisy.", new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, 1001L, 4, null, new Guid("22222222-2222-2222-2222-222222222222") },
                    { 900003L, "Superb luxury villa. Highly recommend!", new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, 1004L, 5, null, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 900101L, "This seeded comment was hidden by moderation for admin review testing.", new DateTime(2026, 4, 14, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 14, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999999"), "Seeded moderation example for admin dashboard testing.", true, 1001L, 1, null, new Guid("10000000-0000-0000-0000-000000000002") }
                });

            migrationBuilder.InsertData(
                table: "PropertyMedia",
                columns: new[] { "Id", "IsPrimary", "Path", "PropertyId" },
                values: new object[,]
                {
                    { 3001L, true, "/images/properties/property1001-main.jpg", 1001L },
                    { 3002L, false, "/images/properties/property1001-sec.jpg", 1001L },
                    { 3003L, true, "/images/properties/property1002-main.jpg", 1002L },
                    { 3004L, false, "/images/properties/property1002-sec.jpg", 1002L },
                    { 3005L, true, "/images/properties/property1003-main.jpg", 1003L },
                    { 3006L, false, "/images/properties/property1003-sec.jpg", 1003L },
                    { 3007L, true, "/images/properties/property1004-main.jpg", 1004L },
                    { 3008L, false, "/images/properties/property1004-sec.jpg", 1004L },
                    { 3009L, true, "/images/properties/property1100-main.jpg", 1100L },
                    { 3010L, false, "/images/properties/property1100-sec.jpg", 1100L },
                    { 3011L, true, "/images/properties/property2001-main.jpg", 2001L },
                    { 3012L, false, "/images/properties/property2001-sec.jpg", 2001L },
                    { 3013L, true, "/images/properties/property2002-main.jpg", 2002L },
                    { 3014L, false, "/images/properties/property2002-sec.jpg", 2002L },
                    { 3015L, true, "/images/properties/property2003-main.jpg", 2003L },
                    { 3016L, false, "/images/properties/property2003-sec.jpg", 2003L },
                    { 3017L, true, "/images/properties/property2004-main.jpg", 2004L },
                    { 3018L, false, "/images/properties/property2004-sec.jpg", 2004L },
                    { 3019L, true, "/images/properties/property2005-main.jpg", 2005L },
                    { 3020L, false, "/images/properties/property2005-sec.jpg", 2005L },
                    { 3021L, true, "/images/properties/property2006-main.jpg", 2006L },
                    { 3022L, false, "/images/properties/property2006-sec.jpg", 2006L },
                    { 3023L, true, "/images/properties/property2007-main.jpg", 2007L },
                    { 3024L, false, "/images/properties/property2007-sec.jpg", 2007L },
                    { 3025L, true, "/images/properties/property2008-main.jpg", 2008L },
                    { 3026L, false, "/images/properties/property2008-sec.jpg", 2008L },
                    { 3027L, true, "/images/properties/property2009-main.jpg", 2009L },
                    { 3028L, false, "/images/properties/property2009-sec.jpg", 2009L },
                    { 3029L, true, "/images/properties/property2010-main.jpg", 2010L },
                    { 3030L, false, "/images/properties/property2010-sec.jpg", 2010L },
                    { 3031L, true, "/images/properties/property2011-main.jpg", 2011L },
                    { 3032L, false, "/images/properties/property2011-sec.jpg", 2011L },
                    { 3033L, true, "/images/properties/property2012-main.jpg", 2012L },
                    { 3034L, false, "/images/properties/property2012-sec.jpg", 2012L },
                    { 3035L, true, "/images/properties/property2013-main.jpg", 2013L },
                    { 3036L, false, "/images/properties/property2013-sec.jpg", 2013L },
                    { 3037L, true, "/images/properties/property2014-main.jpg", 2014L },
                    { 3038L, false, "/images/properties/property2014-sec.jpg", 2014L },
                    { 3039L, true, "/images/properties/property2015-main.jpg", 2015L },
                    { 3040L, false, "/images/properties/property2015-sec.jpg", 2015L },
                    { 3041L, true, "/images/properties/property2016-main.jpg", 2016L },
                    { 3042L, false, "/images/properties/property2016-sec.jpg", 2016L },
                    { 3043L, true, "/images/properties/property2017-main.jpg", 2017L },
                    { 3044L, false, "/images/properties/property2017-sec.jpg", 2017L },
                    { 3045L, true, "/images/properties/property2018-main.jpg", 2018L },
                    { 3046L, false, "/images/properties/property2018-sec.jpg", 2018L },
                    { 3047L, true, "/images/properties/property2019-main.jpg", 2019L },
                    { 3048L, false, "/images/properties/property2019-sec.jpg", 2019L },
                    { 3049L, true, "/images/properties/property2020-main.jpg", 2020L },
                    { 3050L, false, "/images/properties/property2020-sec.jpg", 2020L },
                    { 3051L, true, "/images/properties/property1201-main.jpg", 1201L },
                    { 3052L, false, "/images/properties/property1201-sec.jpg", 1201L },
                    { 3053L, true, "/images/properties/property1202-main.jpg", 1202L },
                    { 3054L, false, "/images/properties/property1202-sec.jpg", 1202L },
                    { 3055L, true, "/images/properties/property1203-main.jpg", 1203L },
                    { 3056L, false, "/images/properties/property1203-sec.jpg", 1203L },
                    { 3057L, true, "/images/properties/property1204-main.jpg", 1204L },
                    { 3058L, false, "/images/properties/property1204-sec.jpg", 1204L },
                    { 3059L, true, "/images/properties/property1205-main.jpg", 1205L },
                    { 3060L, false, "/images/properties/property1205-sec.jpg", 1205L }
                });

            migrationBuilder.InsertData(
                table: "PropertyRules",
                columns: new[] { "Id", "PropertyId", "Rule" },
                values: new object[,]
                {
                    { 1L, 1001L, "No Smoking inside the apartment" },
                    { 2L, 1001L, "No parties or loud music after 11 PM" },
                    { 3L, 1002L, "Pets are not allowed" },
                    { 4L, 1003L, "Single occupancy only" },
                    { 5L, 1004L, "Respect the neighbors" },
                    { 6L, 1004L, "Smoking allowed only in the balcony" },
                    { 7L, 1100L, "Keep shared spaces clean" },
                    { 8L, 1100L, "No guests overnight without roommate approval" },
                    { 9L, 2001L, "Annual maintenance fees are split" },
                    { 10L, 2001L, "Quiet hours after 10 PM" },
                    { 11L, 2002L, "Families only" },
                    { 12L, 2002L, "Small pets allowed with prior consent" },
                    { 13L, 2003L, "Turn off AC when leaving the studio" },
                    { 14L, 2003L, "Checkout is at 12 PM" },
                    { 15L, 2004L, "Share chores weekly" },
                    { 16L, 2004L, "No smoking indoors" },
                    { 17L, 2005L, "Maintain the garden weekly" },
                    { 18L, 2005L, "No events or commercial filming" },
                    { 19L, 2006L, "Clean feet from sand before entering" },
                    { 20L, 2006L, "No loud music on balcony" },
                    { 21L, 2007L, "Maximum of 4 overnight occupants" },
                    { 22L, 2007L, "Inform owner before having visitors" },
                    { 23L, 2008L, "Respect neighbors' parking spaces" },
                    { 24L, 2008L, "No sub-leasing allowed" },
                    { 25L, 2009L, "No smoking in the studio" },
                    { 26L, 2009L, "Check-out by 11 AM" },
                    { 27L, 2010L, "Pool usage only until 8 PM" },
                    { 28L, 2010L, "No loud outdoor activities late at night" },
                    { 29L, 2011L, "Quiet hours after 10 PM" },
                    { 30L, 2012L, "Commercial use of the loft is prohibited" },
                    { 31L, 2012L, "Ideal for students or professionals" },
                    { 32L, 2013L, "No modification to the courtyard structure" },
                    { 33L, 2013L, "Pets allowed in the courtyard only" },
                    { 34L, 2014L, "Shared kitchen duties should be respected" },
                    { 35L, 2014L, "No loud gatherings" },
                    { 36L, 2015L, "Do not leave water taps running" },
                    { 37L, 2016L, "Daily trash disposal is required" },
                    { 38L, 2016L, "Turn off air conditioning when out" },
                    { 39L, 2017L, "Beach wear not allowed inside the living room" },
                    { 40L, 2017L, "Maximum 2 occupants" },
                    { 41L, 2018L, "Key return to the doorman on check-out" },
                    { 42L, 2018L, "No loud parties" },
                    { 43L, 2019L, "Respect local residential rules" },
                    { 44L, 2020L, "No smoking inside the house" },
                    { 45L, 2020L, "No pets allowed" },
                    { 46L, 1201L, "No smoking inside" },
                    { 47L, 1201L, "Respect the historic building rules" },
                    { 48L, 1202L, "Keep the garden area tidy" },
                    { 49L, 1202L, "No noisy gatherings after midnight" },
                    { 50L, 1203L, "For single occupants only" },
                    { 51L, 1204L, "Beach access cards must not be shared" },
                    { 52L, 1204L, "No pets" },
                    { 53L, 1205L, "Pool rules must be strictly followed" },
                    { 54L, 1205L, "Respect neighbors' privacy" }
                });

            migrationBuilder.InsertData(
                table: "SavedProperties",
                columns: new[] { "PropertyId", "UserId" },
                values: new object[,]
                {
                    { 1001L, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 1001L, new Guid("66666666-6666-6666-6666-666666666666") },
                    { 1002L, new Guid("22222222-2222-2222-2222-222222222222") },
                    { 1002L, new Guid("66666666-6666-6666-6666-666666666666") },
                    { 1003L, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 1004L, new Guid("10000000-0000-0000-0000-000000000002") },
                    { 2001L, new Guid("11111111-1111-1111-1111-111111111111") }
                });

            migrationBuilder.InsertData(
                table: "PaymentSchedules",
                columns: new[] { "Id", "Amount", "ContractId", "Currency", "DueDate", "PaymentIntentId", "Status" },
                values: new object[,]
                {
                    { 20001L, 5000m, 1000001L, "egp", new DateTime(2026, 1, 31, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20001", 3 },
                    { 20002L, 5000m, 1000001L, "egp", new DateTime(2026, 2, 28, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20002", 4 },
                    { 20003L, 5000m, 1000001L, "egp", new DateTime(2026, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20003", 5 },
                    { 20004L, 5000m, 1000001L, "egp", new DateTime(2026, 4, 30, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20004", 4 },
                    { 20005L, 5000m, 1000001L, "egp", new DateTime(2026, 5, 31, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20005", 4 },
                    { 20006L, 5000m, 1000001L, "egp", new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, 1 },
                    { 20007L, 5000m, 1000001L, "egp", new DateTime(2026, 7, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20008L, 5000m, 1000001L, "egp", new DateTime(2026, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20009L, 5000m, 1000001L, "egp", new DateTime(2026, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20010L, 5000m, 1000001L, "egp", new DateTime(2026, 10, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20011L, 5000m, 1000001L, "egp", new DateTime(2026, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20012L, 5000m, 1000001L, "egp", new DateTime(2026, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20013L, 96000m, 1000003L, "egp", new DateTime(2027, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20014L, 4000m, 1000004L, "egp", new DateTime(2026, 2, 25, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20014", 3 },
                    { 20015L, 4000m, 1000004L, "egp", new DateTime(2026, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20015", 4 },
                    { 20016L, 4000m, 1000004L, "egp", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20016", 5 },
                    { 20017L, 4000m, 1000004L, "egp", new DateTime(2026, 5, 31, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20017", 4 },
                    { 20018L, 4000m, 1000004L, "egp", new DateTime(2026, 6, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, 1 },
                    { 20019L, 4000m, 1000004L, "egp", new DateTime(2026, 7, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20020L, 4000m, 1000004L, "egp", new DateTime(2025, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20021L, 4000m, 1000004L, "egp", new DateTime(2025, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20022L, 4000m, 1000004L, "egp", new DateTime(2025, 10, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20023L, 4000m, 1000004L, "egp", new DateTime(2025, 11, 30, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20024L, 4000m, 1000004L, "egp", new DateTime(2025, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20025L, 22500m, 1000005L, "egp", new DateTime(2024, 3, 31, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20025", 5 },
                    { 20026L, 22500m, 1000005L, "egp", new DateTime(2024, 6, 30, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20026", 4 },
                    { 20027L, 22500m, 1000005L, "egp", new DateTime(2024, 9, 30, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20027", 5 },
                    { 20028L, 22500m, 1000005L, "egp", new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20028", 4 },
                    { 20029L, 15000m, 1000006L, "egp", new DateTime(2025, 5, 31, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20029", 4 },
                    { 20030L, 15000m, 1000006L, "egp", new DateTime(2025, 6, 30, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20030", 4 },
                    { 20031L, 15000m, 1000006L, "egp", new DateTime(2025, 7, 31, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20031", 5 },
                    { 20032L, 15000m, 1000006L, "egp", new DateTime(2025, 8, 31, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20032", 3 },
                    { 20101L, 6000m, 1000102L, "egp", new DateTime(2025, 12, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20101", 4 },
                    { 20102L, 6000m, 1000102L, "egp", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20102", 4 },
                    { 20103L, 6000m, 1000102L, "egp", new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20103", 4 },
                    { 20104L, 6000m, 1000102L, "egp", new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20104", 4 },
                    { 20105L, 6000m, 1000102L, "egp", new DateTime(2026, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20105", 4 },
                    { 20106L, 6000m, 1000102L, "egp", new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20106", 4 },
                    { 20107L, 6000m, 1000102L, "egp", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 },
                    { 20108L, 5000m, 1000103L, "egp", new DateTime(2026, 6, 23, 0, 0, 0, 0, DateTimeKind.Utc), null, 1 }
                });

            migrationBuilder.InsertData(
                table: "Payments",
                columns: new[] { "Id", "AmountTotal", "ApplicationUserId", "AvailableAt", "Currency", "OwnerAmount", "PaidAt", "PaymentIntentId", "PaymentScheduleId", "PlatformFee", "Status" },
                values: new object[,]
                {
                    { 30001L, 5000m, null, new DateTime(2026, 2, 8, 12, 0, 0, 0, DateTimeKind.Utc), "egp", 4500m, new DateTime(2026, 1, 29, 12, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20001", 20001L, 500m, 2 },
                    { 30002L, 5000m, null, new DateTime(2026, 3, 10, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 4500m, new DateTime(2026, 2, 28, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20002", 20002L, 500m, 2 },
                    { 30003L, 5000m, null, new DateTime(2026, 4, 15, 9, 0, 0, 0, DateTimeKind.Utc), "egp", 4500m, new DateTime(2026, 4, 5, 9, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20003", 20003L, 500m, 2 },
                    { 30004L, 5000m, null, new DateTime(2026, 5, 15, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 4500m, new DateTime(2026, 5, 5, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20004", 20004L, 500m, 1 },
                    { 30005L, 5000m, null, new DateTime(2026, 6, 15, 9, 0, 0, 0, DateTimeKind.Utc), "egp", 4500m, new DateTime(2026, 6, 5, 9, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20005", 20005L, 500m, 1 },
                    { 30006L, 4000m, null, new DateTime(2026, 3, 4, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 3600m, new DateTime(2026, 2, 22, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20014", 20014L, 400m, 2 },
                    { 30007L, 4000m, null, new DateTime(2026, 4, 10, 11, 0, 0, 0, DateTimeKind.Utc), "egp", 3600m, new DateTime(2026, 3, 31, 11, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20015", 20015L, 400m, 2 },
                    { 30008L, 4000m, null, new DateTime(2026, 5, 18, 9, 0, 0, 0, DateTimeKind.Utc), "egp", 3600m, new DateTime(2026, 5, 8, 9, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20016", 20016L, 400m, 2 },
                    { 30009L, 4000m, null, new DateTime(2026, 6, 10, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 3600m, new DateTime(2026, 5, 31, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20017", 20017L, 400m, 1 },
                    { 30010L, 22500m, null, new DateTime(2024, 4, 15, 14, 0, 0, 0, DateTimeKind.Utc), "egp", 20250m, new DateTime(2024, 4, 5, 14, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20025", 20025L, 2250m, 2 },
                    { 30011L, 22500m, null, new DateTime(2024, 7, 10, 11, 0, 0, 0, DateTimeKind.Utc), "egp", 20250m, new DateTime(2024, 6, 30, 11, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20026", 20026L, 2250m, 2 },
                    { 30012L, 22500m, null, new DateTime(2024, 10, 13, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 20250m, new DateTime(2024, 10, 3, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20027", 20027L, 2250m, 2 },
                    { 30013L, 22500m, null, new DateTime(2025, 1, 10, 9, 0, 0, 0, DateTimeKind.Utc), "egp", 20250m, new DateTime(2024, 12, 31, 9, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20028", 20028L, 2250m, 2 },
                    { 30014L, 15000m, null, new DateTime(2025, 6, 10, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 13500m, new DateTime(2025, 5, 31, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20029", 20029L, 1500m, 2 },
                    { 30015L, 15000m, null, new DateTime(2025, 7, 10, 11, 0, 0, 0, DateTimeKind.Utc), "egp", 13500m, new DateTime(2025, 6, 30, 11, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20030", 20030L, 1500m, 2 },
                    { 30016L, 15000m, null, new DateTime(2025, 8, 14, 9, 0, 0, 0, DateTimeKind.Utc), "egp", 13500m, new DateTime(2025, 8, 4, 9, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20031", 20031L, 1500m, 2 },
                    { 30017L, 15000m, null, new DateTime(2025, 9, 8, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 13500m, new DateTime(2025, 8, 29, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20032", 20032L, 1500m, 2 },
                    { 30101L, 6000m, null, new DateTime(2025, 12, 11, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 5400m, new DateTime(2025, 12, 1, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20101", 20101L, 600m, 1 },
                    { 30102L, 6000m, null, new DateTime(2026, 1, 11, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 5400m, new DateTime(2026, 1, 1, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20102", 20102L, 600m, 1 },
                    { 30103L, 6000m, null, new DateTime(2026, 2, 11, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 5400m, new DateTime(2026, 2, 1, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20103", 20103L, 600m, 1 },
                    { 30104L, 6000m, null, new DateTime(2026, 3, 11, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 5400m, new DateTime(2026, 3, 1, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20104", 20104L, 600m, 1 },
                    { 30105L, 6000m, null, new DateTime(2026, 4, 11, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 5400m, new DateTime(2026, 4, 1, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20105", 20105L, 600m, 1 },
                    { 30106L, 6000m, null, new DateTime(2026, 5, 11, 10, 0, 0, 0, DateTimeKind.Utc), "egp", 5400m, new DateTime(2026, 5, 1, 10, 0, 0, 0, DateTimeKind.Utc), "pi_seed_20106", 20106L, 600m, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdminActionLogs_AdminId",
                table: "AdminActionLogs",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminActionLogs_ReportId",
                table: "AdminActionLogs",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminAnalyticsReports_GeneratedAt",
                table: "AdminAnalyticsReports",
                column: "GeneratedAt");

            migrationBuilder.CreateIndex(
                name: "IX_AdminAnalyticsReports_GeneratedByAdminId",
                table: "AdminAnalyticsReports",
                column: "GeneratedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminAnalyticsReports_Scope_Format",
                table: "AdminAnalyticsReports",
                columns: new[] { "Scope", "Format" });

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
                name: "IX_assistantMessages_SessionId_CreatedAt",
                table: "assistantMessages",
                columns: new[] { "SessionId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_assistantMessages_SessionId_ToolOnly_CreatedAt",
                table: "assistantMessages",
                columns: new[] { "SessionId", "ToolOnly", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_assistantMessages_UserId",
                table: "assistantMessages",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_assistantSessions_UserId_UpdatedAt",
                table: "assistantSessions",
                columns: new[] { "UserId", "UpdatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_BookingRequests_PropertyId_RenterId",
                table: "BookingRequests",
                columns: new[] { "PropertyId", "RenterId" });

            migrationBuilder.CreateIndex(
                name: "IX_BookingRequests_RenterId",
                table: "BookingRequests",
                column: "RenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_PropertyId_RenterId",
                table: "Contracts",
                columns: new[] { "PropertyId", "RenterId" });

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_RenterId",
                table: "Contracts",
                column: "RenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_ApplicationUserId",
                table: "Payments",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_AvailableAt",
                table: "Payments",
                column: "AvailableAt");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentIntentId",
                table: "Payments",
                column: "PaymentIntentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentScheduleId",
                table: "Payments",
                column: "PaymentScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentSchedules_ContractId_DueDate",
                table: "PaymentSchedules",
                columns: new[] { "ContractId", "DueDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Properties_OwnerId",
                table: "Properties",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Status",
                table: "Properties",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyAmenities_PropertyId",
                table: "PropertyAmenities",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyFeedbacks_PropertyId_UserId",
                table: "PropertyFeedbacks",
                columns: new[] { "PropertyId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyFeedbacks_UserId",
                table: "PropertyFeedbacks",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyMedia_PropertyId",
                table: "PropertyMedia",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyRules_PropertyId",
                table: "PropertyRules",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportableType",
                table: "Reports",
                column: "ReportableType");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterId",
                table: "Reports",
                column: "ReporterId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReviewerId",
                table: "Reports",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_Status",
                table: "Reports",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_RoommatePreferences_UserId",
                table: "RoommatePreferences",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SavedProperties_UserId",
                table: "SavedProperties",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserActivities_UserId_UserActivityType_CreatedAt",
                table: "UserActivities",
                columns: new[] { "UserId", "UserActivityType", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminActionLogs");

            migrationBuilder.DropTable(
                name: "AdminAnalyticsReports");

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
                name: "assistantMessages");

            migrationBuilder.DropTable(
                name: "BookingRequests");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PropertyAmenities");

            migrationBuilder.DropTable(
                name: "PropertyFeedbacks");

            migrationBuilder.DropTable(
                name: "PropertyMedia");

            migrationBuilder.DropTable(
                name: "PropertyRules");

            migrationBuilder.DropTable(
                name: "RoommatePreferences");

            migrationBuilder.DropTable(
                name: "SavedProperties");

            migrationBuilder.DropTable(
                name: "UserActivities");

            migrationBuilder.DropTable(
                name: "UserDevices");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "assistantSessions");

            migrationBuilder.DropTable(
                name: "PaymentSchedules");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
