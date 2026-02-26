using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MARN_API.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendRegistrationConfirmationEmailAsync(string toEmail, string firstName, string confirmationLink);
        Task SendAccountCreatedEmailAsync(string toEmail, string firstName, string loginLink);
        Task SendResendConfirmationEmailAsync(string toEmail, string firstName, string confirmationLink);
        Task SendResetPasswordEmailAsync(string toEmail, string firstName, string resetLink);
        Task Send2FAEmailAsync(string toEmail, string subject, string code);

    }
}