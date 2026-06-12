using MARN_API.DTOs.Moderation;
using MARN_API.Enums;
using MARN_API.Enums.Report;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly IReportRepo _reportRepo;
        private readonly IAppTextLocalizer _localizer;
        private readonly ILogger<ReportService> _logger;

        public ReportService(
            IReportRepo reportRepo,
            IAppTextLocalizer localizer,
            ILogger<ReportService> logger)
        {
            _reportRepo = reportRepo;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<ServiceResult<ReportSubmissionResultDto>> SubmitReportAsync(Guid reporterId, SubmitReportDto request)
        {
            request ??= new SubmitReportDto();

            var reason = request.Reason?.Trim();
            var targetId = request.ReportableTargetId?.Trim();
            if (string.IsNullOrWhiteSpace(reason) || reason.Length < 5)
            {
                return ServiceResult<ReportSubmissionResultDto>.Fail(
                    "Reason must be at least 5 characters long.",
                    resultType: ServiceResultType.BadRequest);
            }

            if (string.IsNullOrWhiteSpace(targetId))
            {
                return ServiceResult<ReportSubmissionResultDto>.Fail(
                    "ReportableTargetId is required.",
                    resultType: ServiceResultType.BadRequest);
            }

            var parsedTarget = ParseTargetId(request.ReportableType, targetId);
            if (!parsedTarget.Success)
                return parsedTarget.Error!;

            var validationError = await ValidateTargetAsync(reporterId, request.ReportableType, parsedTarget.LongId, parsedTarget.GuidId);
            if (validationError != null)
                return validationError;

            var report = new Report
            {
                ReporterId = reporterId,
                ReportableType = request.ReportableType,
                ReportableId = parsedTarget.LongId,
                ReportableGuidId = parsedTarget.GuidId,
                Reason = reason,
                Status = ReportStatus.InReview
            };

            await _reportRepo.AddAsync(report);
            await _reportRepo.SaveChangesAsync();

            _logger.LogInformation(
                "User {ReporterId} submitted report {ReportId} for {ReportableType}.",
                reporterId,
                report.Id,
                report.ReportableType);

            return ServiceResult<ReportSubmissionResultDto>.Ok(
                new ReportSubmissionResultDto
                {
                    ReportId = report.Id,
                    Status = report.Status,
                    CreatedAt = report.CreatedAt
                },
                "Report submitted successfully.",
                ServiceResultType.Created,
                code: "ZZ_REPORT_SUBMITTED_SUCCESSFULLY");
        }

        private async Task<ServiceResult<ReportSubmissionResultDto>?> ValidateTargetAsync(
            Guid reporterId,
            ReportableType reportableType,
            long? reportableId,
            Guid? reportableGuidId)
        {
            switch (reportableType)
            {
                case ReportableType.User:
                    if (!reportableGuidId.HasValue)
                    {
                        return ServiceResult<ReportSubmissionResultDto>.Fail(
                            "User reports require a valid GUID target ID.",
                            resultType: ServiceResultType.BadRequest);
                    }

                    if (reportableGuidId.Value == reporterId)
                    {
                        return ServiceResult<ReportSubmissionResultDto>.Fail(
                            "You cannot report your own account.",
                            resultType: ServiceResultType.BadRequest);
                    }

                    var targetUser = await _reportRepo.GetUserTargetAsync(reportableGuidId.Value);
                    if (targetUser == null)
                    {
                        return ServiceResult<ReportSubmissionResultDto>.Fail(
                            "Reported user was not found.",
                            resultType: ServiceResultType.NotFound);
                    }
                    break;

                case ReportableType.Property:
                    if (!reportableId.HasValue)
                    {
                        return ServiceResult<ReportSubmissionResultDto>.Fail(
                            "Property reports require a valid numeric target ID.",
                            resultType: ServiceResultType.BadRequest);
                    }

                    var property = await _reportRepo.GetPropertyTargetAsync(reportableId.Value);
                    if (property == null)
                    {
                        return ServiceResult<ReportSubmissionResultDto>.Fail(
                            "Reported property was not found.",
                            resultType: ServiceResultType.NotFound);
                    }
                    break;

                case ReportableType.Message:
                    if (!reportableGuidId.HasValue)
                    {
                        return ServiceResult<ReportSubmissionResultDto>.Fail(
                            "Message reports require a valid GUID target ID.",
                            resultType: ServiceResultType.BadRequest);
                    }

                    var message = await _reportRepo.GetMessageTargetAsync(reportableGuidId.Value);
                    if (message == null)
                    {
                        return ServiceResult<ReportSubmissionResultDto>.Fail(
                            "Reported message was not found.",
                            resultType: ServiceResultType.NotFound);
                    }

                    if (message.SenderId != reporterId && message.ReceiverId != reporterId)
                    {
                        return ServiceResult<ReportSubmissionResultDto>.Fail(
                            "You can only report messages that belong to your conversation.",
                            resultType: ServiceResultType.Forbidden);
                    }
                    break;

                case ReportableType.PropertyComment:
                    if (!reportableId.HasValue)
                    {
                        return ServiceResult<ReportSubmissionResultDto>.Fail(
                            "Property comment reports require a valid numeric target ID.",
                            resultType: ServiceResultType.BadRequest);
                    }

                    var comment = await _reportRepo.GetPropertyCommentTargetAsync(reportableId.Value);
                    if (comment == null)
                    {
                        return ServiceResult<ReportSubmissionResultDto>.Fail(
                            "Reported property comment was not found.",
                            resultType: ServiceResultType.NotFound);
                    }
                    break;

                default:
                    return ServiceResult<ReportSubmissionResultDto>.Fail(
                        "Unsupported report type.",
                        resultType: ServiceResultType.BadRequest);
            }

            return null;
        }

        private (bool Success, long? LongId, Guid? GuidId, ServiceResult<ReportSubmissionResultDto>? Error) ParseTargetId(
            ReportableType reportableType,
            string targetId)
        {
            var reportableTypeDisplayName = _localizer.GetEnumDisplayName(reportableType);

            switch (reportableType)
            {
                case ReportableType.User:
                case ReportableType.Message:
                    if (Guid.TryParse(targetId, out var parsedGuid))
                        return (true, null, parsedGuid, null);

                    return (false, null, null, ServiceResult<ReportSubmissionResultDto>.Fail(
                        "{0} reports require a valid GUID target ID.",
                        resultType: ServiceResultType.BadRequest,
                        code: "REPORT_GUID_TARGET_ID_REQUIRED",
                        messageArguments: [reportableTypeDisplayName]));

                case ReportableType.Property:
                case ReportableType.PropertyComment:
                    if (long.TryParse(targetId, out var parsedLong) && parsedLong > 0)
                        return (true, parsedLong, null, null);

                    return (false, null, null, ServiceResult<ReportSubmissionResultDto>.Fail(
                        "{0} reports require a valid positive numeric target ID.",
                        resultType: ServiceResultType.BadRequest,
                        code: "REPORT_NUMERIC_TARGET_ID_REQUIRED",
                        messageArguments: [reportableTypeDisplayName]));

                default:
                    return (false, null, null, ServiceResult<ReportSubmissionResultDto>.Fail(
                        "Unsupported report type.",
                        resultType: ServiceResultType.BadRequest));
            }
        }
    }
}
