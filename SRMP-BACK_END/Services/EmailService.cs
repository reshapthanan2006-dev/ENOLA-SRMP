using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using SRMP.Interfaces;
using SRMP.Settings;

namespace SRMP.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _settings;

        public EmailService(
            IOptions<EmailSettings> options)
        {
            _settings = options.Value;
        }


        public async Task SendPasswordResetEmailAsync(
            string recipientEmail,
            string recipientName,
            string resetToken)
        {
            var encodedToken =
                Uri.EscapeDataString(resetToken);

            var resetLink =
                $"{_settings.FrontendUrl.TrimEnd('/')}/reset-password?token={encodedToken}";

            var safeName =
                WebUtility.HtmlEncode(recipientName);

            var safeResetLink =
                WebUtility.HtmlEncode(resetLink);


            var message = new MimeMessage();

            message.From.Add(
                new MailboxAddress(
                    _settings.SenderName,
                    _settings.SenderEmail));

            message.To.Add(
                new MailboxAddress(
                    recipientName,
                    recipientEmail));

            message.Subject =
                "Reset your ENOLA SRMP password";


            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = $"""
                    <!DOCTYPE html>
                    <html>
                    <body style="
                        margin:0;
                        padding:30px;
                        background:#f7f3fa;
                        font-family:Arial, sans-serif;
                        color:#292432;
                    ">

                        <div style="
                            max-width:560px;
                            margin:0 auto;
                            background:#ffffff;
                            padding:32px;
                            border-radius:16px;
                            border:1px solid #e8dfea;
                        ">

                            <div style="
                                font-size:13px;
                                font-weight:700;
                                letter-spacing:2px;
                                color:#8576ab;
                                margin-bottom:16px;
                            ">
                                ENOLA SRMP
                            </div>

                            <h2 style="
                                margin:0 0 16px;
                                color:#292432;
                            ">
                                Reset your password
                            </h2>

                            <p>
                                Hello {safeName},
                            </p>

                            <p style="
                                line-height:1.6;
                                color:#655c6d;
                            ">
                                We received a request to reset your
                                ENOLA SRMP account password.
                            </p>

                            <div style="
                                margin:28px 0;
                            ">

                                <a
                                    href="{safeResetLink}"
                                    style="
                                        display:inline-block;
                                        padding:13px 24px;
                                        border-radius:10px;
                                        background:#8576ab;
                                        color:#ffffff;
                                        text-decoration:none;
                                        font-weight:700;
                                    "
                                >
                                    Reset Password
                                </a>

                            </div>

                            <p style="
                                line-height:1.6;
                                color:#655c6d;
                                font-size:14px;
                            ">
                                This reset link expires in 30 minutes
                                and can only be used once.
                            </p>

                            <p style="
                                line-height:1.6;
                                color:#655c6d;
                                font-size:14px;
                            ">
                                If you did not request a password reset,
                                you can ignore this email.
                            </p>

                        </div>

                    </body>
                    </html>
                    """,

                TextBody =
                    $"""
                    Hello {recipientName},

                    We received a request to reset your ENOLA SRMP password.

                    Reset your password here:
                    {resetLink}

                    This link expires in 30 minutes and can only be used once.

                    If you did not request this, you can ignore this email.
                    """
            };

            message.Body =
                bodyBuilder.ToMessageBody();


            using var smtpClient =
                new SmtpClient();

            await smtpClient.ConnectAsync(
                _settings.SmtpServer,
                _settings.Port,
                SecureSocketOptions.StartTls);

            await smtpClient.AuthenticateAsync(
                _settings.Username,
                _settings.Password);

            await smtpClient.SendAsync(
                message);

            await smtpClient.DisconnectAsync(
                true);
        }
    }
}