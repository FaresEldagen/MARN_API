using CsvHelper;
using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.Enums;
using MARN_API.Enums.Admin;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using System.Globalization;

namespace MARN_API.Services.Implementations
{
    public class AdminAnalyticsReportService : IAdminAnalyticsReportService
    {
        private const int MaxHistoryPageSize = 100;
        private const int PdfDetailRowLimit = 20;
        private const int CsvDetailRowLimit = 5000;
        private readonly IAdminAnalyticsReportRepo _analyticsReportRepo;
        private readonly IAdminDashboardService _dashboardService;
        private readonly IAdminDetailedStatsRepo _detailedStatsRepo;
        private readonly IWebHostEnvironment _environment;
        private readonly IAppTextLocalizer _localizer;
        private readonly ILogger<AdminAnalyticsReportService> _logger;

        public AdminAnalyticsReportService(
            IAdminAnalyticsReportRepo analyticsReportRepo,
            IAdminDashboardService dashboardService,
            IAdminDetailedStatsRepo detailedStatsRepo,
            IWebHostEnvironment environment,
            IAppTextLocalizer localizer,
            ILogger<AdminAnalyticsReportService> logger)
        {
            _analyticsReportRepo = analyticsReportRepo;
            _dashboardService = dashboardService;
            _detailedStatsRepo = detailedStatsRepo;
            _environment = environment;
            _localizer = localizer;
            _logger = logger;

            QuestPDF.Settings.License = LicenseType.Community;
        }

        private bool IsRightToLeftLayout => CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft;

        public async Task<ServiceResult<AdminAnalyticsReportDetailsDto>> GenerateAsync(Guid adminId, AdminAnalyticsReportGenerateRequestDto request)
        {
            request ??= new AdminAnalyticsReportGenerateRequestDto();

            var validationError = ValidateGenerateRequest(request);
            if (validationError != null)
                return validationError;

            var adminUser = await _analyticsReportRepo.GetAdminUserAsync(adminId);
            if (adminUser == null)
            {
                return ServiceResult<AdminAnalyticsReportDetailsDto>.Fail(
                    "Admin user not found.",
                    resultType: ServiceResultType.NotFound);
            }

            var period = ResolvePeriod(request.Period, request.FromUtc, request.ToUtc);
            if (!period.Success)
                return ServiceResult<AdminAnalyticsReportDetailsDto>.Fail(period.Message!, resultType: period.ResultType);

            var detailLimit = request.Format == AdminAnalyticsReportFormat.Csv ? CsvDetailRowLimit : PdfDetailRowLimit;
            var bundleResult = await BuildBundleAsync(request.Scope, period.Data!, detailLimit);
            if (!bundleResult.Success)
                return ServiceResult<AdminAnalyticsReportDetailsDto>.Fail(bundleResult.Message!, resultType: bundleResult.ResultType);

            var generatedAt = DateTime.UtcNow;
            var fileExtension = request.Format == AdminAnalyticsReportFormat.Pdf ? "pdf" : "csv";
            var fileName = BuildFileName(request.Scope, request.Format, period.Data!, generatedAt);
            var contentType = request.Format == AdminAnalyticsReportFormat.Pdf ? "application/pdf" : "text/csv";

            byte[] fileBytes;
            if (request.Format == AdminAnalyticsReportFormat.Pdf)
            {
                fileBytes = GeneratePdf(bundleResult.Data!, request.Scope, period.Data!, generatedAt, adminUser);
            }
            else
            {
                fileBytes = GenerateCsv(bundleResult.Data!, request.Scope);
            }

            var relativeStoredPath = Path.Combine("reports", "admin-analytics", fileName).Replace("\\", "/");
            var absoluteStoredPath = GetAbsoluteReportsFolderPath();
            Directory.CreateDirectory(absoluteStoredPath);

            var fullFilePath = Path.Combine(absoluteStoredPath, fileName);
            await File.WriteAllBytesAsync(fullFilePath, fileBytes);

            var report = new Models.AdminAnalyticsReport
            {
                GeneratedByAdminId = adminId,
                Scope = request.Scope,
                Format = request.Format,
                RequestedPeriod = period.Data!.Period,
                FromUtc = period.Data.FromUtc,
                ToUtc = period.Data.ToUtc,
                Grouping = period.Data.Grouping,
                FileName = fileName,
                StoredFilePath = relativeStoredPath,
                ContentType = contentType,
                FileSizeBytes = fileBytes.LongLength,
                GeneratedAt = generatedAt
            };

            try
            {
                await _analyticsReportRepo.AddAsync(report);
                await _analyticsReportRepo.SaveChangesAsync();
            }
            catch
            {
                if (File.Exists(fullFilePath))
                    File.Delete(fullFilePath);
                throw;
            }

            _logger.LogInformation(
                "Admin analytics report {ReportId} generated by {AdminId} with scope {Scope} and format {Format}.",
                report.Id,
                adminId,
                report.Scope,
                report.Format);

            return ServiceResult<AdminAnalyticsReportDetailsDto>.Ok(
                MapDetailsDto(report, adminUser.FirstName + " " + adminUser.LastName),
                "Analytics report generated successfully.",
                code: "ZZ_ADMIN_ANALYTICS_REPORT_GENERATED_SUCCESSFULLY");
        }

