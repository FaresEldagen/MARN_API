using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;
using MARN_API.Localization;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly IAppTextLocalizer _localizer;

        public EmailService(IConfiguration configuration, IAppTextLocalizer localizer)
        {
            _configuration = configuration;
            _localizer = localizer;
        }

        public async Task SendRegistrationConfirmationEmailAsync(string toEmail, string firstName, string confirmationLink)
        {
            var subject = Text("EMAIL_CONFIRMATION_SUBJECT", "Email Confirmation - MARN");
            var htmlContent = BuildActionEmail(
                Text("EMAIL_CONFIRMATION_HEADING", "Welcome, {0}!", firstName),
                Text("EMAIL_CONFIRMATION_BODY", "Thank you for registering. Please confirm your email by clicking the button below."),
                Text("EMAIL_CONFIRMATION_BUTTON", "Confirm Your Email"),
                confirmationLink,
                "#0d6efd");

            await SendEmailAsync(toEmail, subject, htmlContent, true);
        }

        public async Task SendAccountCreatedEmailAsync(string toEmail, string firstName, string loginLink)
        {
            var subject = Text("EMAIL_ACCOUNT_CREATED_SUBJECT", "Account Created - MARN");
            var htmlContent = BuildActionEmail(
                Text("EMAIL_ACCOUNT_CREATED_HEADING", "Hello, {0}!", firstName),
                Text("EMAIL_ACCOUNT_CREATED_BODY", "Your account has been successfully created and your email is confirmed."),
                Text("EMAIL_ACCOUNT_CREATED_BUTTON", "Login to Your Account"),
                loginLink,
                "#198754");

            await SendEmailAsync(toEmail, subject, htmlContent, true);
        }

        public async Task SendResendConfirmationEmailAsync(string toEmail, string firstName, string confirmationLink)
        {
            var subject = Text("EMAIL_CONFIRMATION_SUBJECT", "Email Confirmation - MARN");
            var htmlContent = BuildActionEmail(
                Text("EMAIL_ACCOUNT_CREATED_HEADING", "Hello, {0}!", firstName),
                Text("EMAIL_RESEND_CONFIRMATION_BODY", "You requested a new email confirmation link. Please confirm your email by clicking the button below."),
                Text("EMAIL_CONFIRMATION_BUTTON", "Confirm Your Email"),
                confirmationLink,
                "#0d6efd");

            await SendEmailAsync(toEmail, subject, htmlContent, true);
        }

        public async Task SendResetPasswordEmailAsync(string toEmail, string firstName, string resetLink)
        {
            var subject = Text("EMAIL_RESET_PASSWORD_SUBJECT", "Reset Password - MARN");
            var body = BuildActionEmail(
                Text("EMAIL_ACCOUNT_CREATED_HEADING", "Hello, {0}!", firstName),
                Text("EMAIL_RESET_PASSWORD_BODY", "You requested a Reset Password link. Please reset your password by clicking the button below."),
                Text("EMAIL_RESET_PASSWORD_BUTTON", "Reset Password"),
                resetLink,
                "#0d6efd",
                Text("EMAIL_RESET_PASSWORD_EXPIRY", "This link will expire in 1 hour."));

            await SendEmailAsync(toEmail, subject, body, true);
        }

        public async Task SendAccountDeletionEmailAsync(string toEmail, string firstName)
        {
            const string supportEmail = "support@marn.com";

            await SendEmailAsync(
                toEmail,
                Text("EMAIL_ACCOUNT_DELETION_SUBJECT", "Account Deletion Confirmation - MARN"),
                BuildDeletionEmail(firstName, supportEmail),
                true);
        }

        public async Task Send2FAEmailAsync(string toEmail, string subject, string code)
        {
            var resolvedSubject = Text("EMAIL_TWO_FACTOR_SUBJECT", subject);
            await SendEmailAsync(toEmail, resolvedSubject, BuildTwoFactorEmail(code), isBodyHtml: true);
        }

        public async Task<bool> SendSupportContactEmailAsync(string supportEmail, string subject, string messageBody)
        {
            try
            {
                await SendEmailCoreAsync(supportEmail, subject, messageBody, isBodyHtml: true);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body, bool isBodyHtml = false)
        {
            try
            {
                await SendEmailCoreAsync(toEmail, subject, body, isBodyHtml);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private async Task SendEmailCoreAsync(string toEmail, string subject, string body, bool isBodyHtml)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var senderName = _configuration["EmailSettings:SenderName"];
            var password = _configuration["EmailSettings:Password"];

            using var message = new MailMessage
            {
                From = new MailAddress(senderEmail!, senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = isBodyHtml,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8,
                HeadersEncoding = Encoding.UTF8
            };
            message.To.Add(new MailAddress(toEmail));

            using var client = new SmtpClient(smtpServer, smtpPort)
            {
                Credentials = new NetworkCredential(senderEmail, password),
                EnableSsl = true
            };

            await client.SendMailAsync(message);
        }

        private string BuildActionEmail(string heading, string description, string buttonText, string actionLink, string accentColor, string? footerNote = null)
        {
            return $@"
                <!DOCTYPE html>
                <html dir='{Direction}' lang='{LanguageCode}'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    </head>
                    <body style='font-family: Arial, sans-serif; background-color: #f4f6f8; margin:0; padding:20px; direction:{Direction};'>
                        <div style='max-width:600px; margin:auto; background:#fff; padding:30px; border-radius:8px; text-align:{TextAlign};'>
                            <h2 style='color:#333;'>{Html(heading)}</h2>
                            <p style='font-size:16px; color:#555;'>{Html(description)}</p>
                            <p style='text-align:center;'>
                                <a href='{Attribute(actionLink)}' style='background:{accentColor}; color:#fff; padding:12px 24px; border-radius:6px; text-decoration:none; font-weight:bold;'>{Html(buttonText)}</a>
                            </p>
                            {(string.IsNullOrWhiteSpace(footerNote) ? string.Empty : $"<p style='font-size:16px; color:#555;'>{Html(footerNote)}</p>")}
                            <p style='font-size:12px; color:#999; margin-top:30px;'>&copy; {DateTime.UtcNow.Year} {Html(Text("EMAIL_FOOTER", "MARN. All rights reserved."))}</p>
                        </div>
                    </body>
                </html>";
        }

        private string BuildDeletionEmail(string firstName, string supportEmail)
        {
            return $@"
                <!DOCTYPE html>
                <html dir='{Direction}' lang='{LanguageCode}'>
                    <head>
                        <meta charset='UTF-8'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    </head>
                    <body style='font-family: Arial, sans-serif; background-color: #f4f6f8; margin:0; padding:20px; direction:{Direction};'>
                        <div style='max-width:600px; margin:auto; background:#fff; padding:30px; border-radius:8px; text-align:{TextAlign};'>
                            <h2 style='color:#333;'>{Html(Text("EMAIL_ACCOUNT_DELETION_HEADING", "Goodbye, {0}", firstName))}</h2>
                            <p style='font-size:16px; color:#555;'>{Html(Text("EMAIL_ACCOUNT_DELETION_BODY_1", "Your account has been successfully deleted from our platform."))}</p>
                            <p style='font-size:16px; color:#555;'>{Html(Text("EMAIL_ACCOUNT_DELETION_BODY_2", "If this action was not intended or you would like to restore your account or create a new one using the same email address, please contact our support team."))}</p>
                            <p style='text-align:center;'>
                                <a href='mailto:{Attribute(supportEmail)}' style='background:#dc3545; color:#fff; padding:12px 24px; border-radius:6px; text-decoration:none; font-weight:bold;'>{Html(Text("EMAIL_ACCOUNT_DELETION_BUTTON", "Contact Support"))}</a>
                            </p>
                            <p style='font-size:14px; color:#555; margin-top:20px;'>{Html(Text("EMAIL_SUPPORT_EMAIL_LABEL", "Support Email:"))} {Html(supportEmail)}</p>
                            <p style='font-size:12px; color:#999; margin-top:30px;'>&copy; {DateTime.UtcNow.Year} {Html(Text("EMAIL_FOOTER", "MARN. All rights reserved."))}</p>
                        </div>
                    </body>
                </html>";
        }

        private string BuildTwoFactorEmail(string code)
        {
            return $@"
                <!DOCTYPE html>
                <html dir='{Direction}' lang='{LanguageCode}'>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>{Html(Text("EMAIL_TWO_FACTOR_HEADING", "Two-Factor Authentication"))}</title>
                </head>
                <body style='margin:0; padding:0; background-color:#f4f6f8; font-family: Arial, sans-serif; direction:{Direction};'>
                    <table width='100%' cellpadding='0' cellspacing='0' style='background-color:#f4f6f8; padding:20px 0;'>
                        <tr>
                            <td align='center'>
                                <table width='100%' cellpadding='0' cellspacing='0' style='max-width:500px; background:#ffffff; border-radius:8px; padding:40px 30px; box-shadow:0 4px 10px rgba(0,0,0,0.05);'>
                                    <tr>
                                        <td align='center' style='font-size:22px; font-weight:bold; color:#333333; padding-bottom:10px;'>
                                            {Html(Text("EMAIL_TWO_FACTOR_HEADING", "Two-Factor Authentication"))}
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='center' style='font-size:14px; color:#555555; padding-bottom:30px;'>
                                            {Html(Text("EMAIL_TWO_FACTOR_BODY", "Use the verification code below to complete your sign-in."))}
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='center'>
                                            <div style='display:inline-block; font-size:28px; font-weight:bold; letter-spacing:6px; color:#2d3748; background:#edf2f7; padding:15px 25px; border-radius:6px;'>
                                                {Html(code)}
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align='center' style='font-size:13px; color:#888888; padding-top:30px;'>
                                            {Html(Text("EMAIL_TWO_FACTOR_EXPIRY", "This code will expire in 5 minutes."))}<br/>
                                            {Html(Text("EMAIL_TWO_FACTOR_IGNORE", "If you did not request this code, please ignore this email."))}
                                        </td>
                                    </tr>
                                </table>
                                <table width='100%' cellpadding='0' cellspacing='0' style='max-width:500px; padding-top:15px;'>
                                    <tr>
                                        <td align='center' style='font-size:12px; color:#aaaaaa;'>
                                            &copy; {DateTime.UtcNow.Year} {Html(Text("EMAIL_FOOTER", "MARN. All rights reserved."))}
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </body>
                </html>";
        }

        private string Text(string key, string fallback, params object?[] arguments)
            => _localizer.GetOrFallback(key, fallback, CultureInfo.CurrentUICulture, arguments);

        private static string Html(string? value)
            => WebUtility.HtmlEncode(value ?? string.Empty);

        private static string Attribute(string? value)
            => WebUtility.HtmlEncode(value ?? string.Empty);

        private bool IsArabic
            => string.Equals(CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, LocalizationConstants.ArabicCulture, StringComparison.OrdinalIgnoreCase);

        private string Direction => IsArabic ? "rtl" : "ltr";
        private string LanguageCode => IsArabic ? "ar" : "en";
        private string TextAlign => IsArabic ? "right" : "left";
    }
}
