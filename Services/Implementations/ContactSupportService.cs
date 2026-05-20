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

            return ServiceResult<bool>.Ok(true, "Your message was sent successfully. Support will contact you later if needed.");
        }

        private static string BuildSupportMessage(ContactSupportRequestDto request, Guid? userId)
        {
            // 1. Identify if the request is anonymous (null or empty Guid)
            var isAnonymous = !userId.HasValue || userId == Guid.Empty;
            var userIdField = isAnonymous ? "Anonymous User" : userId.ToString();

            // 2. Assign badge color: Gray for anonymous, Blue for registered users
            var badgeColor = isAnonymous ? "#6c757d" : "#0d6efd";

            // 3. Return the fully structured, email-client-friendly HTML template
            return $@"
    <div style=""font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; max-width: 600px; margin: 20px auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 8px; background-color: #f9f9f9; color: #333;"">
        
        <!-- Header Section -->
        <div style=""border-bottom: 2px solid #0d6efd; padding-bottom: 10px; margin-bottom: 20px;"">
            <h2 style=""margin: 0; color: #333;"">New Support Message</h2>
            <p style=""margin: 5px 0 0 0; font-size: 14px; color: #666;"">Subject: <strong>{(string.IsNullOrWhiteSpace(request.Subject) ? "No Subject" : request.Subject.Trim())}</strong></p>
        </div>

        <!-- Metadata Grid Table -->
        <table style=""width: 100%; border-collapse: collapse; margin-bottom: 20px; text-align: left; table-layout: fixed;"">
            <tr>
                <td style=""padding: 6px 0; font-weight: bold; color: #555; width: 130px; vertical-align: middle;"">Sender Name:</td>
                <td style=""padding: 6px 0; color: #222; vertical-align: middle;"">{request.FullName.Trim()}</td>
            </tr>
            <tr>
                <td style=""padding: 6px 0; font-weight: bold; color: #555; width: 130px; vertical-align: middle;"">Email:</td>
                <td style=""padding: 6px 0; vertical-align: middle;""><a href=""mailto:{request.Email.Trim()}"" style=""color: #0d6efd; text-decoration: none;"">{request.Email.Trim()}</a></td>
            </tr>
            <tr>
                <td style=""padding: 6px 0; font-weight: bold; color: #555; width: 130px; vertical-align: middle;"">Phone:</td>
                <td style=""padding: 6px 0; color: #222; vertical-align: middle;"">{(string.IsNullOrWhiteSpace(request.PhoneNumber) ? "Not Provided" : request.PhoneNumber.Trim())}</td>
            </tr>
            <tr>
                <td style=""padding: 6px 0; font-weight: bold; color: #555; width: 130px; vertical-align: middle;"">User Id:</td>
                <td style=""padding: 6px 0; vertical-align: middle;""><span style=""background-color: {badgeColor}; color: #ffffff; padding: 4px 10px; border-radius: 4px; font-size: 12px; font-weight: bold; display: inline-block; white-space: nowrap;"">{userIdField}</span></td>
            </tr>
        </table>

        <!-- Message Body Section -->
        <div style=""background-color: #ffffff; border-left: 4px solid #0d6efd; padding: 15px; border-radius: 4px; margin-top: 15px; box-shadow: inset 0 1px 3px rgba(0,0,0,0.05);"">
            <h4 style=""margin: 0 0 10px 0; color: #555;"">Message:</h4>
            <!-- pre-wrap guarantees that paragraph line breaks typed by the user stay intact -->
            <p style=""margin: 0; line-height: 1.6; white-space: pre-wrap; color: #222;"">{request.Message.Trim()}</p>
        </div>

        <!-- Email Footer -->
        <div style=""margin-top: 25px; text-align: center; font-size: 12px; color: #999; border-top: 1px solid #e0e0e0; padding-top: 15px;"">
            This is an automated notification from your application's Contact Us form.
        </div>
    </div>";
        }
    }
}

