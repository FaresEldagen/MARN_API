using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MARN_API.Migrations
{
    /// <inheritdoc />
    public partial class IntiateDatabase : Migration
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
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
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
                name: "ConnectedAccounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StripeAccountId = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    IsOnboardingComplete = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConnectedAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConnectedAccounts_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
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
                    IsShared = table.Column<bool>(type: "bit", nullable: false),
                    MaxOccupants = table.Column<int>(type: "int", nullable: false),
                    Bedrooms = table.Column<int>(type: "int", nullable: false),
                    Beds = table.Column<int>(type: "int", nullable: false),
                    Bathrooms = table.Column<int>(type: "int", nullable: false),
                    Views = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RentalUnit = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Availability = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
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
                    ReportableId = table.Column<long>(type: "bigint", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
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
                    Smoking = table.Column<bool>(type: "bit", nullable: true),
                    Pets = table.Column<bool>(type: "bit", nullable: true),
                    SleepSchedule = table.Column<int>(type: "int", nullable: false),
                    EducationLevel = table.Column<int>(type: "int", nullable: false),
                    FieldOfStudy = table.Column<int>(type: "int", nullable: false),
                    NoiseTolerance = table.Column<int>(type: "int", nullable: true),
                    GuestsFrequency = table.Column<int>(type: "int", nullable: false),
                    WorkSchedule = table.Column<int>(type: "int", nullable: false),
                    SharingLevel = table.Column<int>(type: "int", nullable: false),
                    BudgetRangeMin = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BudgetRangeMax = table.Column<decimal>(type: "decimal(18,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoommatePreferences", x => x.Id);
                    table.CheckConstraint("CK_RoommatePreference_Budget", "[BudgetRangeMax] IS NULL OR [BudgetRangeMin] IS NULL OR [BudgetRangeMax] >= [BudgetRangeMin]");
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
                    Status = table.Column<int>(type: "int", nullable: false),
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
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractNumber = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false),
                    LeaseStartDate = table.Column<DateOnly>(type: "date", nullable: true),
                    LeaseEndDate = table.Column<DateOnly>(type: "date", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(260)", maxLength: 260, nullable: false),
                    FileBytes = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    Hash = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    AnchoredAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OtsFileBytes = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MerkleRoot = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RenterSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerSignature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AnchoringStatus = table.Column<int>(type: "int", nullable: false),
                    SignedByRenterAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SignedByOwnerAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentFrequency = table.Column<int>(type: "int", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CancellationReason = table.Column<string>(type: "nvarchar(max)", nullable: true)
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
                name: "RentalTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    StripeSessionId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PaymentId = table.Column<long>(type: "bigint", nullable: true),
                    ContractId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RentalTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RentalTransactions_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.CheckConstraint("CK_Review_Rating", "[Rating] >= 1 AND [Rating] <= 5");
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reviews_Properties_PropertyId",
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
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContractId = table.Column<long>(type: "bigint", nullable: true),
                    StripeSessionId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AmountTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PlatformFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OwnerAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RenterId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RenterEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropertyId = table.Column<long>(type: "bigint", nullable: false),
                    OwnerStripeAccountId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaidAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvailableAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentIntentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiptUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_AspNetUsers_RenterId",
                        column: x => x.RenterId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Payments_Contracts_ContractId",
                        column: x => x.ContractId,
                        principalTable: "Contracts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Payments_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                columns: new[] { "Id", "AccessFailedCount", "AccountStatus", "ArabicAddress", "ArabicFullName", "BackIdPhoto", "Bio", "ConcurrencyStamp", "Country", "CreatedAt", "DateOfBirth", "DeletedAt", "Discriminator", "Email", "EmailConfirmed", "FirstName", "FrontIdPhoto", "Gender", "Language", "LastName", "LockoutEnabled", "LockoutEnd", "NationalIDNumber", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImage", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 0, 2, "123 شارع النيل، القاهرة", "رينتر ألفا", "/images/idCards/b8ee0c84-7a46-457d-a6d5-9696166b3c87.jpg", null, "SEED-RENTER-A-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Renter", "renter.a@example.com", true, "Renter", "/images/idCards/95c1567c-357c-4c0a-b711-e0ba27c1a96f.jpg", 1, 1, "Alpha", false, null, "12345678901234", "RENTER.A@EXAMPLE.COM", "RENTER.A@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, "/images/profiles/78e645e4-7c92-4cdc-b3bc-11a8f4ef796c.png", "SEED-RENTER-A-SECURITY-STAMP", false, "renter.a@example.com" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 0, 2, "456 شارع المعادي، القاهرة", "رينتر بيتا", "/images/idCards/0b2b1890-82ff-4459-be9a-6dc65971849a.jpg", null, "SEED-RENTER-B-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Renter", "renter.b@example.com", true, "Renter", "/images/idCards/f9797aa8-46ce-4dbb-ad14-2a521ed962fc.jpg", 2, 0, "Beta", false, null, "23456789012345", "RENTER.B@EXAMPLE.COM", "RENTER.B@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-RENTER-B-SECURITY-STAMP", false, "renter.b@example.com" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 0, 2, null, null, null, null, "SEED-RENTER-C-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Renter", "renter.c@example.com", true, "Renter", null, 1, 0, "Gamma", false, null, null, "RENTER.C@EXAMPLE.COM", "RENTER.C@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-RENTER-C-SECURITY-STAMP", false, "renter.c@example.com" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), 0, 2, null, null, null, null, "SEED-OWNER-X-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Owner", "owner.x@example.com", true, "Owner", null, 1, 0, "X", false, null, null, "OWNER.X@EXAMPLE.COM", "OWNER.X@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-OWNER-X-SECURITY-STAMP", false, "owner.x@example.com" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), 0, 2, null, null, null, null, "SEED-OWNER-Y-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Owner", "owner.y@example.com", true, "Owner", null, 2, 0, "Y", false, null, null, "OWNER.Y@EXAMPLE.COM", "OWNER.Y@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-OWNER-Y-SECURITY-STAMP", false, "owner.y@example.com" },
                    { new Guid("66666666-6666-6666-6666-666666666666"), 0, 2, null, null, null, null, "SEED-OWNER-Z-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Owner", "owner.z@example.com", true, "Owner", null, 1, 0, "Z", false, null, null, "OWNER.Z@EXAMPLE.COM", "OWNER.Z@EXAMPLE.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, false, null, "SEED-OWNER-Z-SECURITY-STAMP", false, "owner.z@example.com" },
                    { new Guid("99999999-9999-9999-9999-999999999999"), 0, 2, null, null, null, null, "SEED-ADMIN-CONCURRENCY-STAMP", 1, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Admin", "admin@marn.com", true, "System", null, 0, 0, "Admin", false, null, null, "ADMIN@MARN.COM", "ADMIN@MARN.COM", "AQAAAAIAAYagAAAAEM0BKYvM1Frqg562lK6yise79LW/u17GHrDxW01Y9TICzOxotl6+yOY+VhgcZQowlg==", null, true, null, "SEED-ADMIN-SECURITY-STAMP", false, "admin@marn.com" }
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
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("11111111-1111-1111-1111-111111111111") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("22222222-2222-2222-2222-222222222222") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("33333333-3333-3333-3333-333333333333") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("44444444-4444-4444-4444-444444444444") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("55555555-5555-5555-5555-555555555555") },
                    { new Guid("11111111-1111-1111-1111-111111111111"), new Guid("66666666-6666-6666-6666-666666666666") },
                    { new Guid("22222222-2222-2222-2222-222222222222"), new Guid("66666666-6666-6666-6666-666666666666") },
                    { new Guid("33333333-3333-3333-3333-333333333333"), new Guid("99999999-9999-9999-9999-999999999999") }
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "Content", "ReadAt", "ReceiverId", "SenderId", "SentAt" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000001"), "XB+UQj6hKk23omCXxH8uwFxZpOCQjhe1tRbMbKMHUIKitggz1H61tTuCsIyQwnDRBEWtEIP3n24n1DyxJMAPTuWIvOprIjOmfp48oVxQa6M=", new DateTime(2025, 3, 20, 10, 30, 0, 0, DateTimeKind.Utc), new Guid("44444444-4444-4444-4444-444444444444"), new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 3, 20, 10, 0, 0, 0, DateTimeKind.Utc) },
                    { new Guid("00000000-0000-0000-0000-000000000002"), "E8jOydWqRhQPRv/E1P+cXgNPhEczTZ62c8OsZm62YoKZnffb6X6KXosOMw92CvheYLt5FO58PHhnweOYeJRQ6A==", null, new Guid("11111111-1111-1111-1111-111111111111"), new Guid("44444444-4444-4444-4444-444444444444"), new DateTime(2025, 3, 20, 11, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                table: "Notifications",
                columns: new[] { "Id", "ActionId", "ActionType", "Body", "CreatedAt", "Data", "ReadAt", "Title", "Type", "UserId", "UserType" },
                values: new object[,]
                {
                    { 6001L, null, 4, "Your next rent payment is due soon.", new DateTime(2025, 4, 5, 0, 0, 0, 0, DateTimeKind.Utc), "{\"propertyName\":\"Cozy Seed Apartment\"}", null, "Upcoming Payment Due", 8, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6002L, null, 4, "Your booking request has been accepted.", new DateTime(2025, 4, 6, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Booking Request Update", 4, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6003L, null, null, "Thanks for signing up!", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 3, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Welcome to the platform", 0, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6004L, "44444444-4444-4444-4444-444444444444", 2, "You have a new message from the owner.", new DateTime(2025, 4, 7, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "New Message", 1, new Guid("11111111-1111-1111-1111-111111111111"), 1 },
                    { 6005L, null, 3, "Add more details to your profile to get better recommendations.", new DateTime(2025, 3, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 3, 4, 0, 0, 0, 0, DateTimeKind.Utc), "Complete Your Profile", 0, new Guid("22222222-2222-2222-2222-222222222222"), 1 },
                    { 6006L, "1002", 1, "A renter submitted a booking request for one of your properties.", new DateTime(2025, 4, 8, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "New booking request", 2, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6007L, null, 5, "A rent payment was successfully processed.", new DateTime(2025, 4, 9, 0, 0, 0, 0, DateTimeKind.Utc), "{\"amount\":\"1200\", \"currency\":\"USD\"}", null, "Payment received", 12, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6008L, null, 3, "Complete your listing details to attract more renters.", new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), "Welcome, property owner", 0, new Guid("44444444-4444-4444-4444-444444444444"), 2 },
                    { 6009L, null, 4, "Your next rent payment for Cozy Seed Apartment is due soon.", new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Rent Payment Due Soon", 8, new Guid("66666666-6666-6666-6666-666666666666"), 1 },
                    { 6010L, null, 4, "Your booking request for Seed Studio Flat has been submitted.", new DateTime(2025, 4, 11, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Booking Submitted", 4, new Guid("66666666-6666-6666-6666-666666666666"), 1 },
                    { 6011L, null, null, "Thanks for joining MARN! Explore properties near you.", new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 2, 6, 0, 0, 0, 0, DateTimeKind.Utc), "Welcome to MARN", 0, new Guid("66666666-6666-6666-6666-666666666666"), 1 },
                    { 6012L, null, 5, "Luxury Seed Villa is now visible to renters.", new DateTime(2025, 4, 12, 0, 0, 0, 0, DateTimeKind.Utc), null, null, "Your property is live", 0, new Guid("66666666-6666-6666-6666-666666666666"), 2 },
                    { 6013L, null, 3, "Set up your payout details to start receiving rent payments.", new DateTime(2025, 2, 5, 0, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2025, 2, 7, 0, 0, 0, 0, DateTimeKind.Utc), "Welcome, property owner", 0, new Guid("66666666-6666-6666-6666-666666666666"), 2 }
                });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Address", "Availability", "Bathrooms", "Bedrooms", "Beds", "CreatedAt", "DeletedAt", "Description", "IsActive", "IsShared", "Latitude", "Longitude", "MaxOccupants", "OwnerId", "Price", "RentalUnit", "Status", "Title", "Type", "Views" },
                values: new object[,]
                {
                    { 1001L, "123 Seed Street, Cairo", 0, 1, 2, 3, new DateTime(2025, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), null, "A cozy seeded apartment suitable for testing active rentals.", true, false, 30.0444, 31.235700000000001, 3, new Guid("44444444-4444-4444-4444-444444444444"), 5000m, 1, 1, "Cozy Seed Apartment", 0, 5 },
                    { 1002L, "456 Integration Avenue, Cairo", 0, 1, 1, 1, new DateTime(2025, 2, 2, 0, 0, 0, 0, DateTimeKind.Utc), null, "A modern loft used for pending booking and payments tests.", true, false, 30.050000000000001, 31.239999999999998, 2, new Guid("44444444-4444-4444-4444-444444444444"), 7500m, 1, 1, "Modern Seed Loft", 0, 3 },
                    { 1003L, "789 Scenario Road, Cairo", 0, 1, 1, 1, new DateTime(2025, 2, 3, 0, 0, 0, 0, DateTimeKind.Utc), null, "A small studio property used for saved properties and pending bookings.", true, false, 30.059999999999999, 31.245000000000001, 1, new Guid("44444444-4444-4444-4444-444444444444"), 3500m, 1, 1, "Seed Studio Flat", 4, 1 },
                    { 1004L, "321 Elite Boulevard, Cairo", 0, 3, 4, 5, new DateTime(2025, 2, 4, 0, 0, 0, 0, DateTimeKind.Utc), null, "A luxury villa owned by the dual-role Owner Z for owner dashboard testing.", true, false, 30.07, 31.25, 6, new Guid("66666666-6666-6666-6666-666666666666"), 15000m, 1, 1, "Luxury Seed Villa", 3, 12 }
                });

            migrationBuilder.InsertData(
                table: "Reports",
                columns: new[] { "Id", "CreatedAt", "Reason", "ReportableId", "ReportableType", "ReporterId", "ReviewedAt", "ReviewerId", "Status" },
                values: new object[] { 1L, new DateTime(2025, 3, 15, 0, 0, 0, 0, DateTimeKind.Utc), "Misleading information about the property.", 1001L, 1, new Guid("11111111-1111-1111-1111-111111111111"), null, new Guid("99999999-9999-9999-9999-999999999999"), 0 });

            migrationBuilder.InsertData(
                table: "RoommatePreferences",
                columns: new[] { "Id", "BudgetRangeMax", "BudgetRangeMin", "EducationLevel", "FieldOfStudy", "GuestsFrequency", "NoiseTolerance", "Pets", "RoommatePreferencesEnabled", "SharingLevel", "SleepSchedule", "Smoking", "UserId", "WorkSchedule" },
                values: new object[,]
                {
                    { 1L, 6000m, 3000m, 2, 1, 2, 3, true, true, 3, 1, false, new Guid("11111111-1111-1111-1111-111111111111"), 2 },
                    { 2L, 4500m, 2000m, 2, 5, 4, 5, false, true, 3, 2, true, new Guid("22222222-2222-2222-2222-222222222222"), 5 }
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
                table: "BookingRequests",
                columns: new[] { "Id", "CreatedAt", "EndDate", "PropertyId", "RenterId", "StartDate", "Status" },
                values: new object[,]
                {
                    { 5001L, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1002L, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2026, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 5002L, new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 10, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1002L, new Guid("11111111-1111-1111-1111-111111111111"), new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1 },
                    { 5003L, new DateTime(2025, 4, 2, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1003L, new Guid("22222222-2222-2222-2222-222222222222"), new DateTime(2026, 2, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0 },
                    { 5004L, new DateTime(2025, 4, 10, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 8, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1003L, new Guid("66666666-6666-6666-6666-666666666666"), new DateTime(2026, 7, 1, 0, 0, 0, 0, DateTimeKind.Utc), 0 }
                });

            migrationBuilder.InsertData(
                table: "PropertyAmenities",
                columns: new[] { "Id", "Amenity", "PropertyId" },
                values: new object[,]
                {
                    { 1L, 0, 1001L },
                    { 2L, 2, 1001L },
                    { 3L, 4, 1001L },
                    { 4L, 0, 1002L },
                    { 5L, 7, 1002L },
                    { 6L, 8, 1002L },
                    { 7L, 0, 1003L },
                    { 8L, 5, 1003L },
                    { 9L, 0, 1004L },
                    { 10L, 2, 1004L },
                    { 11L, 9, 1004L },
                    { 12L, 10, 1004L },
                    { 13L, 1, 1004L }
                });

            migrationBuilder.InsertData(
                table: "PropertyMedia",
                columns: new[] { "Id", "IsPrimary", "Path", "PropertyId" },
                values: new object[,]
                {
                    { 2001L, true, "/images/seed/property1-main.jpg", 1001L },
                    { 2002L, true, "/images/seed/property2-main.jpg", 1002L },
                    { 2003L, true, "/images/seed/property3-main.jpg", 1003L },
                    { 2004L, true, "/images/seed/property4-main.jpg", 1004L }
                });

            migrationBuilder.InsertData(
                table: "PropertyRules",
                columns: new[] { "Id", "PropertyId", "Rule" },
                values: new object[,]
                {
                    { 1L, 1001L, "No Smoking inside the apartment." },
                    { 2L, 1001L, "No parties or loud music after 11 PM." },
                    { 3L, 1002L, "Pets are not allowed." },
                    { 4L, 1003L, "Single occupancy only." },
                    { 5L, 1004L, "Respect the neighbors." },
                    { 6L, 1004L, "Smoking allowed only in the balcony." }
                });

            migrationBuilder.InsertData(
                table: "Reviews",
                columns: new[] { "Id", "Comment", "CreatedAt", "PropertyId", "Rating", "UserId" },
                values: new object[,]
                {
                    { 1L, "Great place! Very clean and quiet.", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1001L, 5, new Guid("11111111-1111-1111-1111-111111111111") },
                    { 2L, "Awesome location, but the neighbors were a bit noisy.", new DateTime(2025, 3, 5, 0, 0, 0, 0, DateTimeKind.Utc), 1001L, 4, new Guid("22222222-2222-2222-2222-222222222222") },
                    { 3L, "Superb luxury villa. Highly recommend!", new DateTime(2025, 3, 10, 0, 0, 0, 0, DateTimeKind.Utc), 1004L, 5, new Guid("11111111-1111-1111-1111-111111111111") }
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
                name: "IX_ConnectedAccounts_ApplicationUserId",
                table: "ConnectedAccounts",
                column: "ApplicationUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConnectedAccounts_StripeAccountId",
                table: "ConnectedAccounts",
                column: "StripeAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_ContractNumber",
                table: "Contracts",
                column: "ContractNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contracts_PropertyId_RenterId_OwnerId",
                table: "Contracts",
                columns: new[] { "PropertyId", "RenterId", "OwnerId" });

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
                name: "IX_Payments_ContractId",
                table: "Payments",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_DueDate",
                table: "Payments",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PropertyId",
                table: "Payments",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_RenterId",
                table: "Payments",
                column: "RenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Status_AvailableAt",
                table: "Payments",
                columns: new[] { "Status", "AvailableAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_StripeSessionId",
                table: "Payments",
                column: "StripeSessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_Availability",
                table: "Properties",
                column: "Availability");

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
                name: "IX_PropertyMedia_PropertyId",
                table: "PropertyMedia",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyRules_PropertyId",
                table: "PropertyRules",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalTransactions_ContractId",
                table: "RentalTransactions",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalTransactions_PaymentId",
                table: "RentalTransactions",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalTransactions_PropertyId",
                table: "RentalTransactions",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_RentalTransactions_RenterId_PropertyId_CreatedAt",
                table: "RentalTransactions",
                columns: new[] { "RenterId", "PropertyId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_RentalTransactions_StripeSessionId",
                table: "RentalTransactions",
                column: "StripeSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReporterId",
                table: "Reports",
                column: "ReporterId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReviewerId",
                table: "Reports",
                column: "ReviewerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_PropertyId_UserId",
                table: "Reviews",
                columns: new[] { "PropertyId", "UserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

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
                name: "ConnectedAccounts");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PropertyAmenities");

            migrationBuilder.DropTable(
                name: "PropertyMedia");

            migrationBuilder.DropTable(
                name: "PropertyRules");

            migrationBuilder.DropTable(
                name: "RentalTransactions");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "RoommatePreferences");

            migrationBuilder.DropTable(
                name: "SavedProperties");

            migrationBuilder.DropTable(
                name: "UserActivities");

            migrationBuilder.DropTable(
                name: "UserDevices");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Contracts");

            migrationBuilder.DropTable(
                name: "Properties");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