        public async Task<ServiceResult<PagedResult<AdminAnalyticsReportListItemDto>>> GetReportsAsync(AdminAnalyticsReportQueryDto query)
        {
            query ??= new AdminAnalyticsReportQueryDto();
            if (query.PageNumber < 1)
                query.PageNumber = 1;
            if (query.PageSize < 1)
                query.PageSize = 20;
            if (query.PageSize > MaxHistoryPageSize)
                query.PageSize = MaxHistoryPageSize;

            var reports = await _analyticsReportRepo.GetReportsAsync(query);
            LocalizeReportItems(reports.Items);
            return ServiceResult<PagedResult<AdminAnalyticsReportListItemDto>>.Ok(reports);
        }

        public async Task<ServiceResult<AdminAnalyticsReportDetailsDto>> GetReportAsync(long reportId)
        {
            var report = await _analyticsReportRepo.GetByIdAsync(reportId);
            if (report == null)
            {
                return ServiceResult<AdminAnalyticsReportDetailsDto>.Fail(
                    "Analytics report not found.",
                    resultType: ServiceResultType.NotFound);
            }

            var adminName = $"{report.GeneratedByAdmin.FirstName} {report.GeneratedByAdmin.LastName}".Trim();
            return ServiceResult<AdminAnalyticsReportDetailsDto>.Ok(MapDetailsDto(report, adminName));
        }

