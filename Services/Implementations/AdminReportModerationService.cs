using MARN_API.DTOs.Moderation;
using MARN_API.Enums;
using MARN_API.Enums.Account;
using MARN_API.Enums.Report;
using MARN_API.Models;
using MARN_API.Repositories.Interfaces;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class AdminReportModerationService : IAdminReportModerationService
    {
        private const int MaxPageSize = 100;
        private const string HiddenMessagePlaceholder = "[Message hidden by admin]";
        private readonly IReportRepo _reportRepo;
        private readonly IEncryptionService _encryptionService;
        private readonly IAppTextLocalizer _localizer;
        private readonly ILogger<AdminReportModerationService> _logger;

        public AdminReportModerationService(
            IReportRepo reportRepo,
            IEncryptionService encryptionService,
            IAppTextLocalizer localizer,
            ILogger<AdminReportModerationService> logger)
        {
            _reportRepo = reportRepo;
            _encryptionService = encryptionService;
            _localizer = localizer;
            _logger = logger;
        }

        public async Task<ServiceResult<AdminModerationQueueDto>> GetReportsAsync(AdminReportQueryDto query)
        {
            query ??= new AdminReportQueryDto();
            query.PageNumber = query.PageNumber < 1 ? 1 : query.PageNumber;
            query.PageSize = query.PageSize < 1 ? 20 : Math.Min(query.PageSize, MaxPageSize);

            var result = await _reportRepo.GetAdminQueueAsync(query);
            LocalizeTargetLabels(result.Reports.Items);
            return ServiceResult<AdminModerationQueueDto>.Ok(result);
        }

        public async Task<ServiceResult<AdminModerationReportDetailsDto>> GetReportDetailsAsync(long reportId)
        {
            var report = await _reportRepo.GetReportDetailsAsync(reportId);
            if (report == null)
            {
                return ServiceResult<AdminModerationReportDetailsDto>.Fail(
                    "Report not found.",
                    resultType: ServiceResultType.NotFound);
            }

            var details = await BuildDetailsAsync(report);
            return ServiceResult<AdminModerationReportDetailsDto>.Ok(details);
        }

        public async Task<ServiceResult<AdminModerationReportDetailsDto>> ReviewReportAsync(Guid adminId, long reportId, AdminReviewReportDto request)
        {
            request ??= new AdminReviewReportDto();
            var normalizedActions = NormalizeActions(request);

            if (request.Status == ReportStatus.InReview)
            {
                return ServiceResult<AdminModerationReportDetailsDto>.Fail(
                    "Review status must be Resolved or Rejected.",
                    resultType: ServiceResultType.BadRequest);
            }

            if (request.Status == ReportStatus.Rejected && normalizedActions.Count > 0)
            {
                return ServiceResult<AdminModerationReportDetailsDto>.Fail(
                    "Rejected reports cannot apply a moderation action.",
                    resultType: ServiceResultType.BadRequest);
            }

            var report = await _reportRepo.GetTrackedReportAsync(reportId);
            if (report == null)
            {
                return ServiceResult<AdminModerationReportDetailsDto>.Fail(
                    "Report not found.",
                    resultType: ServiceResultType.NotFound);
            }

            if (report.Status != ReportStatus.InReview)
            {
                return ServiceResult<AdminModerationReportDetailsDto>.Fail(
                    "This report has already been reviewed.",
                    resultType: ServiceResultType.Conflict);
            }

            if (normalizedActions.Count > 0)
            {
                var actionValidationError = ValidateActionCompatibility(report.ReportableType, normalizedActions);
                if (actionValidationError != null)
                    return actionValidationError;

                foreach (var actionType in normalizedActions)
                {
                    var applyActionResult = await ApplyModerationActionAsync(adminId, report, actionType, request.Note?.Trim());
                    if (!applyActionResult.Success)
                    {
                        return ServiceResult<AdminModerationReportDetailsDto>.Fail(
                            applyActionResult.Message ?? "Failed to apply moderation action.",
                            applyActionResult.Errors,
                            resultType: applyActionResult.ResultType);
                    }
                }
            }

            report.Status = request.Status;
            report.ReviewerId = adminId;
            report.ReviewerNote = string.IsNullOrWhiteSpace(request.Note) ? null : request.Note.Trim();
            report.ActionTaken = normalizedActions.Count > 0 ? normalizedActions[0] : null;
            report.ReviewedAt = DateTime.UtcNow;

            await _reportRepo.SaveChangesAsync();

            _logger.LogInformation(
                "Admin {AdminId} reviewed report {ReportId} with status {Status} and action {ActionType}.",
                adminId,
                reportId,
                report.Status,
                report.ActionTaken);

            var refreshedReport = await _reportRepo.GetReportDetailsAsync(reportId);
            var details = await BuildDetailsAsync(refreshedReport!);

            return ServiceResult<AdminModerationReportDetailsDto>.Ok(
                details,
                "Report reviewed successfully.",
                code: "ZZ_ADMIN_REPORT_REVIEWED_SUCCESSFULLY");
        }

        private static ServiceResult<AdminModerationReportDetailsDto>? ValidateActionCompatibility(
            ReportableType reportableType,
            IReadOnlyCollection<ReportModerationActionType> actionTypes)
        {
            HashSet<ReportModerationActionType> allowedActions = reportableType switch
            {
                ReportableType.User => new HashSet<ReportModerationActionType>
                {
                    ReportModerationActionType.BanUser
                },
                ReportableType.Property => new HashSet<ReportModerationActionType>
                {
                    ReportModerationActionType.DeactivateProperty,
                    ReportModerationActionType.BanUser
                },
                ReportableType.Message => new HashSet<ReportModerationActionType>
                {
                    ReportModerationActionType.HideMessage,
                    ReportModerationActionType.BanUser
                },
                ReportableType.PropertyComment => new HashSet<ReportModerationActionType>
                {
                    ReportModerationActionType.HidePropertyComment,
                    ReportModerationActionType.BanUser
                },
                _ => new HashSet<ReportModerationActionType>()
            };

            var invalidActions = actionTypes
                .Where(actionType => !allowedActions.Contains(actionType))
                .Distinct()
                .ToList();

            return invalidActions.Count == 0
                ? null
                : ServiceResult<AdminModerationReportDetailsDto>.Fail(
                    $"The selected moderation action(s) do not match the reported target type: {string.Join(", ", invalidActions)}.",
                    resultType: ServiceResultType.BadRequest);
        }

        private async Task<ServiceResult<bool>> ApplyModerationActionAsync(
            Guid adminId,
            Report report,
            ReportModerationActionType actionType,
            string? note)
        {
            switch (actionType)
            {
                case ReportModerationActionType.BanUser:
                {
                    var user = await ResolveUserTargetForBanAsync(report);
                    if (user == null)
                    {
                        return ServiceResult<bool>.Fail(
                            "The reported user or responsible actor no longer exists.",
                            resultType: ServiceResultType.Conflict);
                    }

                    if (user.AccountStatus != AccountStatus.Banned)
                    {
                        user.StatusBeforeBan ??= user.AccountStatus;
                        user.AccountStatus = AccountStatus.Banned;
                    }

                    await _reportRepo.AddActionLogAsync(new AdminActionLog
                    {
                        AdminId = adminId,
                        ReportId = report.Id,
                        ActionType = actionType.ToString(),
                        TargetType = report.ReportableType.ToString(),
                        TargetGuidId = user.Id,
                        Reason = note
                    });
                    break;
                }

                case ReportModerationActionType.DeactivateProperty:
                {
                    if (!report.ReportableId.HasValue)
                    {
                        return ServiceResult<bool>.Fail(
                            "This report does not contain a valid property target.",
                            resultType: ServiceResultType.BadRequest);
                    }

                    var property = await _reportRepo.GetPropertyTargetAsync(report.ReportableId.Value);
                    if (property == null)
                    {
                        return ServiceResult<bool>.Fail(
                            "The reported property no longer exists.",
                            resultType: ServiceResultType.Conflict);
                    }

                    property.IsActive = false;

                    await _reportRepo.AddActionLogAsync(new AdminActionLog
                    {
                        AdminId = adminId,
                        ReportId = report.Id,
                        ActionType = actionType.ToString(),
                        TargetType = report.ReportableType.ToString(),
                        TargetLongId = property.Id,
                        Reason = note
                    });
                    break;
                }

                case ReportModerationActionType.HideMessage:
                {
                    if (!report.ReportableGuidId.HasValue)
                    {
                        return ServiceResult<bool>.Fail(
                            "This report does not contain a valid message target.",
                            resultType: ServiceResultType.BadRequest);
                    }

                    var message = await _reportRepo.GetMessageTargetAsync(report.ReportableGuidId.Value);
                    if (message == null)
                    {
                        return ServiceResult<bool>.Fail(
                            "The reported message no longer exists.",
                            resultType: ServiceResultType.Conflict);
                    }

                    message.IsHiddenByModeration = true;
                    message.HiddenAt = DateTime.UtcNow;
                    message.HiddenByAdminId = adminId;
                    message.HiddenReason = note;

                    await _reportRepo.AddActionLogAsync(new AdminActionLog
                    {
                        AdminId = adminId,
                        ReportId = report.Id,
                        ActionType = actionType.ToString(),
                        TargetType = report.ReportableType.ToString(),
                        TargetGuidId = message.Id,
                        Reason = note
                    });
                    break;
                }

                case ReportModerationActionType.HidePropertyComment:
                {
                    if (!report.ReportableId.HasValue)
                    {
                        return ServiceResult<bool>.Fail(
                            "This report does not contain a valid property comment target.",
                            resultType: ServiceResultType.BadRequest);
                    }

                    var comment = await _reportRepo.GetPropertyCommentTargetAsync(report.ReportableId.Value);
                    if (comment == null)
                    {
                        return ServiceResult<bool>.Fail(
                            "The reported property comment no longer exists.",
                            resultType: ServiceResultType.Conflict);
                    }

                    comment.IsHiddenByModeration = true;
                    comment.HiddenAt = DateTime.UtcNow;
                    comment.HiddenByAdminId = adminId;
                    comment.HiddenReason = note;

                    await _reportRepo.AddActionLogAsync(new AdminActionLog
                    {
                        AdminId = adminId,
                        ReportId = report.Id,
                        ActionType = actionType.ToString(),
                        TargetType = report.ReportableType.ToString(),
                        TargetLongId = comment.Id,
                        Reason = note
                    });
                    break;
                }
            }

            return ServiceResult<bool>.Ok(true, code: "ZZ_ADMIN_REPORT_MODERATION_ACTION_APPLIED_SUCCESSFULLY");
        }

        private async Task<AdminModerationReportDetailsDto> BuildDetailsAsync(Report report)
        {
            var details = new AdminModerationReportDetailsDto
            {
                ReportId = report.Id,
                ReportableType = report.ReportableType,
                Status = report.Status,
                Reason = report.Reason,
                ReviewerNote = report.ReviewerNote,
                ActionTaken = report.ActionTaken,
                ActionsTaken = GetAppliedActions(report),
                CreatedAt = report.CreatedAt,
                ReviewedAt = report.ReviewedAt,
                ReporterId = report.ReporterId,
                ReporterName = report.Reporter == null
                    ? T("[Deleted user]")
                    : $"{report.Reporter.FirstName} {report.Reporter.LastName}".Trim(),
                ReviewerId = report.ReviewerId,
                ReviewerName = report.Reviewer == null
                    ? null
                    : $"{report.Reviewer.FirstName} {report.Reviewer.LastName}".Trim(),
                ReportableTargetId = GetTargetIdString(report),
                Target = await BuildTargetDetailsAsync(report)
            };

            return details;
        }

        private async Task<ApplicationUser?> ResolveUserTargetForBanAsync(Report report)
        {
            switch (report.ReportableType)
            {
                case ReportableType.User:
                    return report.ReportableGuidId.HasValue
                        ? await _reportRepo.GetUserTargetAsync(report.ReportableGuidId.Value)
                        : null;

                case ReportableType.Property:
                {
                    if (!report.ReportableId.HasValue)
                        return null;

                    var property = await _reportRepo.GetPropertyTargetAsync(report.ReportableId.Value);
                    return property == null ? null : await _reportRepo.GetUserTargetAsync(property.OwnerId);
                }

                case ReportableType.Message:
                {
                    if (!report.ReportableGuidId.HasValue)
                        return null;

                    var message = await _reportRepo.GetMessageTargetAsync(report.ReportableGuidId.Value);
                    return message == null ? null : await _reportRepo.GetUserTargetAsync(message.SenderId);
                }

                case ReportableType.PropertyComment:
                {
                    if (!report.ReportableId.HasValue)
                        return null;

                    var comment = await _reportRepo.GetPropertyCommentTargetAsync(report.ReportableId.Value);
                    return comment == null ? null : await _reportRepo.GetUserTargetAsync(comment.UserId);
                }

                default:
                    return null;
            }
        }

        private static List<ReportModerationActionType> NormalizeActions(AdminReviewReportDto request)
        {
            var normalizedActions = new List<ReportModerationActionType>();
            var seen = new HashSet<ReportModerationActionType>();

            if (request.ActionTypes == null)
                return normalizedActions;

            foreach (var actionType in request.ActionTypes)
            {
                if (seen.Add(actionType))
                {
                    normalizedActions.Add(actionType);
                }
            }

            return normalizedActions;
        }

        private static string GetTargetIdString(Report report)
        {
            return report.ReportableGuidId?.ToString()
                ?? report.ReportableId?.ToString()
                ?? string.Empty;
        }

        private static List<ReportModerationActionType> GetAppliedActions(Report report)
        {
            var actions = report.ActionLogs
                .Select(log => Enum.TryParse<ReportModerationActionType>(log.ActionType, ignoreCase: true, out var parsedAction)
                    ? parsedAction
                    : (ReportModerationActionType?)null)
                .Where(action => action.HasValue)
                .Select(action => action!.Value)
                .Distinct()
                .ToList();

            if (actions.Count == 0 && report.ActionTaken.HasValue)
            {
                actions.Add(report.ActionTaken.Value);
            }

            return actions;
        }

        private async Task<AdminModerationTargetDetailsDto> BuildTargetDetailsAsync(Report report)
        {
            switch (report.ReportableType)
            {
                case ReportableType.User:
                {
                    if (!report.ReportableGuidId.HasValue)
                        return MissingTarget();

                    var user = await _reportRepo.GetUserTargetAsync(report.ReportableGuidId.Value);
                    if (user == null)
                        return MissingTarget();

                    return new AdminModerationTargetDetailsDto
                    {
                        Exists = true,
                        Title = $"{user.FirstName} {user.LastName}".Trim(),
                        Subtitle = user.Email,
                        Preview = user.Bio,
                        UserId = user.Id,
                        IsDeletedOrInactive = user.DeletedAt != null || user.AccountStatus == AccountStatus.Banned
                    };
                }

                case ReportableType.Property:
                {
                    if (!report.ReportableId.HasValue)
                        return MissingTarget();

                    var property = await _reportRepo.GetPropertyTargetAsync(report.ReportableId.Value);
                    if (property == null)
                        return MissingTarget();

                    return new AdminModerationTargetDetailsDto
                    {
                        Exists = true,
                        PropertyId = property.Id,
                        Title = property.Title,
                        Subtitle = $"{GetLocationDisplayName<Enums.Property.City>(property.City)}, {GetLocationDisplayName<Enums.Property.Governorate>(property.State)}",
                        Preview = property.Description,
                        UserId = property.OwnerId,
                        IsDeletedOrInactive = property.DeletedAt != null || !property.IsActive
                    };
                }

                case ReportableType.Message:
                {
                    if (!report.ReportableGuidId.HasValue)
                        return MissingTarget();

                    var message = await _reportRepo.GetMessageTargetAsync(report.ReportableGuidId.Value);
                    if (message == null)
                        return MissingTarget();

                    return new AdminModerationTargetDetailsDto
                    {
                        Exists = true,
                        MessageId = message.Id,
                        Title = $"{message.Sender.FirstName} {message.Sender.LastName}".Trim(),
                        Subtitle = TF("TEXT_TO_0", "To {0}", $"{message.Receiver.FirstName} {message.Receiver.LastName}".Trim()),
                        Preview = message.IsHiddenByModeration
                            ? T(HiddenMessagePlaceholder)
                            : _encryptionService.Decrypt(message.Content),
                        UserId = message.SenderId,
                        IsHidden = message.IsHiddenByModeration
                    };
                }

                case ReportableType.PropertyComment:
                {
                    if (!report.ReportableId.HasValue)
                        return MissingTarget();

                    var comment = await _reportRepo.GetPropertyCommentTargetAsync(report.ReportableId.Value);
                    if (comment == null)
                        return MissingTarget();

                    return new AdminModerationTargetDetailsDto
                    {
                        Exists = true,
                        PropertyCommentId = comment.Id,
                        PropertyId = comment.PropertyId,
                        UserId = comment.UserId,
                        Title = $"{comment.User.FirstName} {comment.User.LastName}".Trim(),
                        Subtitle = TF("TEXT_ON_PROPERTY_0", "On property: {0}", comment.Property.Title),
                        Preview = comment.Content,
                        IsHidden = comment.IsHiddenByModeration
                    };
                }

                default:
                    return MissingTarget();
            }
        }

        private AdminModerationTargetDetailsDto MissingTarget()
        {
            return new AdminModerationTargetDetailsDto
            {
                Exists = false,
                Title = T("[Target not found]")
            };
        }

        private string T(string literal) => _localizer.LocalizeLiteral(literal);

        private string TF(string key, string fallback, params object?[] arguments)
            => _localizer.GetOrFallback(key, fallback, arguments: arguments);

        private void LocalizeTargetLabels(IEnumerable<AdminModerationReportListItemDto> reports)
        {
            foreach (var report in reports)
            {
                report.TargetLabel = report.ReportableType switch
                {
                    ReportableType.Message => LocalizeMessageTargetLabel(report.TargetLabel),
                    ReportableType.PropertyComment when string.Equals(report.TargetLabel, "[Deleted comment]", StringComparison.Ordinal) => T("[Deleted comment]"),
                    ReportableType.PropertyComment => TF("TEXT_ON_PROPERTY_0", "On property: {0}", report.TargetLabel.Replace("Comment on ", string.Empty, StringComparison.Ordinal)),
                    ReportableType.User when string.Equals(report.TargetLabel, "[Deleted user]", StringComparison.Ordinal) => T("[Deleted user]"),
                    ReportableType.Property when string.Equals(report.TargetLabel, "[Deleted property]", StringComparison.Ordinal) => T("[Deleted property]"),
                    _ => report.TargetLabel
                };
            }
        }

        private string LocalizeMessageTargetLabel(string targetLabel)
        {
            if (string.Equals(targetLabel, "[Deleted message]", StringComparison.Ordinal))
            {
                return T("[Deleted message]");
            }

            const string prefix = "Message between ";
            var marker = " and ";
            if (!targetLabel.StartsWith(prefix, StringComparison.Ordinal))
            {
                return targetLabel;
            }

            var names = targetLabel[prefix.Length..];
            var separatorIndex = names.IndexOf(marker, StringComparison.Ordinal);
            if (separatorIndex <= 0)
            {
                return targetLabel;
            }

            var firstName = names[..separatorIndex];
            var secondName = names[(separatorIndex + marker.Length)..];
            return TF("TEXT_MESSAGE_BETWEEN_0_AND_1", "Message between {0} and {1}", firstName, secondName);
        }

        private string GetLocationDisplayName<TEnum>(string? rawValue) where TEnum : struct, Enum
        {
            if (!string.IsNullOrWhiteSpace(rawValue) && Enum.TryParse<TEnum>(rawValue, true, out var parsed))
            {
                return _localizer.GetEnumDisplayName(parsed);
            }

            return rawValue ?? string.Empty;
        }
    }
}
