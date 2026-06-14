using System.Globalization;
using System.Net;
using MARN_API.DTOs.Common;
using MARN_API.Enums;
using MARN_API.Models;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class ContactSupportService : IContactSupportService
    {
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public ContactSupportService(IConfiguration configuration, IEmailService emailService)
        {
            _configuration = configuration;
            _emailService = emailService;
        }

        public async Task<ServiceResult<bool>> SendContactUsEmailAsync(ContactSupportRequestDto request, Guid? userId)
        {
            var supportEmail = _configuration["EmailSettings:SupportEmail"];
            if (string.IsNullOrWhiteSpace(supportEmail))
            {
                return ServiceResult<bool>.Fail(
                    "Support email is not configured.",
                    resultType: ServiceResultType.BadRequest);
            }

            var normalizedSubject = request.Subject.Trim();
            var messageBody = BuildSupportMessage(request, userId);
            var emailSubject = $"Contact Us - {normalizedSubject}";

            var sent = await _emailService.SendSupportContactEmailAsync(supportEmail, emailSubject, messageBody);
            if (!sent)
            {
                return ServiceResult<bool>.Fail(
                    "Failed to send your support request at the moment. Please try again later.",
                    resultType: ServiceResultType.BadRequest);
            }

            return ServiceResult<bool>.Ok(
                true,
                "Your message was sent successfully. Support will contact you later if needed.",
                code: "ZZ_SUPPORT_REQUEST_SENT_SUCCESSFULLY");
        }

        private static string BuildSupportMessage(ContactSupportRequestDto request, Guid? userId)
        {
            var isAnonymous = !userId.HasValue || userId == Guid.Empty;
            var userIdField = isAnonymous ? "Anonymous User" : userId.ToString();
            var badgeColor = isAnonymous ? "#6c757d" : "#0d6efd";
            var culture = CultureInfo.CurrentUICulture;
            var isRtl = culture.TextInfo.IsRightToLeft;
            var direction = isRtl ? "rtl" : "ltr";
            var textAlign = isRtl ? "right" : "left";
            var messageBorder = isRtl ? "border-right" : "border-left";
            var labelPadding = isRtl ? "6px 0 6px 8px" : "6px 8px 6px 0";
            var subject = string.IsNullOrWhiteSpace(request.Subject) ? "No Subject" : request.Subject.Trim();
            var phoneNumber = string.IsNullOrWhiteSpace(request.PhoneNumber) ? "Not Provided" : request.PhoneNumber.Trim();
            var email = request.Email.Trim();

            return $@"
<!DOCTYPE html>
<html dir=""{direction}"" lang=""{culture.TwoLetterISOLanguageName}"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>New Support Message</title>
</head>
<body style=""margin:0; padding:0; background-color:#f4f6f8; direction:{direction};"">
    <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""background-color:#f4f6f8; padding:20px 0; font-family:'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;"">
        <tr>
            <td align=""center"" style=""padding:0 12px;"">
                <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""max-width:600px; border:1px solid #e0e0e0; border-radius:8px; background-color:#f9f9f9; color:#333333; text-align:{textAlign};"">
                    <tr>
                        <td style=""padding:20px;"">
                            <div style=""border-bottom:2px solid #0d6efd; padding-bottom:10px; margin-bottom:20px;"">
                                <h2 style=""margin:0; color:#333333; font-size:22px; line-height:1.35;"">New Support Message</h2>
                                <p style=""margin:5px 0 0 0; font-size:14px; line-height:1.5; color:#666666;"">Subject: <strong>{Html(subject)}</strong></p>
                            </div>

                            <table role=""presentation"" width=""100%"" cellpadding=""0"" cellspacing=""0"" style=""border-collapse:collapse; margin-bottom:20px; text-align:{textAlign}; table-layout:fixed;"">
                                <tr>
                                    <td style=""padding:{labelPadding}; font-weight:bold; color:#555555; width:130px; vertical-align:top;"">Sender Name:</td>
                                    <td style=""padding:6px 0; color:#222222; vertical-align:top; overflow-wrap:break-word; word-break:break-word;"">{Html(request.FullName.Trim())}</td>
                                </tr>
                                <tr>
                                    <td style=""padding:{labelPadding}; font-weight:bold; color:#555555; width:130px; vertical-align:top;"">Email:</td>
                                    <td style=""padding:6px 0; vertical-align:top; overflow-wrap:break-word; word-break:break-word;""><a href=""mailto:{Attribute(email)}"" style=""color:#0d6efd; text-decoration:none;"">{Html(email)}</a></td>
                                </tr>
                                <tr>
                                    <td style=""padding:{labelPadding}; font-weight:bold; color:#555555; width:130px; vertical-align:top;"">Phone:</td>
                                    <td style=""padding:6px 0; color:#222222; vertical-align:top; overflow-wrap:break-word; word-break:break-word;"">{Html(phoneNumber)}</td>
                                </tr>
                                <tr>
                                    <td style=""padding:{labelPadding}; font-weight:bold; color:#555555; width:130px; vertical-align:top;"">User Id:</td>
                                    <td style=""padding:6px 0; vertical-align:top;""><span style=""background-color:{badgeColor}; color:#ffffff; padding:4px 10px; border-radius:4px; font-size:12px; font-weight:bold; display:inline-block; line-height:1.5; overflow-wrap:anywhere; word-break:break-word;"">{Html(userIdField)}</span></td>
                                </tr>
                            </table>

                            <div style=""background-color:#ffffff; {messageBorder}:4px solid #0d6efd; padding:15px; border-radius:4px; margin-top:15px; box-shadow:inset 0 1px 3px rgba(0,0,0,0.05);"">
                                <h4 style=""margin:0 0 10px 0; color:#555555; font-size:16px; line-height:1.4;"">Message:</h4>
                                <p style=""margin:0; line-height:1.6; white-space:pre-wrap; color:#222222; overflow-wrap:break-word; word-break:break-word;"">{Html(request.Message.Trim())}</p>
                            </div>

                            <div style=""margin-top:25px; text-align:center; font-size:12px; line-height:1.5; color:#999999; border-top:1px solid #e0e0e0; padding-top:15px;"">
                                This is an automated notification from your application's Contact Us form.
                            </div>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</body>
</html>";
        }

        private static string Html(string? value)
            => WebUtility.HtmlEncode(value ?? string.Empty);

        private static string Attribute(string? value)
            => WebUtility.HtmlEncode(value ?? string.Empty);
    }
}

