using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using MARN_API.Services.Interfaces;

namespace MARN_API.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task SendRegistrationConfirmationEmailAsync(string toEmail, string firstName, string confirmationLink)
        {
            string htmlContent = $@"
                <html>
                    <body style='font-family: Arial, sans-serif; background-color: #f4f6f8; margin:0; padding:20px;'>
                        <div style='max-width:600px; margin:auto; background:#fff; padding:30px; border-radius:8px;'>
                            <h2 style='color:#333;'>Welcome, {firstName}!</h2>
                            <p style='font-size:16px; color:#555;'>Thank you for registering. Please confirm your email by clicking the button below.</p>
                            <p style='text-align:center;'>
                                <a href='{confirmationLink}' style='background:#0d6efd; color:#fff; padding:12px 24px; border-radius:6px; text-decoration:none; font-weight:bold;'>Confirm Your Email</a>
                            </p>
                            <p style='font-size:12px; color:#999; margin-top:30px;'>&copy; {DateTime.UtcNow.Year} MARN. All rights reserved.</p>
                        </div>
                    </body>
                </html>
            ";

            await SendEmailAsync(toEmail, "Email Confirmation - MARN", htmlContent, true);
        }

        public async Task SendAccountCreatedEmailAsync(string toEmail, string firstName, string loginLink)
        {
            string htmlContent = $@"
                <html>
                    <body style='font-family: Arial, sans-serif; background-color: #f4f6f8; margin:0; padding:20px;'>
                        <div style='max-width:600px; margin:auto; background:#fff; padding:30px; border-radius:8px;'>
                            <h2 style='color:#333;'>Hello, {firstName}!</h2>
                            <p style='font-size:16px; color:#555;'>Your account has been successfully created and your email is confirmed.</p>
                            <p style='text-align:center;'>
                                <a href='{loginLink}' style='background:#198754; color:#fff; padding:12px 24px; border-radius:6px; text-decoration:none; font-weight:bold;'>Login to Your Account</a>
                            </p>
                            <p style='font-size:12px; color:#999; margin-top:30px;'>&copy; {DateTime.UtcNow.Year} MARN. All rights reserved.</p>
                        </div>
                    </body>
                </html>
            ";

            await SendEmailAsync(toEmail, "Account Created - MARN", htmlContent, true);
        }

        public async Task SendResendConfirmationEmailAsync(string toEmail, string firstName, string confirmationLink)
        {
            string htmlContent = $@"
                <html>
                    <body style='font-family: Arial, sans-serif; background-color: #f4f6f8; margin:0; padding:20px;'>
                        <div style='max-width:600px; margin:auto; background:#fff; padding:30px; border-radius:8px;'>
                            <h2 style='color:#333;'>Hello, {firstName}!</h2>
                            <p style='font-size:16px; color:#555;'>You requested a new email confirmation link. Please confirm your email by clicking the button below.</p>
                            <p style='text-align:center;'>
                                <a href='{confirmationLink}' style='background:#0d6efd; color:#fff; padding:12px 24px; border-radius:6px; text-decoration:none; font-weight:bold;'>Confirm Your Email</a>
                            </p>
                            <p style='font-size:12px; color:#999; margin-top:30px;'>&copy; {DateTime.UtcNow.Year} MARN. All rights reserved.</p>
                        </div>
                    </body>
                </html>
            ";

            await SendEmailAsync(toEmail, "Email Confirmation - MARN", htmlContent, true);
        }

        public async Task SendResetPasswordEmailAsync(string toEmail, string firstName, string resetLink)
        {
            var body = $@"
                <html>
                    <body style='font-family: Arial, sans-serif; background-color: #f4f6f8; margin:0; padding:20px;'>
                        <div style='max-width:600px; margin:auto; background:#fff; padding:30px; border-radius:8px;'>
                            <h2 style='color:#333;'>Hello, {firstName}!</h2>
                            <p style='font-size:16px; color:#555;'>You requested a Reset Password link. Please reset your password by clicking the button below.</p>
                            <p style='text-align:center;'>
                                <a href='{resetLink}' style='background:#0d6efd; color:#fff; padding:12px 24px; border-radius:6px; text-decoration:none; font-weight:bold;'>Reset Password</a>
                            </p>
                            <p style='font-size:16px; color:#555;'>This link will expire in 1 hour.</p>
                            <p style='font-size:12px; color:#999; margin-top:30px;'>&copy; {DateTime.UtcNow.Year} MARN. All rights reserved.</p>
                        </div>
                    </body>
                </html>
            ";

            await SendEmailAsync(toEmail, "Reset Password - MARN", body, true);
        }

        public async Task SendAccountDeletionEmailAsync(string toEmail, string firstName)
        {
            string supportEmail = "support@marn.com";

            string htmlContent = $@"
                    <html>
                        <body style='font-family: Arial, sans-serif; background-color: #f4f6f8; margin:0; padding:20px;'>
                            <div style='max-width:600px; margin:auto; background:#fff; padding:30px; border-radius:8px;'>
                                <h2 style='color:#333;'>Goodbye, {firstName}</h2>
                    
                                <p style='font-size:16px; color:#555;'>
                                    Your account has been successfully deleted from our platform.
                                </p>

                                <p style='font-size:16px; color:#555;'>
                                    If this action was not intended or you would like to restore your account or create a new one using the same email address, please contact our support team.
                                </p>

                                <p style='text-align:center;'>
                                    <a href='mailto:{supportEmail}' style='background:#dc3545; color:#fff; padding:12px 24px; border-radius:6px; text-decoration:none; font-weight:bold;'>
                                        Contact Support
                                    </a>
                                </p>

                                <p style='font-size:14px; color:#555; margin-top:20px;'>
                                    Support Email: {supportEmail}
                                </p>

                                <p style='font-size:12px; color:#999; margin-top:30px;'>
                                    &copy; {DateTime.UtcNow.Year} MARN. All rights reserved.
                                </p>
                            </div>
                        </body>
                    </html>
                ";

            await SendEmailAsync(toEmail, "Account Deletion Confirmation - MARN", htmlContent, true);
        }

        public async Task Send2FAEmailAsync(string toEmail, string subject, string code)
        {
            var body = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>2FA Code</title>
                </head>
                <body style='margin:0; padding:0; background-color:#f4f6f8; font-family: Arial, sans-serif;'>
                    <table width='100%' cellpadding='0' cellspacing='0' style='background-color:#f4f6f8; padding:20px 0;'>
                        <tr>
                            <td align='center'>
                                <table width='100%' cellpadding='0' cellspacing='0' 
                                       style='max-width:500px; background:#ffffff; border-radius:8px; padding:40px 30px; box-shadow:0 4px 10px rgba(0,0,0,0.05);'>
                        
                                    <tr>
                                        <td align='center' style='font-size:22px; font-weight:bold; color:#333333; padding-bottom:10px;'>
                                            Two-Factor Authentication
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align='center' style='font-size:14px; color:#555555; padding-bottom:30px;'>
                                            Use the verification code below to complete your sign-in.
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align='center'>
                                            <div style='
                                                display:inline-block;
                                                font-size:28px;
                                                font-weight:bold;
                                                letter-spacing:6px;
                                                color:#2d3748;
                                                background:#edf2f7;
                                                padding:15px 25px;
                                                border-radius:6px;'>
                                                {code}
                                            </div>
                                        </td>
                                    </tr>

                                    <tr>
                                        <td align='center' style='font-size:13px; color:#888888; padding-top:30px;'>
                                            This code will expire in 5 minutes.<br/>
                                            If you did not request this code, please ignore this email.
                                        </td>
                                    </tr>

                                </table>

                                <table width='100%' cellpadding='0' cellspacing='0' style='max-width:500px; padding-top:15px;'>
                                    <tr>
                                        <td align='center' style='font-size:12px; color:#aaaaaa;'>
                                            © {DateTime.UtcNow.Year} MARN. All rights reserved.
                                        </td>
                                    </tr>
                                </table>

                            </td>
                        </tr>
                    </table>
                </body>
                </html>";

            await SendEmailAsync(toEmail, subject, body, isBodyHtml: true);
        }

        private async Task SendEmailAsync(string toEmail, string subject, string body, bool isBodyHtml = false)
        {
            try
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
                    IsBodyHtml = isBodyHtml
                };
                message.To.Add(new MailAddress(toEmail));

                using var client = new SmtpClient(smtpServer, smtpPort)
                {
                    Credentials = new NetworkCredential(senderEmail, password),
                    EnableSsl = true
                };

                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}