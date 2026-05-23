using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
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
                    Type = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Metadata = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                name: "PropertyComments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsHiddenByModeration = table.Column<bool>(type: "bit", nullable: false),
                    HiddenAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    HiddenByAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    HiddenReason = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertyComments_Properties_PropertyId",
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
                name: "PropertyRatings",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyRatings", x => x.Id);
                    table.CheckConstraint("CK_PropertyRating_Rating", "[Rating] >= 1 AND [Rating] <= 5");
                    table.ForeignKey(
                        name: "FK_PropertyRatings_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PropertyRatings_Properties_PropertyId",
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
                    { new Guid("33333333-3333-3333-3333-333333333333"), null, "Admin", "ADMIN" },
                    { new Guid("aaaaaaaa-1111-2222-3333-444444444444"), null, "Moderator", "MODERATOR" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "AccountStatus", "ArabicAddress", "ArabicFullName", "BackIdPhoto", "Bio", "ConcurrencyStamp", "Country", "CreatedAt", "DateOfBirth", "DeletedAt", "Email", "EmailConfirmed", "FirstName", "FrontIdPhoto", "Gender", "ImagesDeletionJob", "Language", "LastName", "LockoutEnabled", "LockoutEnd", "NationalIDNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "StatusBeforeBan", "StripeAccountId", "StripeChargesEnabled", "StripePayoutsEnabled", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), 0, 1, "15 شارع التسعين، القاهرة الجديدة", "مستخدم قيد التحقق", "/images/idCards/pending-renter-back.jpg", null, "SCENARIO-PENDING-RENTER-CONCURRENCY-STAMP", 1, new DateTime(2026, 5, 2, 9, 0, 0, 0, DateTimeKind.Utc), null, null, "pending.renter@example.com", true, "Pending", "/images/idCards/pending-renter-front.jpg", 2, null, 1, "Renter", false, null, "34567890123456", "PENDING.RENTER@EXAMPLE.COM", "PENDING.RENTER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SCENARIO-PENDING-RENTER-SECURITY-STAMP", null, null, false, false, false, "pending.renter@example.com" },
                    { new Guid("10000000-0000-0000-0000-000000000002"), 0, 4, "22 شارع النصر، مدينة نصر", "مستخدم موقوف", "/images/idCards/banned-renter-back.jpg", null, "SCENARIO-BANNED-RENTER-CONCURRENCY-STAMP", 1, new DateTime(2026, 2, 14, 10, 0, 0, 0, DateTimeKind.Utc), null, null, "banned.renter@example.com", true, "Banned", "/images/idCards/banned-renter-front.jpg", 1, null, 0, "Renter", false, null, "45678901234567", "BANNED.RENTER@EXAMPLE.COM", "BANNED.RENTER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SCENARIO-BANNED-RENTER-SECURITY-STAMP", 1, null, false, false, false, "banned.renter@example.com" },
                    { new Guid("10000000-0000-0000-0000-000000000003"), 0, 2, null, null, null, null, "SCENARIO-DELETED-RENTER-CONCURRENCY-STAMP", 1, new DateTime(2026, 3, 3, 8, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 4, 1, 12, 0, 0, 0, DateTimeKind.Utc), "deleted.renter@example.com", true, "Deleted", null, 1, null, 0, "Renter", false, null, null, "DELETED.RENTER@EXAMPLE.COM", "DELETED.RENTER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SCENARIO-DELETED-RENTER-SECURITY-STAMP", null, null, false, false, false, "deleted.renter@example.com" },
                    { new Guid("10000000-0000-0000-0000-000000000004"), 0, 2, null, null, null, "Fresh account created to validate the dashboard new-user metrics.", "SCENARIO-RECENT-RENTER-CONCURRENCY-STAMP", 1, new DateTime(2026, 5, 10, 14, 30, 0, 0, DateTimeKind.Utc), null, null, "recent.renter@example.com", true, "Recent", null, 2, null, 0, "Renter", false, null, null, "RECENT.RENTER@EXAMPLE.COM", "RECENT.RENTER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SCENARIO-RECENT-RENTER-SECURITY-STAMP", null, null, false, false, false, "recent.renter@example.com" },
                    { new Guid("10000000-0000-0000-0000-000000000005"), 0, 2, null, null, null, "Seeded moderator candidate for role-management testing.", "SCENARIO-MODERATOR-USER-CONCURRENCY-STAMP", 1, new DateTime(2026, 4, 20, 11, 0, 0, 0, DateTimeKind.Utc), null, null, "moderator.user@example.com", true, "Mona", null, 2, null, 0, "Moderator", false, null, null, "MODERATOR.USER@EXAMPLE.COM", "MODERATOR.USER@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SCENARIO-MODERATOR-USER-SECURITY-STAMP", null, null, false, false, false, "moderator.user@example.com" },
                    { new Guid("11111111-1111-1111-1111-111111111111"), 0, 2, "123 شارع النيل، القاهرة", "رينتر ألفا", "/images/idCards/b8ee0c84-7a46-457d-a6d5-9696166b3c87.jpg", null, "SEED-RENTER-A-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "renter.a@example.com", true, "Renter", "/images/idCards/95c1567c-357c-4c0a-b711-e0ba27c1a96f.jpg", 1, null, 1, "Alpha", false, null, "12345678901234", "RENTER.A@EXAMPLE.COM", "RENTER.A@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, "/images/profiles/78e645e4-7c92-4cdc-b3bc-11a8f4ef796c.png", "SEED-RENTER-A-SECURITY-STAMP", null, null, false, false, false, "renter.a@example.com" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 0, 2, "456 شارع المعادي، القاهرة", "رينتر بيتا", "/images/idCards/0b2b1890-82ff-4459-be9a-6dc65971849a.jpg", null, "SEED-RENTER-B-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "renter.b@example.com", true, "Renter", "/images/idCards/f9797aa8-46ce-4dbb-ad14-2a521ed962fc.jpg", 2, null, 0, "Beta", false, null, "23456789012345", "RENTER.B@EXAMPLE.COM", "RENTER.B@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-RENTER-B-SECURITY-STAMP", null, null, false, false, false, "renter.b@example.com" },
                    { new Guid("30000000-0000-0000-0000-000000000001"), 0, 2, null, null, null, null, "SCENARIO-SECOND-ADMIN-CONCURRENCY-STAMP", 1, new DateTime(2026, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), null, null, "assistant.admin@marn.com", true, "Assistant", null, 0, null, 0, "Admin", false, null, null, "ASSISTANT.ADMIN@MARN.COM", "ASSISTANT.ADMIN@MARN.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SCENARIO-SECOND-ADMIN-SECURITY-STAMP", null, null, false, false, false, "assistant.admin@marn.com" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 0, 2, null, null, null, null, "SEED-RENTER-C-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "renter.c@example.com", true, "Renter", null, 1, null, 0, "Gamma", false, null, null, "RENTER.C@EXAMPLE.COM", "RENTER.C@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-RENTER-C-SECURITY-STAMP", null, null, false, false, false, "renter.c@example.com" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 0, 2, null, null, null, null, "SEED-OWNER-X-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "owner.x@example.com", true, "Owner", null, 1, null, 0, "X", false, null, null, "OWNER.X@EXAMPLE.COM", "OWNER.X@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-OWNER-X-SECURITY-STAMP", null, null, false, false, false, "owner.x@example.com" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 0, 2, null, null, null, null, "SEED-OWNER-Y-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "owner.y@example.com", true, "Owner", null, 2, null, 0, "Y", false, null, null, "OWNER.Y@EXAMPLE.COM", "OWNER.Y@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-OWNER-Y-SECURITY-STAMP", null, null, false, false, false, "owner.y@example.com" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), 0, 2, null, null, null, null, "SEED-OWNER-Z-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "owner.z@example.com", true, "Owner", null, 1, null, 0, "Z", false, null, null, "OWNER.Z@EXAMPLE.COM", "OWNER.Z@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-OWNER-Z-SECURITY-STAMP", null, null, false, false, false, "owner.z@example.com" },
                    { new Guid("77777777-7777-7777-7777-777777777777"), 0, 2, null, null, null, null, "SEED-RENTER-D-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "renter.d@example.com", true, "Renter", null, 1, null, 0, "Delta", false, null, null, "RENTER.D@EXAMPLE.COM", "RENTER.D@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-RENTER-D-SECURITY-STAMP", null, null, false, false, false, "renter.d@example.com" },
                    { new Guid("88888888-8888-8888-8888-888888888888"), 0, 2, null, null, null, null, "SEED-RENTER-E-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "renter.e@example.com", true, "Renter", null, 1, null, 0, "Epsilon", false, null, null, "RENTER.E@EXAMPLE.COM", "RENTER.E@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-RENTER-E-SECURITY-STAMP", null, null, false, false, false, "renter.e@example.com" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), 0, 2, null, null, null, null, "SEED-ADMIN-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "admin@marn.com", true, "System", null, 0, null, 0, "Admin", false, null, null, "ADMIN@MARN.COM", "ADMIN@MARN.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SEED-ADMIN-SECURITY-STAMP", null, null, false, false, false, "admin@marn.com" }
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
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("10000000-0000-0000-0000-000000000005") },
                    { new Guid("aaaaaaaa-1111-2222-3333-444444444444"), new Guid("10000000-0000-0000-0000-000000000005") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("30000000-0000-0000-0000-000000000001") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("66666666-6666-6666-6666-666666666666") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("66666666-6666-6666-6666-666666666666") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("77777777-7777-7777-7777-777777777777") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("88888888-8888-8888-8888-888888888888") },
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
                    { 6001L, null, 4, "Your next rent payment is due soon.", null, new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), "{\"propertyName\":\"Cozy Seed Apartment\"}", null, null, "Upcoming Payment Due", null, 10, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6002L, null, 4, "Your booking request has been accepted.", null, new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Booking Request Update", null, 6, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6003L, null, null, "Thanks for signing up!", null, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Welcome to the platform", null, 0, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6004L, "44444444-4444-4444-4444-444444444444", 2, "You have a new message from the owner.", null, new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "New Message", null, 1, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6005L, null, 3, "Add more details to your profile to get better recommendations.", null, new DateTime(2025, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2025, 3, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Complete Your Profile", null, 0, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6006L, "1002", 1, "A renter submitted a booking request for one of your properties.", null, new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "New booking request", null, 2, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6007L, null, 5, "A rent payment was successfully processed.", null, new DateTime(2025, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), "{\"amount\":\"1200\", \"currency\":\"USD\"}", null, null, "Payment received", null, 15, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6008L, null, 3, "Complete your listing details to attract more renters.", null, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Welcome, property owner", null, 0, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6009L, null, 4, "Your next rent payment for Cozy Seed Apartment is due soon.", null, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Rent Payment Due Soon", null, 10, new Guid("66666666-6666-6666-6666-666666666666"), 1 },
                    { 6010L, null, 4, "Your booking request for Seed Studio Flat has been submitted.", null, new DateTime(2025, 4, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Booking Submitted", null, 6, new Guid("66666666-6666-6666-6666-666666666666"), 1 },
                    { 6011L, null, null, "Thanks for joining MARN! Explore properties near you.", null, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Welcome to MARN", null, 0, new Guid("66666666-6666-6666-6666-666666666666"), 1 },
                    { 6012L, null, 5, "Luxury Seed Villa is now visible to renters.", null, new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, "Your property is live", null, 0, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6013L, null, 3, "Set up your payout details to start receiving rent payments.", null, new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Welcome, property owner", null, 0, new Guid("66666666-6666-6666-6666-666666666666"), 2 }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Address", "Bathrooms", "Bedrooms", "Beds", "City", "CreatedAt", "DeletedAt", "Description", "ImagesDeletionJob", "IsActive", "IsShared", "Latitude", "Longitude", "MaxOccupants", "OwnerId", "Price", "ProofOfOwnership", "RentalUnit", "SquareMeters", "State", "Status", "Title", "Type", "Views", "ZipCode" },
                values: new object[,]
                {
                    { 1001L, "123 Seed Street, Cairo", 1, 2, 3, "Cairo", new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "A cozy seeded apartment suitable for testing active rentals.", null, true, false, 30.0444, 31.235700000000001, 3, new Guid("44444444-4444-4444-4444-444444444444"), 5000m, "/images/documents/property1-POO.jpg", 1, 0.0, "CairoGovernorate", 1, "Cozy Seed Apartment", 0, 5, "11511" },
                    { 1002L, "456 Integration Avenue, Cairo", 1, 1, 1, "Cairo", new DateTime(2023, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "A modern loft used for pending booking and payments tests.", null, true, false, 30.050000000000001, 31.239999999999998, 2, new Guid("44444444-4444-4444-4444-444444444444"), 90000m, "/images/documents/property2-POO.jpg", 2, 0.0, "CairoGovernorate", 1, "Modern Seed Loft", 0, 3, "11512" },
                    { 1003L, "789 Scenario Road, Cairo", 1, 1, 1, "Giza", new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "A small studio property used for saved properties and pending bookings.", null, true, false, 30.059999999999999, 31.245000000000001, 1, new Guid("44444444-4444-4444-4444-444444444444"), 3500m, "/images/documents/property3-POO.jpg", 1, 0.0, "GizaGovernorate", 1, "Seed Studio Flat", 4, 1, "12511" },
                    { 1004L, "321 Elite Boulevard, Cairo", 3, 4, 5, "New Cairo", new DateTime(2025, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, "A luxury villa owned by the dual-role Owner Z for owner dashboard testing.", null, true, false, 30.07, 31.25, 6, new Guid("66666666-6666-6666-6666-666666666666"), 15000m, "/images/documents/property4-POO.jpg", 1, 0.0, "CairoGovernorate", 1, "Luxury Seed Villa", 3, 12, "11835" },
                    { 1100L, "555 Shared Lane, Cairo", 2, 3, 4, "Cairo", new DateTime(2024, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, "A shared house seeded for testing roommate matching logic.", null, true, true, 30.079999999999998, 31.260000000000002, 4, new Guid("44444444-4444-4444-4444-444444444444"), 4000m, "/images/documents/property100-POO.jpg", 1, 0.0, "CairoGovernorate", 1, "Shared Seed House", 1, 10, "11513" },
                    { 1201L, "10 Tahrir Square", 1, 1, 1, "Cairo", new DateTime(2026, 5, 3, 9, 0, 0, 0, DateTimeKind.Utc), null, "Ownership documents are uploaded and waiting for admin review.", null, true, false, 30.044, 31.234999999999999, 2, new Guid("55555555-5555-5555-5555-555555555555"), 6200m, "/docs/properties/pending-downtown-apartment.pdf", 1, 85.0, "CairoGovernorate", 0, "Pending Downtown Apartment", 0, 0, "11511" },
                    { 1202L, "88 Palm Street", 2, 3, 4, "Giza", new DateTime(2026, 4, 18, 12, 0, 0, 0, DateTimeKind.Utc), null, "A property with rejected ownership documentation for verification testing.", null, true, false, 30.010999999999999, 31.207999999999998, 5, new Guid("55555555-5555-5555-5555-555555555555"), 11000m, "/docs/properties/declined-garden-house.pdf", 1, 180.0, "GizaGovernorate", 2, "Declined Garden House", 1, 4, "12511" },
                    { 1203L, "34 Sunset Alley", 1, 1, 1, "Alexandria", new DateTime(2026, 3, 8, 16, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 4, 13, 0, 0, 0, DateTimeKind.Utc), "Soft deleted property used to validate include-deleted admin filters.", null, false, false, 31.199999999999999, 29.918700000000001, 1, new Guid("55555555-5555-5555-5555-555555555555"), 4300m, "/docs/properties/deleted-test-studio.pdf", 1, 55.0, "AlexandriaGovernorate", 1, "Soft Deleted Test Studio", 4, 1, "21511" },
                    { 1204L, "5 Marina Walk", 2, 2, 2, "North Coast", new DateTime(2026, 5, 5, 10, 0, 0, 0, DateTimeKind.Utc), null, "Fresh verified property created this month for dashboard trend checks.", null, true, false, 30.899999999999999, 28.899999999999999, 3, new Guid("55555555-5555-5555-5555-555555555555"), 7800m, "/docs/properties/recent-marina-flat.pdf", 1, 110.0, "MarsaMatruhGovernorate", 1, "Recent Marina Flat", 0, 9, "51711" },
                    { 1205L, "77 Corniche View", 3, 4, 5, "Luxor", new DateTime(2026, 5, 7, 15, 0, 0, 0, DateTimeKind.Utc), null, "Property already deactivated through a seeded moderation outcome.", null, false, false, 25.687200000000001, 32.639600000000002, 6, new Guid("55555555-5555-5555-5555-555555555555"), 16000m, "/docs/properties/moderated-riverside-villa.pdf", 1, 240.0, "LuxorGovernorate", 1, "Moderated Riverside Villa", 3, 22, "85951" }
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
                    { 9105L, null, new DateTime(2026, 5, 9, 9, 0, 0, 0, DateTimeKind.Utc), "Suspicious behavior, but without evidence.", new Guid("10000000-0000-0000-0000-000000000005"), null, 0, new Guid("33333333-3333-3333-3333-333333333333"), new DateTime(2026, 5, 9, 11, 0, 0, 0, DateTimeKind.Utc), new Guid("30000000-0000-0000-0000-000000000001"), "Insufficient evidence after review.", 2 }
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
                columns: new[] { "Id", "CreatedAt", "Description", "IPAddress", "Metadata", "Type", "UserId" },
                values: new object[,]
                {
                    { 1L, new DateTime(2025, 3, 24, 10, 0, 0, 0, DateTimeKind.Utc), "User logged in.", "127.0.0.1", null, 0, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 2L, new DateTime(2025, 3, 24, 10, 5, 0, 0, DateTimeKind.Utc), "User viewed property 1001.", null, "{\"PropertyId\": 1001}", 9, new Guid("11111111-1111-1111-1111-111111111111") }
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
                    { 1000001L, null, 0, new DateTime(2025, 12, 27, 0, 0, 0, 0, DateTimeKind.Utc), "rental-contract-1000001.pdf", "Storage/contracts/1000001/1000001.pdf", "3039d56c00f0d4068ebe0b93a771e151c13954c3a18b5668817c573098f63198", new DateOnly(2027, 1, 1), new DateOnly(2026, 1, 1), null, "Storage/contracts/1000001/1000001.ots", 1, 1001L, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), 1, 60000m, null },
                    { 1000002L, null, 0, new DateTime(2025, 12, 28, 0, 0, 0, 0, DateTimeKind.Utc), "rental-contract-1000002.pdf", "Storage/contracts/1000002/1000002.pdf", "ff411815aaad5ad467d9b4f65d194bff57438215019590ac11cef7ec788fca39", new DateOnly(2027, 1, 1), new DateOnly(2026, 1, 1), null, "Storage/contracts/1000002/1000002.ots", 2, 1002L, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2025, 12, 29, 0, 0, 0, 0, DateTimeKind.Utc), 0, 90000m, null },
                    { 1000003L, null, 0, new DateTime(2025, 5, 24, 0, 0, 0, 0, DateTimeKind.Utc), "rental-contract-1000003.pdf", "Storage/contracts/1000003/1000003.pdf", "d7c850ed73db284d3804dbf6fa4e97d7ebebf30e046484d9a0ea2de8459b414d", new DateOnly(2027, 6, 1), new DateOnly(2025, 6, 1), null, "Storage/contracts/1000003/1000003.ots", 0, 1100L, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 5, 25, 0, 0, 0, 0, DateTimeKind.Utc), 1, 96000m, null },
                    { 1000004L, null, 0, new DateTime(2026, 1, 27, 0, 0, 0, 0, DateTimeKind.Utc), "rental-contract-1000004.pdf", "Storage/contracts/1000004/1000004.pdf", "050a52314d17bad942a9552a176b93f3c706366c14792f5570379d511bae24ba", new DateOnly(2027, 2, 1), new DateOnly(2026, 2, 1), null, "Storage/contracts/1000004/1000004.ots", 1, 1100L, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 1, 28, 0, 0, 0, 0, DateTimeKind.Utc), 1, 48000m, null },
                    { 1000005L, null, 0, new DateTime(2023, 12, 19, 0, 0, 0, 0, DateTimeKind.Utc), "rental-contract-1000005.pdf", "Storage/contracts/1000005/1000005.pdf", "037a1152d09ce6cecda1cc548dfce20efe010d53749dd5b7fa5409c2f1632139", new DateOnly(2024, 12, 31), new DateOnly(2024, 1, 1), null, "Storage/contracts/1000005/1000005.ots", 2, 1002L, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2023, 12, 20, 0, 0, 0, 0, DateTimeKind.Utc), 3, 90000m, null },
                    { 1000006L, null, 0, new DateTime(2026, 4, 20, 0, 0, 0, 0, DateTimeKind.Utc), "rental-contract-1000006.pdf", "Storage/contracts/1000006/1000006.pdf", "59aa5fa3b0c47d6473f48638de632bd0e9de58332e4e3d77d6cdc3748c03de96", new DateOnly(2027, 5, 1), new DateOnly(2026, 5, 1), null, "Storage/contracts/1000006/1000006.ots", 1, 1004L, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 4, 25, 0, 0, 0, 0, DateTimeKind.Utc), 2, 180000m, null },
                    { 1000101L, null, 0, new DateTime(2026, 5, 8, 13, 0, 0, 0, DateTimeKind.Utc), "seed-contract-1000101.pdf", null, "SEEDHASH1000101PENDINGADMINDASHBOARD", new DateOnly(2026, 7, 31), new DateOnly(2026, 6, 1), null, null, 1, 1204L, new Guid("10000000-0000-0000-0000-000000000004"), null, 0, 15600m, null },
                    { 1000102L, new DateTime(2025, 11, 30, 9, 0, 0, 0, DateTimeKind.Utc), 1, new DateTime(2025, 11, 28, 12, 0, 0, 0, DateTimeKind.Utc), "seed-contract-1000102.pdf", null, "SEEDHASH1000102REVENUEGRAPHADMINDASHBOARD", new DateOnly(2026, 6, 30), new DateOnly(2025, 12, 1), null, null, 1, 1003L, new Guid("88888888-8888-8888-8888-888888888888"), new DateTime(2025, 11, 29, 10, 0, 0, 0, DateTimeKind.Utc), 1, 42000m, null }
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
                    { 13L, 1, 1004L }
                });

            migrationBuilder.InsertData(
                table: "PropertyComments",
                columns: new[] { "Id", "Content", "CreatedAt", "HiddenAt", "HiddenByAdminId", "HiddenReason", "IsHiddenByModeration", "PropertyId", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 900001L, "Great place! Very clean and quiet.", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, 1001L, null, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 900002L, "Awesome location, but the neighbors were a bit noisy.", new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, 1001L, null, new Guid("22222222-2222-2222-2222-222222222222") },
                    { 900003L, "Superb luxury villa. Highly recommend!", new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, null, null, false, 1004L, null, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 900101L, "This seeded comment was hidden by moderation for admin review testing.", new DateTime(2026, 4, 14, 8, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 4, 14, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("99999999-9999-9999-9999-999999999999"), "Seeded moderation example for admin dashboard testing.", true, 1001L, null, new Guid("10000000-0000-0000-0000-000000000002") }
                });

            migrationBuilder.InsertData(
                table: "PropertyMedia",
                columns: new[] { "Id", "IsPrimary", "Path", "PropertyId" },
                values: new object[,]
                {
                    { 2001L, true, "/images/properties/property1-main.jpg", 1001L },
                    { 2002L, false, "/images/properties/property1-secondary.jpg", 1001L },
                    { 2003L, true, "/images/properties/property2-main.jpg", 1002L },
                    { 2004L, true, "/images/properties/property3-main.jpg", 1003L },
                    { 2005L, true, "/images/properties/property4-main.jpg", 1004L },
                    { 2006L, true, "/images/properties/property100-main.jpg", 1100L }
                });

            migrationBuilder.InsertData(
                table: "PropertyRatings",
                columns: new[] { "Id", "CreatedAt", "PropertyId", "Rating", "UpdatedAt", "UserId" },
                values: new object[,]
                {
                    { 900001L, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1001L, 5, null, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 900002L, new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1001L, 4, null, new Guid("22222222-2222-2222-2222-222222222222") },
                    { 900003L, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1004L, 5, null, new Guid("11111111-1111-1111-1111-111111111111") }
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
                    { 6L, 1004L, "Smoking allowed only in the balcony" }
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
                    { 1003L, new Guid("11111111-1111-1111-1111-111111111111") }
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
                    { 20107L, 6000m, 1000102L, "egp", new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, 0 }
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
                name: "IX_PropertyComments_PropertyId",
                table: "PropertyComments",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyComments_UserId",
                table: "PropertyComments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyMedia_PropertyId",
                table: "PropertyMedia",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyRatings_PropertyId_UserId",
                table: "PropertyRatings",
                columns: new[] { "PropertyId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PropertyRatings_UserId",
                table: "PropertyRatings",
                column: "UserId");

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
                name: "IX_UserActivities_UserId_Type_CreatedAt",
                table: "UserActivities",
                columns: new[] { "UserId", "Type", "CreatedAt" });
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
                name: "PropertyComments");

            migrationBuilder.DropTable(
                name: "PropertyMedia");

            migrationBuilder.DropTable(
                name: "PropertyRatings");

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
