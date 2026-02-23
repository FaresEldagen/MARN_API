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

        public async Task SendResetPasswordEmailAsync(string to, string resetLink)
        {
            var body = $@"
    <h2>Password Reset</h2>
    <p>Click the link below to reset your password:</p>
    <a href='{resetLink}'>Reset Password</a>
    <p>This link will expire in 1 hour.</p>
";
            await SendEmailAsync(to, "Reset Password - MARN", body, true);

        }
    }
}