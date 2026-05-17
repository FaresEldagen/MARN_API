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

        public async Task<ServiceResult<bool>> SendContactUsEmailAsync(ContactSupportRequestDto request, Guid userId)
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

        private static string BuildSupportMessage(ContactSupportRequestDto request, Guid userId)
        {
            return
                $"UserId: {userId.ToString()}{Environment.NewLine}" +
                $"Full Name: {request.FullName.Trim()}{Environment.NewLine}" +
                $"Contact Email: {request.Email.Trim()}{Environment.NewLine}" +
                $"Phone Number: {request.PhoneNumber.Trim()}{Environment.NewLine}" +
                $"Subject: {request.Subject.Trim()}{Environment.NewLine}" +
                $"Message: {request.Message.Trim()}";
        }
    }
}
