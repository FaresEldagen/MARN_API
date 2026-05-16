using CsvHelper;
using MARN_API.DTOs.Admin;
using MARN_API.DTOs.Common;
using MARN_API.Enums;
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
        private readonly ILogger<AdminAnalyticsReportService> _logger;

        public AdminAnalyticsReportService(
            IAdminAnalyticsReportRepo analyticsReportRepo,
            IAdminDashboardService dashboardService,
            IAdminDetailedStatsRepo detailedStatsRepo,
            IWebHostEnvironment environment,
            ILogger<AdminAnalyticsReportService> logger)
        {
            _analyticsReportRepo = analyticsReportRepo;
            _dashboardService = dashboardService;
            _detailedStatsRepo = detailedStatsRepo;
            _environment = environment;
            _logger = logger;

            QuestPDF.Settings.License = LicenseType.Community;
        }

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
                "Analytics report generated successfully.");
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
                    csv.WriteRecords(BuildOverviewCsvRows(bundle));
                    break;
                case AdminAnalyticsReportScope.Users:
                    csv.WriteRecords(bundle.Users!.Users.Items.Select(x => new
                    {
                        x.UserId,
                        x.FullName,
                        x.Email,
                        AccountStatus = x.AccountStatus.ToString(),
                        x.IsDeleted,
                        x.CreatedAt,
                        Roles = string.Join(", ", x.Roles)
                    }));
                    break;
                case AdminAnalyticsReportScope.Properties:
                    csv.WriteRecords(bundle.Properties!.Properties.Items.Select(x => new
                    {
                        x.PropertyId,
                        x.Title,
                        x.OwnerId,
                        x.OwnerName,
                        Status = x.Status.ToString(),
                        Type = x.Type.ToString(),
                        x.City,
                        x.State,
                        x.Price,
                        x.IsActive,
                        x.IsDeleted,
                        x.CreatedAt
                    }));
                    break;
                case AdminAnalyticsReportScope.Contracts:
                    csv.WriteRecords(bundle.Contracts!.Contracts.Items.Select(x => new
                    {
                        x.ContractId,
                        Status = x.Status.ToString(),
                        x.CreatedAt,
                        x.LeaseStartDate,
                        x.LeaseEndDate,
                        x.TotalContractAmount,
                        x.PaymentFrequency,
                        x.PropertyId,
                        x.PropertyTitle,
                        x.OwnerId,
                        x.OwnerName,
                        x.RenterId,
                        x.RenterName
                    }));
                    break;
                case AdminAnalyticsReportScope.Revenue:
                    csv.WriteRecords(bundle.Revenue!.Payments.Items.Select(x => new
                    {
                        x.PaymentId,
                        x.ContractId,
                        x.PaymentScheduleId,
                        Status = x.Status.ToString(),
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

        private static void ConfigurePage(
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
                column.Item().Text($"Admin {scope} Report")
                    .SemiBold()
                    .FontSize(20);
                column.Item().Text($"Period: {FormatPeriod(period)}");
                column.Item().Text($"Generated at: {generatedAt:u}");
                column.Item().Text($"Generated by: {adminUser.FirstName} {adminUser.LastName}".Trim());
            });

            page.Footer().AlignCenter().Text(text =>
            {
                text.Span("Page ");
                text.CurrentPageNumber();
                text.Span(" / ");
                text.TotalPages();
            });
        }

        private static void ComposeExecutiveSnapshot(ColumnDescriptor column, AnalyticsExportBundle bundle)
        {
            if (bundle.Overview == null)
                return;

            column.Item().Text("Executive Snapshot").SemiBold().FontSize(14);
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                AddKeyValueRow(table, "Total Users", bundle.Overview.TotalUsers.Value.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, "Total Properties", bundle.Overview.TotalProperties.Value.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, "Pending Verifications", bundle.Overview.PendingVerifications.Value.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, "Total Contracts", bundle.Overview.TotalContracts.Value.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, "Total Revenue", bundle.Overview.RevenueSummary.TotalRevenue.ToString("N2", CultureInfo.InvariantCulture));
                AddKeyValueRow(table, "Total Sales", bundle.Overview.RevenueSummary.TotalSales.ToString("N2", CultureInfo.InvariantCulture));
                AddKeyValueRow(table, "Active Contracts", bundle.Overview.RevenueSummary.ActiveContracts.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, "New Users This Month", bundle.Overview.RevenueSummary.NewUsersThisMonth.ToString(CultureInfo.InvariantCulture));
            });
        }

        private static void ComposeOverviewPeriodNote(ColumnDescriptor column, ResolvedPeriod period)
        {
            column.Item().Text($"This report reflects the current admin snapshot and the selected period context: {FormatPeriod(period)}.");
        }

        private static void ComposeUsersSection(ColumnDescriptor column, AdminDetailedUsersResponseDto users)
        {
            column.Item().Text("Users").SemiBold().FontSize(14);
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                AddKeyValueRow(table, "Total Users", users.TotalUsers.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, "Deleted Users", users.DeletedUsers.ToString(CultureInfo.InvariantCulture));
            });

            column.Item().Text("Status Breakdown").SemiBold();
            ComposeThreeColumnTable(
                column,
                ["Status", "Count", "Share"],
                users.StatusBreakdown.Select(x => new[]
                {
                    x.Status.ToString(),
                    x.Count.ToString(CultureInfo.InvariantCulture),
                    users.TotalUsers == 0 ? "0%" : $"{(x.Count * 100m / users.TotalUsers):N1}%"
                }));

            column.Item().Text("Latest Users").SemiBold();
            ComposeFourColumnTable(
                column,
                ["User", "Status", "Created", "Roles"],
                users.Users.Items.Select(x => new[]
                {
                    x.FullName,
                    x.AccountStatus.ToString(),
                    x.CreatedAt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    string.Join(", ", x.Roles)
                }));
        }

        private static void ComposePropertiesSection(ColumnDescriptor column, AdminDetailedPropertiesResponseDto properties)
        {
            column.Item().Text("Properties").SemiBold().FontSize(14);
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                AddKeyValueRow(table, "Total Properties", properties.TotalProperties.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, "Active Properties", properties.ActiveProperties.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, "Inactive Properties", properties.InactiveProperties.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, "Deleted Properties", properties.DeletedProperties.ToString(CultureInfo.InvariantCulture));
            });

            column.Item().Text("Status Breakdown").SemiBold();
            ComposeTwoColumnTable(column, ["Status", "Count"], properties.StatusBreakdown.Select(x => new[]
            {
                x.Status.ToString(),
                x.Count.ToString(CultureInfo.InvariantCulture)
            }));

            column.Item().Text("Latest Properties").SemiBold();
            ComposeFourColumnTable(
                column,
                ["Property", "Owner", "Status", "Location"],
                properties.Properties.Items.Select(x => new[]
                {
                    x.Title,
                    x.OwnerName,
                    x.Status.ToString(),
                    $"{x.City}, {x.State}"
                }));
        }

        private static void ComposeContractsSection(ColumnDescriptor column, AdminDetailedContractsResponseDto contracts)
        {
            column.Item().Text("Contracts").SemiBold().FontSize(14);
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                AddKeyValueRow(table, "Total Contracts", contracts.TotalContracts.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, "Total Contract Value", contracts.TotalContractValue.ToString("N2", CultureInfo.InvariantCulture));
            });

            column.Item().Text("Status Breakdown").SemiBold();
            ComposeTwoColumnTable(column, ["Status", "Count"], contracts.StatusBreakdown.Select(x => new[]
            {
                x.Status.ToString(),
                x.Count.ToString(CultureInfo.InvariantCulture)
            }));

            column.Item().Text("Latest Contracts").SemiBold();
            ComposeFourColumnTable(
                column,
                ["Contract", "Property", "Owner", "Renter"],
                contracts.Contracts.Items.Select(x => new[]
                {
                    x.ContractId.ToString(CultureInfo.InvariantCulture),
                    x.PropertyTitle,
                    x.OwnerName,
                    x.RenterName
                }));
        }

        private static void ComposeRevenueSection(ColumnDescriptor column, AdminDetailedRevenueResponseDto revenue)
        {
            column.Item().Text("Revenue").SemiBold().FontSize(14);
            column.Item().Table(table =>
            {
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                AddKeyValueRow(table, "Total Payments", revenue.TotalPayments.ToString(CultureInfo.InvariantCulture));
                AddKeyValueRow(table, "Total Sales", revenue.TotalSales.ToString("N2", CultureInfo.InvariantCulture));
                AddKeyValueRow(table, "Platform Revenue", revenue.TotalRevenue.ToString("N2", CultureInfo.InvariantCulture));
                AddKeyValueRow(table, "Owner Payouts", revenue.TotalOwnerPayouts.ToString("N2", CultureInfo.InvariantCulture));
            });

            column.Item().Text("Payment Status Breakdown").SemiBold();
            ComposeThreeColumnTable(
                column,
                ["Status", "Count", "Revenue"],
                revenue.StatusBreakdown.Select(x => new[]
                {
                    x.Status.ToString(),
                    x.Count.ToString(CultureInfo.InvariantCulture),
                    x.Revenue.ToString("N2", CultureInfo.InvariantCulture)
                }));

            column.Item().Text("Revenue Over Time").SemiBold();
            ComposeThreeColumnTable(
                column,
                ["Period", "Revenue", "Sales"],
                revenue.RevenueOverTime.Select(x => new[]
                {
                    x.Label,
                    x.Revenue.ToString("N2", CultureInfo.InvariantCulture),
                    x.Sales.ToString("N2", CultureInfo.InvariantCulture)
                }));
        }

        private static void ComposeTwoColumnTable(ColumnDescriptor column, string[] headers, IEnumerable<string[]> rows)
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

        private static void ComposeThreeColumnTable(ColumnDescriptor column, string[] headers, IEnumerable<string[]> rows)
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

        private static void ComposeFourColumnTable(ColumnDescriptor column, string[] headers, IEnumerable<string[]> rows)
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

        private static void AddKeyValueRow(TableDescriptor table, string key, string value)
        {
            table.Cell().Element(CellStyle).Text(key);
            table.Cell().Element(CellStyle).Text(value);
        }

        private static void AddHeaderRow(TableDescriptor table, IEnumerable<string> headers)
        {
            foreach (var header in headers)
            {
                table.Cell().Element(HeaderCellStyle).Text(header).SemiBold();
            }
        }

        private static void AddRow(TableDescriptor table, IEnumerable<string> cells)
        {
            foreach (var cell in cells)
            {
                table.Cell().Element(CellStyle).Text(cell ?? string.Empty);
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

        private static List<OverviewMetricCsvRow> BuildOverviewCsvRows(AnalyticsExportBundle bundle)
        {
            if (bundle.Overview == null)
                return [];

            return
            [
                new OverviewMetricCsvRow("Total Users", bundle.Overview.TotalUsers.Value.ToString(CultureInfo.InvariantCulture)),
                new OverviewMetricCsvRow("Total Properties", bundle.Overview.TotalProperties.Value.ToString(CultureInfo.InvariantCulture)),
                new OverviewMetricCsvRow("Pending Verifications", bundle.Overview.PendingVerifications.Value.ToString(CultureInfo.InvariantCulture)),
                new OverviewMetricCsvRow("Total Contracts", bundle.Overview.TotalContracts.Value.ToString(CultureInfo.InvariantCulture)),
                new OverviewMetricCsvRow("Total Revenue", bundle.Overview.RevenueSummary.TotalRevenue.ToString("N2", CultureInfo.InvariantCulture)),
                new OverviewMetricCsvRow("Total Sales", bundle.Overview.RevenueSummary.TotalSales.ToString("N2", CultureInfo.InvariantCulture)),
                new OverviewMetricCsvRow("New Users This Month", bundle.Overview.RevenueSummary.NewUsersThisMonth.ToString(CultureInfo.InvariantCulture)),
                new OverviewMetricCsvRow("Active Contracts", bundle.Overview.RevenueSummary.ActiveContracts.ToString(CultureInfo.InvariantCulture))
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

        private static string FormatPeriod(ResolvedPeriod period)
        {
            return period.Period == AdminAnalyticsReportPeriod.Custom && period.FromUtc.HasValue && period.ToUtc.HasValue
                ? $"{period.FromUtc:yyyy-MM-dd} to {period.ToUtc:yyyy-MM-dd}"
                : period.Period.ToString();
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
                Format = report.Format,
                RequestedPeriod = report.RequestedPeriod,
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