        public async Task<ServiceResult<AdminAnalyticsReportDownloadDto>> DownloadAsync(long reportId)
        {
            var report = await _analyticsReportRepo.GetByIdAsync(reportId);
            if (report == null)
            {
                return ServiceResult<AdminAnalyticsReportDownloadDto>.Fail(
                    "Analytics report not found.",
                    resultType: ServiceResultType.NotFound);
            }

            var fullPath = Path.Combine(GetWebRootPath(), report.StoredFilePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (!File.Exists(fullPath))
            {
                return ServiceResult<AdminAnalyticsReportDownloadDto>.Fail(
                    "Stored report file was not found on disk.",
                    resultType: ServiceResultType.NotFound);
            }

            var fileBytes = await File.ReadAllBytesAsync(fullPath);
            return ServiceResult<AdminAnalyticsReportDownloadDto>.Ok(new AdminAnalyticsReportDownloadDto
            {
                FileName = report.FileName,
                ContentType = report.ContentType,
                FileBytes = fileBytes
            });
        }

        private ServiceResult<AdminAnalyticsReportDetailsDto>? ValidateGenerateRequest(AdminAnalyticsReportGenerateRequestDto request)
        {
            if (request.Format == AdminAnalyticsReportFormat.Csv && request.Scope == AdminAnalyticsReportScope.Full)
            {
                return ServiceResult<AdminAnalyticsReportDetailsDto>.Fail(
                    "CSV exports support overview, users, properties, contracts, and revenue scopes individually. Use PDF for the full combined report.",
                    resultType: ServiceResultType.BadRequest);
            }

            return null;
        }

        private async Task<ServiceResult<AnalyticsExportBundle>> BuildBundleAsync(
            AdminAnalyticsReportScope scope,
            ResolvedPeriod period,
            int detailLimit)
        {
            var bundle = new AnalyticsExportBundle
            {
                Scope = scope,
                Period = period
            };

            var needsOverview = scope == AdminAnalyticsReportScope.Overview || scope == AdminAnalyticsReportScope.Full || scope != AdminAnalyticsReportScope.Overview;
            if (needsOverview)
            {
                var overview = await _dashboardService.GetOverviewAsync();
                if (!overview.Success || overview.Data == null)
                {
                    return ServiceResult<AnalyticsExportBundle>.Fail(
                        overview.Message ?? "Failed to load dashboard overview data.",
                        resultType: overview.ResultType);
                }

                bundle.Overview = overview.Data;
            }

            if (scope is AdminAnalyticsReportScope.Users or AdminAnalyticsReportScope.Full)
            {
                var usersQuery = new AdminDetailedUsersQueryDto
                {
                    Period = ToDetailedStatsPeriodValue(period.Period),
                    FromUtc = period.FromUtc,
                    ToUtc = period.ToUtc,
                    PageNumber = 1,
                    PageSize = detailLimit,
                    IncludeDeleted = true
                };

                bundle.Users = await _detailedStatsRepo.GetUsersAsync(usersQuery, period.FromUtc, period.ToUtc, period.GroupByDay);
            }

            if (scope is AdminAnalyticsReportScope.Properties or AdminAnalyticsReportScope.Full)
            {
                var propertiesQuery = new AdminDetailedPropertiesQueryDto
                {
                    Period = ToDetailedStatsPeriodValue(period.Period),
                    FromUtc = period.FromUtc,
                    ToUtc = period.ToUtc,
                    PageNumber = 1,
                    PageSize = detailLimit,
                    IncludeDeleted = true
                };

                bundle.Properties = await _detailedStatsRepo.GetPropertiesAsync(propertiesQuery, period.FromUtc, period.ToUtc);
            }

            if (scope is AdminAnalyticsReportScope.Contracts or AdminAnalyticsReportScope.Full)
            {
                var contractsQuery = new AdminDetailedContractsQueryDto
                {
                    Period = ToDetailedStatsPeriodValue(period.Period),
                    FromUtc = period.FromUtc,
                    ToUtc = period.ToUtc,
                    PageNumber = 1,
                    PageSize = detailLimit
                };

                bundle.Contracts = await _detailedStatsRepo.GetContractsAsync(contractsQuery, period.FromUtc, period.ToUtc);
            }

            if (scope is AdminAnalyticsReportScope.Revenue or AdminAnalyticsReportScope.Full)
            {
                var revenueQuery = new AdminDetailedRevenueQueryDto
                {
                    Period = ToDetailedStatsPeriodValue(period.Period),
                    FromUtc = period.FromUtc,
                    ToUtc = period.ToUtc,
                    PageNumber = 1,
                    PageSize = detailLimit
                };

                bundle.Revenue = await _detailedStatsRepo.GetRevenueAsync(revenueQuery, period.FromUtc, period.ToUtc, period.GroupByDay);
            }

            return ServiceResult<AnalyticsExportBundle>.Ok(bundle);
        }

        private byte[] GenerateCsv(AnalyticsExportBundle bundle, AdminAnalyticsReportScope scope)
        {
            using var writer = new StringWriter(CultureInfo.InvariantCulture);
            using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);

            switch (scope)
            {
                case AdminAnalyticsReportScope.Overview:
                    WriteCsv(
                        csv,
                        [T("Metric"), T("Value")],
                        BuildOverviewCsvRows(bundle).Select(row => new object?[] { row.Metric, row.Value }));
                    break;
                case AdminAnalyticsReportScope.Users:
                    WriteCsv(
                        csv,
                        [
                            T("User ID"),
                            T("Full Name"),
                            T("Email"),
                            T("Account Status"),
                            T("Is Deleted"),
                            T("Created At"),
                            T("Roles"),
                            T("Owned Properties Count"),
                            T("Active Properties Count"),
                            T("Renter Contracts Count"),
                            T("Owner Contracts Count"),
                            T("Active Contracts Count"),
                            T("Cancelled Contracts Count"),
                            T("Payments Made Count"),
                            T("Payments Received Count"),
                            T("Total Paid Amount"),
                            T("Total Received Amount"),
                            T("Reports Submitted Count"),
                            T("Reports Against User Count")
                        ],
                        bundle.Users!.Users.Items.Select(x => new object?[]
                        {
                            x.UserId,
                            x.FullName,
                            x.Email,
                            _localizer.GetEnumDisplayName(x.AccountStatus),
                            x.IsDeleted,
                            x.CreatedAt,
                            string.Join(", ", GetRoleDisplayNames(x.Roles)),
                            x.OwnedPropertiesCount,
                            x.ActivePropertiesCount,
                            x.RenterContractsCount,
                            x.OwnerContractsCount,
                            x.ActiveContractsCount,
                            x.CancelledContractsCount,
                            x.PaymentsMadeCount,
                            x.PaymentsReceivedCount,
                            x.TotalPaidAmount,
                            x.TotalReceivedAmount,
                            x.ReportsSubmittedCount,
                            x.ReportsAgainstUserCount
                        }));
                    break;
                case AdminAnalyticsReportScope.Properties:
                    WriteCsv(
                        csv,
                        [
                            T("Property ID"),
                            T("Title"),
                            T("Owner ID"),
                            T("Owner Name"),
                            T("Status"),
                            T("Type"),
                            T("City"),
                            T("Governorate"),
                            T("Price"),
                            T("Is Active"),
                            T("Is Deleted"),
                            T("Created At")
                        ],
                        bundle.Properties!.Properties.Items.Select(x => new object?[]
                        {
                            x.PropertyId,
                            x.Title,
                            x.OwnerId,
                            x.OwnerName,
                            _localizer.GetEnumDisplayName(x.Status),
                            _localizer.GetEnumDisplayName(x.Type),
                            GetLocationDisplayName<Enums.Property.City>(x.City),
                            GetLocationDisplayName<Enums.Property.Governorate>(x.Governorate),
                            x.Price,
                            x.IsActive,
                            x.IsDeleted,
                            x.CreatedAt
                        }));
                    break;
                case AdminAnalyticsReportScope.Contracts:
                    WriteCsv(
                        csv,
                        [
                            T("Contract ID"),
                            T("Status"),
                            T("Created At"),
                            T("Lease Start Date"),
                            T("Lease End Date"),
                            T("Total Contract Amount"),
                            T("Payment Frequency"),
                            T("Property ID"),
                            T("Property Title"),
                            T("Owner ID"),
                            T("Owner Name"),
                            T("Renter ID"),
                            T("Renter Name")
                        ],
                        bundle.Contracts!.Contracts.Items.Select(x => new object?[]
                        {
                            x.ContractId,
                            _localizer.GetEnumDisplayName(x.Status),
                            x.CreatedAt,
                            x.LeaseStartDate,
                            x.LeaseEndDate,
                            x.TotalContractAmount,
                            GetPaymentFrequencyDisplayName(x.PaymentFrequency),
                            x.PropertyId,
                            x.PropertyTitle,
                            x.OwnerId,
                            x.OwnerName,
                            x.RenterId,
                            x.RenterName
                        }));
                    break;
                case AdminAnalyticsReportScope.Revenue:
                    WriteCsv(
                        csv,
                        [
                            T("Payment ID"),
                            T("Contract ID"),
                            T("Payment Schedule ID"),
                            T("Status"),
                            T("Amount Total"),
                            T("Platform Fee"),
                            T("Owner Amount"),
                            T("Paid At"),
                            T("Available At"),
                            T("Currency"),
                            T("Property ID"),
                            T("Property Title"),
                            T("Owner ID"),
                            T("Owner Name"),
                            T("Renter ID"),
                            T("Renter Name")
                        ],
                        bundle.Revenue!.Payments.Items.Select(x => new object?[]
                        {
                            x.PaymentId,
                            x.ContractId,
                            x.PaymentScheduleId,
                            _localizer.GetEnumDisplayName(x.Status),
                            x.AmountTotal,
                            x.PlatformFee,
                            x.OwnerAmount,
                            x.PaidAt,
                            x.AvailableAt,
                            x.Currency,
                            x.PropertyId,
                            x.PropertyTitle,
                            x.OwnerId,
                            x.OwnerName,
                            x.RenterId,
                            x.RenterName
                        }));
                    break;
            }

            return System.Text.Encoding.UTF8.GetBytes(writer.ToString());
        }

