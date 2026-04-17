using EmployeesManager.Infrastructure.Identity;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace EmployeesManager.Infrastructure.Email;

public sealed class MailKitEmailSender : IEmailSender<AppUser>
{
    private readonly EmailSettings _settings;
    private readonly ILogger<MailKitEmailSender> _logger;

    public MailKitEmailSender(IOptions<EmailSettings> settings, ILogger<MailKitEmailSender> logger)
    {
        _settings = settings.Value;
        _logger = logger;
    }

    public Task SendConfirmationLinkAsync(AppUser user, string email, string confirmationLink) =>
        SendEmailAsync(
            to: email,
            subject: "Confirm your email address",
            htmlBody: BuildConfirmationEmailBody(user, confirmationLink)
        );

    public Task SendPasswordResetLinkAsync(AppUser user, string email, string resetLink) =>
        SendEmailAsync(
            to: email,
            subject: "Reset your password",
            htmlBody: BuildPasswordResetEmailBody(user, resetLink)
        );

    public Task SendPasswordResetCodeAsync(AppUser user, string email, string resetCode) =>
        SendEmailAsync(
            to: email,
            subject: "Your password reset code",
            htmlBody: BuildPasswordResetCodeEmailBody(user, resetCode)
        );

    // -------------------------------------------------------------------------
    // Core SMTP dispatch
    // -------------------------------------------------------------------------

    private async Task SendEmailAsync(string to, string subject, string htmlBody)
    {
        var message = BuildMimeMessage(to, subject, htmlBody);

        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(
                _settings.Host,
                _settings.Port,
                _settings.UseSsl ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls,
                CancellationToken.None
            );

            // Only authenticate if credentials are provided.
            // Some internal SMTP relays don't require auth.
            if (!string.IsNullOrWhiteSpace(_settings.UserName))
                await client.AuthenticateAsync(_settings.UserName, _settings.Password);

            await client.SendAsync(message);

            _logger.LogInformation("Email sent | To: {To} | Subject: {Subject}", to, subject);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to send email | To: {To} | Subject: {Subject}",
                to,
                subject
            );

            throw;
        }
        finally
        {
            await client.DisconnectAsync(quit: true);
        }
    }

    private MimeMessage BuildMimeMessage(string to, string subject, string htmlBody)
    {
        var message = new MimeMessage();

        message.From.Add(new MailboxAddress(_settings.FromName, _settings.FromEmail));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;

        message.Body = new BodyBuilder
        {
            HtmlBody = htmlBody,
            TextBody = HtmlToPlainText(htmlBody),
        }.ToMessageBody();

        return message;
    }

    // -------------------------------------------------------------------------
    // Email templates
    // -------------------------------------------------------------------------

    private static string BuildConfirmationEmailBody(AppUser user, string confirmationLink) =>
        $"""
            <html>
            <body style="font-family: Arial, sans-serif; color: #333; max-width: 600px; margin: 0 auto;">
                <h2>Confirm your email address</h2>
                <p>Hi {HtmlEncode(user.UserName ?? user.Email ?? "there")},</p>
                <p>Thank you for registering. Please confirm your email address by clicking the button below.</p>
                <p style="margin: 32px 0;">
                    <a href="{confirmationLink}"
                       style="background-color: #2563eb; color: white; padding: 12px 24px;
                              text-decoration: none; border-radius: 4px; font-weight: bold;">
                        Confirm Email
                    </a>
                </p>
                <p>Or copy and paste this link into your browser:</p>
                <p style="word-break: break-all; color: #2563eb;">{confirmationLink}</p>
                <hr style="margin-top: 32px; border: none; border-top: 1px solid #eee;" />
                <p style="font-size: 12px; color: #999;">
                    If you did not create an account, you can safely ignore this email.
                </p>
            </body>
            </html>
            """;

    private static string BuildPasswordResetEmailBody(AppUser user, string resetLink) =>
        $"""
            <html>
            <body style="font-family: Arial, sans-serif; color: #333; max-width: 600px; margin: 0 auto;">
                <h2>Reset your password</h2>
                <p>Hi {HtmlEncode(user.UserName ?? user.Email ?? "there")},</p>
                <p>We received a request to reset your password. Click the button below to choose a new one.</p>
                <p style="margin: 32px 0;">
                    <a href="{resetLink}"
                       style="background-color: #2563eb; color: white; padding: 12px 24px;
                              text-decoration: none; border-radius: 4px; font-weight: bold;">
                        Reset Password
                    </a>
                </p>
                <p>Or copy and paste this link into your browser:</p>
                <p style="word-break: break-all; color: #2563eb;">{resetLink}</p>
                <hr style="margin-top: 32px; border: none; border-top: 1px solid #eee;" />
                <p style="font-size: 12px; color: #999;">
                    This link expires in 1 hour. If you did not request a password reset,
                    you can safely ignore this email.
                </p>
            </body>
            </html>
            """;

    private static string BuildPasswordResetCodeEmailBody(AppUser user, string resetCode) =>
        $"""
            <html>
            <body style="font-family: Arial, sans-serif; color: #333; max-width: 600px; margin: 0 auto;">
                <h2>Your password reset code</h2>
                <p>Hi {HtmlEncode(user.UserName ?? user.Email ?? "there")},</p>
                <p>Use the code below to reset your password. It expires in 1 hour.</p>
                <p style="margin: 32px 0; font-size: 32px; font-weight: bold;
                          letter-spacing: 8px; text-align: center; color: #2563eb;">
                    {HtmlEncode(resetCode)}
                </p>
                <hr style="margin-top: 32px; border: none; border-top: 1px solid #eee;" />
                <p style="font-size: 12px; color: #999;">
                    If you did not request a password reset, you can safely ignore this email.
                </p>
            </body>
            </html>
            """;

    // -------------------------------------------------------------------------
    // Helpers
    // -------------------------------------------------------------------------

    private static string HtmlEncode(string value) => System.Net.WebUtility.HtmlEncode(value);

    private static string HtmlToPlainText(string html) =>
        System
            .Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", string.Empty)
            .Replace("&nbsp;", " ")
            .Replace("&amp;", "&")
            .Replace("&lt;", "<")
            .Replace("&gt;", ">")
            .Trim();
}