        private byte[] GeneratePdf(
            AnalyticsExportBundle bundle,
            AdminAnalyticsReportScope scope,
            ResolvedPeriod period,
            DateTime generatedAt,
            Models.ApplicationUser adminUser)
        {
            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    ConfigurePage(page, scope, period, generatedAt, adminUser);

                    page.Content().Column(column =>
                    {
                        column.Spacing(16);
                        ComposeExecutiveSnapshot(column, bundle);

                        if (scope == AdminAnalyticsReportScope.Overview)
                        {
                            ComposeOverviewPeriodNote(column, period);
                        }

                        if (scope is AdminAnalyticsReportScope.Users or AdminAnalyticsReportScope.Full)
                            ComposeUsersSection(column, bundle.Users!);

                        if (scope is AdminAnalyticsReportScope.Properties or AdminAnalyticsReportScope.Full)
                            ComposePropertiesSection(column, bundle.Properties!);

                        if (scope is AdminAnalyticsReportScope.Contracts or AdminAnalyticsReportScope.Full)
                            ComposeContractsSection(column, bundle.Contracts!);

                        if (scope is AdminAnalyticsReportScope.Revenue or AdminAnalyticsReportScope.Full)
                            ComposeRevenueSection(column, bundle.Revenue!);
                    });
                });
            });

            return document.GeneratePdf();
        }

        private void ConfigurePage(
            PageDescriptor page,
            AdminAnalyticsReportScope scope,
            ResolvedPeriod period,
            DateTime generatedAt,
            Models.ApplicationUser adminUser)
        {
            page.Margin(28);
            page.DefaultTextStyle(x => x.FontSize(10));
            page.Header().Column(column =>
            {
                AlignForLayout(column.Item()).Text(T("Admin {0} Report", _localizer.GetEnumDisplayName(scope)))
                    .SemiBold()
                    .FontSize(20);
                AlignForLayout(column.Item()).Text(T("Period: {0}", FormatPeriod(period)));
                AlignForLayout(column.Item()).Text(T("Generated at: {0}", generatedAt.ToString("u", CultureInfo.InvariantCulture)));
                AlignForLayout(column.Item()).Text(T("Generated by: {0}", $"{adminUser.FirstName} {adminUser.LastName}".Trim()));
            });

            page.Footer().AlignCenter().Text(text =>
            {
                text.Span(T("Page"));
                text.Span(" ");
                text.CurrentPageNumber();
                text.Span(" / ");
                text.TotalPages();
            });
        }

        private void ComposeExecutiveSnapshot(ColumnDescriptor column, AnalyticsExportBundle bundle)
        {
            if (bundle.Overview == null)
                return;

            AlignForLayout(column.Item()).Text(T("Executive Snapshot")).SemiBold().FontSize(14);
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                AddKeyValueRow(table, T("Total Users"), bundle.Overview.TotalUsers.Value.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, T("Total Properties"), bundle.Overview.TotalProperties.Value.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, T("Pending Verifications"), bundle.Overview.PendingVerifications.Value.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, T("Total Contracts"), bundle.Overview.TotalContracts.Value.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, T("Total Revenue"), bundle.Overview.RevenueSummary.TotalRevenue.ToString("N2", CultureInfo.InvariantCulture));
                AddKeyValueRow(table, T("Total Sales"), bundle.Overview.RevenueSummary.TotalSales.ToString("N2", CultureInfo.InvariantCulture));
                AddKeyValueRow(table, T("Active Contracts"), bundle.Overview.RevenueSummary.ActiveContracts.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, T("New Users This Month"), bundle.Overview.RevenueSummary.NewUsersThisMonth.ToString(CultureInfo.InvariantCulture));
            });
        }

        private void ComposeOverviewPeriodNote(ColumnDescriptor column, ResolvedPeriod period)
        {
            AlignForLayout(column.Item()).Text(T("This report reflects the current admin snapshot and the selected period context: {0}.", FormatPeriod(period)));
        }

        private void ComposeUsersSection(ColumnDescriptor column, AdminDetailedUsersResponseDto users)
        {
            AlignForLayout(column.Item()).Text(T("Users")).SemiBold().FontSize(14);
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                AddKeyValueRow(table, T("Total Users"), users.TotalUsers.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, T("Deleted Users"), users.DeletedUsers.ToString(CultureInfo.InvariantCulture));
            });

            AlignForLayout(column.Item()).Text(T("Status Breakdown")).SemiBold();
            ComposeThreeColumnTable(
                column,
                [T("Status"), T("Count"), T("Share")],
                users.StatusBreakdown.Select(x => new[]
                {
                    _localizer.GetEnumDisplayName(x.Status),
                    x.Count.ToString(CultureInfo.InvariantCulture),
                    users.TotalUsers == 0 ? "0%" : $"{(x.Count * 100m / users.TotalUsers):N1}%"
                }));

            AlignForLayout(column.Item()).Text(T("Latest Users")).SemiBold();
            ComposeFourColumnTable(
                column,
                [T("User"), T("Status"), T("Created"), T("Roles")],
                users.Users.Items.Select(x => new[]
                {
                    x.FullName,
                    _localizer.GetEnumDisplayName(x.AccountStatus),
                    x.CreatedAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    string.Join(", ", GetRoleDisplayNames(x.Roles))
                }));

            AlignForLayout(column.Item()).Text(T("User Activity Metrics")).SemiBold();
            ComposeSixColumnTable(
                column,
                [T("User"), T("Properties"), T("Contracts"), T("Payments"), T("Amounts"), T("Reports")],
                users.Users.Items.Select(x => new[]
                {
                    x.FullName,
                    T("Owned: {0}, Active: {1}", x.OwnedPropertiesCount, x.ActivePropertiesCount),
                    T("Renter: {0}, Owner: {1}, Active: {2}, Cancelled: {3}", x.RenterContractsCount, x.OwnerContractsCount, x.ActiveContractsCount, x.CancelledContractsCount),
                    T("Made: {0}, Received: {1}", x.PaymentsMadeCount, x.PaymentsReceivedCount),
                    T("Paid: {0}, Received: {1}", x.TotalPaidAmount.ToString("N2", CultureInfo.InvariantCulture), x.TotalReceivedAmount.ToString("N2", CultureInfo.InvariantCulture)),
                    T("Filed: {0}, Against: {1}", x.ReportsSubmittedCount, x.ReportsAgainstUserCount)
                }));
        }

        private void ComposePropertiesSection(ColumnDescriptor column, AdminDetailedPropertiesResponseDto properties)
        {
            AlignForLayout(column.Item()).Text(T("Properties")).SemiBold().FontSize(14);
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                AddKeyValueRow(table, T("Total Properties"), properties.TotalProperties.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, T("Active Properties"), properties.ActiveProperties.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, T("Inactive Properties"), properties.InactiveProperties.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, T("Deleted Properties"), properties.DeletedProperties.ToString(CultureInfo.InvariantCulture));
            });

            AlignForLayout(column.Item()).Text(T("Status Breakdown")).SemiBold();
            ComposeTwoColumnTable(column, [T("Status"), T("Count")], properties.StatusBreakdown.Select(x => new[]
            {
                _localizer.GetEnumDisplayName(x.Status),
                x.Count.ToString(CultureInfo.InvariantCulture)
            }));

            AlignForLayout(column.Item()).Text(T("Latest Properties")).SemiBold();
            ComposeFourColumnTable(
                column,
                [T("Property"), T("Owner"), T("Status"), T("Location")],
                properties.Properties.Items.Select(x => new[]
                {
                    x.Title,
                    x.OwnerName,
                    _localizer.GetEnumDisplayName(x.Status),
                    $"{GetLocationDisplayName<Enums.Property.City>(x.City)}, {GetLocationDisplayName<Enums.Property.Governorate>(x.Governorate)}"
                }));
        }

        private void ComposeContractsSection(ColumnDescriptor column, AdminDetailedContractsResponseDto contracts)
        {
            AlignForLayout(column.Item()).Text(T("Contracts")).SemiBold().FontSize(14);
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                AddKeyValueRow(table, T("Total Contracts"), contracts.TotalContracts.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, T("Total Contract Value"), contracts.TotalContractValue.ToString("N2", CultureInfo.InvariantCulture));
            });

            AlignForLayout(column.Item()).Text(T("Status Breakdown")).SemiBold();
            ComposeTwoColumnTable(column, [T("Status"), T("Count")], contracts.StatusBreakdown.Select(x => new[]
            {
                _localizer.GetEnumDisplayName(x.Status),
                x.Count.ToString(CultureInfo.InvariantCulture)
            }));

            AlignForLayout(column.Item()).Text(T("Latest Contracts")).SemiBold();
            ComposeFourColumnTable(
                column,
                [T("Contract"), T("Property"), T("Owner"), T("Renter")],
                contracts.Contracts.Items.Select(x => new[]
                {
                    x.ContractId.ToString(CultureInfo.InvariantCulture),
                    x.PropertyTitle,
                    x.OwnerName,
                    x.RenterName
                }));
        }

        private void ComposeRevenueSection(ColumnDescriptor column, AdminDetailedRevenueResponseDto revenue)
        {
            AlignForLayout(column.Item()).Text(T("Revenue")).SemiBold().FontSize(14);
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                AddKeyValueRow(table, T("Total Payments"), revenue.TotalPayments.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, T("Total Sales"), revenue.TotalSales.ToString("N2", CultureInfo.InvariantCulture));
                AddKeyValueRow(table, T("Platform Revenue"), revenue.TotalRevenue.ToString("N2", CultureInfo.InvariantCulture));
                AddKeyValueRow(table, T("Owner Payouts"), revenue.TotalOwnerPayouts.ToString("N2", CultureInfo.InvariantCulture));
            });

            AlignForLayout(column.Item()).Text(T("Payment Status Breakdown")).SemiBold();
            ComposeThreeColumnTable(
                column,
                [T("Status"), T("Count"), T("Revenue")],
                revenue.StatusBreakdown.Select(x => new[]
                {
                    _localizer.GetEnumDisplayName(x.Status),
                    x.Count.ToString(CultureInfo.InvariantCulture),
                    x.Revenue.ToString("N2", CultureInfo.InvariantCulture)
                }));

            AlignForLayout(column.Item()).Text(T("Revenue Over Time")).SemiBold();
            ComposeThreeColumnTable(
                column,
                [T("Period"), T("Revenue"), T("Sales")],
                revenue.RevenueOverTime.Select(x => new[]
                {
                    x.Label,
                    x.Revenue.ToString("N2", CultureInfo.InvariantCulture),
                    x.Sales.ToString("N2", CultureInfo.InvariantCulture)
                }));
        }

        private void ComposeTwoColumnTable(ColumnDescriptor column, string[] headers, IEnumerable<string[]> rows)
        {
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                AddHeaderRow(table, headers);
                foreach (var row in rows)
                    AddRow(table, row);
            });
        }

        private void ComposeThreeColumnTable(ColumnDescriptor column, string[] headers, IEnumerable<string[]> rows)
        {
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                AddHeaderRow(table, headers);
                foreach (var row in rows)
                    AddRow(table, row);
            });
        }

        private void ComposeFourColumnTable(ColumnDescriptor column, string[] headers, IEnumerable<string[]> rows)
        {
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                AddHeaderRow(table, headers);
                foreach (var row in rows)
                    AddRow(table, row);
            });
        }

        private IEnumerable<string> GetRoleDisplayNames(IEnumerable<string> roles)
        {
            return roles.Select(GetRoleDisplayName);
        }

        private string GetRoleDisplayName(string role)
        {
            var roleKey = $"ROLE_{role.Replace(' ', '_')}";
            return _localizer.GetOrFallback(roleKey, role);
        }

        private void ComposeSixColumnTable(ColumnDescriptor column, string[] headers, IEnumerable<string[]> rows)
        {
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                AddHeaderRow(table, headers);
                foreach (var row in rows)
                    AddRow(table, row);
            });
        }

        private void AddKeyValueRow(TableDescriptor table, string key, string value)
        {
            var cells = IsRightToLeftLayout ? new[] { value, key } : new[] { key, value };
            foreach (var cell in cells)
            {
                AddCell(table, cell, isHeader: false);
            }
        }

        private void AddHeaderRow(TableDescriptor table, IEnumerable<string> headers)
        {
            foreach (var header in OrderForLayout(headers))
            {
                AddCell(table, header, isHeader: true);
            }
        }

        private void AddRow(TableDescriptor table, IEnumerable<string> cells)
        {
            foreach (var cell in OrderForLayout(cells))
            {
                AddCell(table, cell ?? string.Empty, isHeader: false);
            }
        }

        private void AddCell(TableDescriptor table, string text, bool isHeader)
        {
            var container = table.Cell().Element(isHeader ? HeaderCellStyle : CellStyle);
            var aligned = IsRightToLeftLayout ? container.AlignRight() : container.AlignLeft();
            var textDescriptor = aligned.Text(text);
            if (isHeader)
            {
                textDescriptor.SemiBold();
            }
        }

        private static IContainer HeaderCellStyle(IContainer container)
        {
            return container
                .Border(1)
                .Padding(4)
                .Background("#E5E7EB");
        }

        private static IContainer CellStyle(IContainer container)
        {
            return container
                .Border(1)
                .Padding(4);
        }

        private IEnumerable<string> OrderForLayout(IEnumerable<string> values)
        {
            var list = values.ToList();
            if (IsRightToLeftLayout)
            {
                list.Reverse();
            }

            return list;
        }

        private IContainer AlignForLayout(IContainer container)
        {
            return IsRightToLeftLayout ? container.AlignRight() : container.AlignLeft();
        }

        private List<OverviewMetricCsvRow> BuildOverviewCsvRows(AnalyticsExportBundle bundle)
        {
            if (bundle.Overview == null)
                return [];

            return
            [
                new OverviewMetricCsvRow(T("Total Users"), bundle.Overview.TotalUsers.Value.ToString(CultureInfo.InvariantCulture)),
                new OverviewMetricCsvRow(T("Total Properties"), bundle.Overview.TotalProperties.Value.ToString(CultureInfo.InvariantCulture)),
                new OverviewMetricCsvRow(T("Pending Verifications"), bundle.Overview.PendingVerifications.Value.ToString(CultureInfo.InvariantCulture)),
                new OverviewMetricCsvRow(T("Total Contracts"), bundle.Overview.TotalContracts.Value.ToString(CultureInfo.InvariantCulture)),
                new OverviewMetricCsvRow(T("Total Revenue"), bundle.Overview.RevenueSummary.TotalRevenue.ToString("N2", CultureInfo.InvariantCulture)),
                new OverviewMetricCsvRow(T("Total Sales"), bundle.Overview.RevenueSummary.TotalSales.ToString("N2", CultureInfo.InvariantCulture)),
                new OverviewMetricCsvRow(T("New Users This Month"), bundle.Overview.RevenueSummary.NewUsersThisMonth.ToString(CultureInfo.InvariantCulture)),
                new OverviewMetricCsvRow(T("Active Contracts"), bundle.Overview.RevenueSummary.ActiveContracts.ToString(CultureInfo.InvariantCulture))
            ];
        }

        private static string BuildFileName(AdminAnalyticsReportScope scope, AdminAnalyticsReportFormat format, ResolvedPeriod period, DateTime generatedAt)
        {
            var extension = format == AdminAnalyticsReportFormat.Pdf ? "pdf" : "csv";
            var periodToken = period.Period == AdminAnalyticsReportPeriod.Custom && period.FromUtc.HasValue && period.ToUtc.HasValue
                ? $"{period.FromUtc:yyyyMMdd}-{period.ToUtc:yyyyMMdd}"
                : period.Period.ToString().ToLowerInvariant();

            return $"admin-{scope.ToString().ToLowerInvariant()}-{periodToken}-{generatedAt:yyyyMMddHHmmss}.{extension}";
        }

        private string FormatPeriod(ResolvedPeriod period)
        {
            return period.Period == AdminAnalyticsReportPeriod.Custom && period.FromUtc.HasValue && period.ToUtc.HasValue
                ? T("{0} to {1}", period.FromUtc.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), period.ToUtc.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture))
                : _localizer.GetEnumDisplayName(period.Period);
        }

        private static string ToDetailedStatsPeriodValue(AdminAnalyticsReportPeriod period)
        {
            return period switch
            {
                AdminAnalyticsReportPeriod.AllTime => "allTime",
                AdminAnalyticsReportPeriod.ThisMonth => "thisMonth",
                AdminAnalyticsReportPeriod.ThisYear => "thisYear",
                AdminAnalyticsReportPeriod.Custom => "custom",
                _ => "thisMonth"
            };
        }

        private static ServiceResult<ResolvedPeriod> ResolvePeriod(AdminAnalyticsReportPeriod period, DateTime? fromUtc, DateTime? toUtc)
        {
            var nowUtc = DateTime.UtcNow;

            if (period == AdminAnalyticsReportPeriod.AllTime)
                return ServiceResult<ResolvedPeriod>.Ok(new ResolvedPeriod(AdminAnalyticsReportPeriod.AllTime, null, null, false));

            if (period == AdminAnalyticsReportPeriod.ThisMonth)
            {
                var start = new DateTime(nowUtc.Year, nowUtc.Month, 1, 0, 0, 0, DateTimeKind.Utc);
                return ServiceResult<ResolvedPeriod>.Ok(new ResolvedPeriod(AdminAnalyticsReportPeriod.ThisMonth, start, nowUtc, true));
            }

            if (period == AdminAnalyticsReportPeriod.ThisYear)
            {
                var start = new DateTime(nowUtc.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                return ServiceResult<ResolvedPeriod>.Ok(new ResolvedPeriod(AdminAnalyticsReportPeriod.ThisYear, start, nowUtc, false));
            }

            if (period == AdminAnalyticsReportPeriod.Custom)
            {
                if (!fromUtc.HasValue || !toUtc.HasValue)
                    return ServiceResult<ResolvedPeriod>.Fail("Custom period requires fromUtc and toUtc.", resultType: ServiceResultType.BadRequest);

                if (fromUtc.Value >= toUtc.Value)
                    return ServiceResult<ResolvedPeriod>.Fail("fromUtc must be earlier than toUtc.", resultType: ServiceResultType.BadRequest);

                var duration = toUtc.Value - fromUtc.Value;
                return ServiceResult<ResolvedPeriod>.Ok(new ResolvedPeriod(AdminAnalyticsReportPeriod.Custom, fromUtc.Value, toUtc.Value, duration.TotalDays <= 31));
            }

            return ServiceResult<ResolvedPeriod>.Fail(
                "Invalid period.",
                resultType: ServiceResultType.BadRequest);
        }

        private AdminAnalyticsReportDetailsDto MapDetailsDto(Models.AdminAnalyticsReport report, string generatedByName)
        {
            return new AdminAnalyticsReportDetailsDto
            {
                ReportId = report.Id,
                Scope = report.Scope,
                ScopeDisplayName = _localizer.GetEnumDisplayName(report.Scope),
                Format = report.Format,
                FormatDisplayName = _localizer.GetEnumDisplayName(report.Format),
                RequestedPeriod = report.RequestedPeriod,
                RequestedPeriodDisplayName = _localizer.GetEnumDisplayName(report.RequestedPeriod),
                FromUtc = report.FromUtc,
                ToUtc = report.ToUtc,
                Grouping = report.Grouping,
                FileName = report.FileName,
                FileSizeBytes = report.FileSizeBytes,
                GeneratedAt = report.GeneratedAt,
                GeneratedByAdminId = report.GeneratedByAdminId,
                GeneratedByAdminName = generatedByName.Trim(),
                ContentType = report.ContentType,
                DownloadUrl = $"/api/admin/analytics-reports/{report.Id}/download"
            };
        }

        private void LocalizeReportItems(IEnumerable<AdminAnalyticsReportListItemDto> items)
        {
            foreach (var item in items)
            {
                item.ScopeDisplayName = _localizer.GetEnumDisplayName(item.Scope);
                item.FormatDisplayName = _localizer.GetEnumDisplayName(item.Format);
                item.RequestedPeriodDisplayName = _localizer.GetEnumDisplayName(item.RequestedPeriod);
            }
        }

        private void WriteCsv(CsvWriter csv, IReadOnlyList<string> headers, IEnumerable<object?[]> rows)
        {
            foreach (var header in headers)
            {
                csv.WriteField(header);
            }

            csv.NextRecord();

            foreach (var row in rows)
            {
                foreach (var cell in row)
                {
                    csv.WriteField(cell);
                }

                csv.NextRecord();
            }
        }

        private string T(string fallback)
        {
            return _localizer.LocalizeMessage(null, fallback);
        }

        private string T(string fallback, params object?[] args)
        {
            return _localizer.LocalizeMessage(null, fallback, null, args);
        }

        private string GetPaymentFrequencyDisplayName(string? rawValue)
        {
            if (!string.IsNullOrWhiteSpace(rawValue) && Enum.TryParse<Enums.Payment.PaymentFrequency>(rawValue, true, out var parsed))
            {
                return _localizer.GetEnumDisplayName(parsed);
            }

            return rawValue ?? string.Empty;
        }

        private string GetLocationDisplayName<TEnum>(string? rawValue) where TEnum : struct, Enum
        {
            if (!string.IsNullOrWhiteSpace(rawValue) && Enum.TryParse<TEnum>(rawValue, true, out var parsed))
            {
                return _localizer.GetEnumDisplayName(parsed);
            }

            return rawValue ?? string.Empty;
        }

        private string GetAbsoluteReportsFolderPath()
        {
            return Path.Combine(GetWebRootPath(), "reports", "admin-analytics");
        }

        private string GetWebRootPath()
        {
            var webRootPath = _environment.WebRootPath;
            if (!string.IsNullOrWhiteSpace(webRootPath))
                return webRootPath;

            return Path.Combine(_environment.ContentRootPath, "wwwroot");
        }

        private sealed class AnalyticsExportBundle
        {
            public AdminAnalyticsReportScope Scope { get; set; }
            public ResolvedPeriod Period { get; set; } = null!;
            public AdminDashboardOverviewDto? Overview { get; set; }
            public AdminDetailedUsersResponseDto? Users { get; set; }
            public AdminDetailedPropertiesResponseDto? Properties { get; set; }
            public AdminDetailedContractsResponseDto? Contracts { get; set; }
            public AdminDetailedRevenueResponseDto? Revenue { get; set; }
        }

        private sealed class ResolvedPeriod
        {
            public ResolvedPeriod(AdminAnalyticsReportPeriod period, DateTime? fromUtc, DateTime? toUtc, bool groupByDay)
            {
                Period = period;
                FromUtc = fromUtc;
                ToUtc = toUtc;
                GroupByDay = groupByDay;
            }

            public AdminAnalyticsReportPeriod Period { get; }
            public DateTime? FromUtc { get; }
            public DateTime? ToUtc { get; }
            public bool GroupByDay { get; }
            public string Grouping => GroupByDay ? "day" : "month";
        }

        private sealed record OverviewMetricCsvRow(string Metric, string Value);
    }
}
