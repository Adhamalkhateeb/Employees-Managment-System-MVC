using System.Text;
using EmployeesManager.Application.Common.Interfaces.Identity;
using EmployeesManager.Domain.Common.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace EmployeesManager.Infrastructure.Identity
{
    public sealed class EmailService : IEmailService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailSender<AppUser> _emailSender;
        private readonly ILogger<EmailService> _logger;

        public EmailService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IEmailSender<AppUser> emailSender,
            ILogger<EmailService> logger
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task<Result<Updated>> ConfirmEmailAsync(Guid userId, string code)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return Error.NotFound("Email.UserNotFound", "User not found.");

            if (user.EmailConfirmed)
                return Result.Updated;

            var decodedTokenResult = DecodeToken(code);
            if (decodedTokenResult.IsError)
                return decodedTokenResult.Errors;

            var confirmEmailResult = await _userManager.ConfirmEmailAsync(
                user,
                decodedTokenResult.Value
            );

            if (!confirmEmailResult.Succeeded)
                return confirmEmailResult.Errors.MapIdentityErrors();

            _logger.LogInformation("Email confirmed for user {UserId}", userId);

            return Result.Updated;
        }

        public async Task<Result<Updated>> ConfirmEmailChangeAsync(
            Guid userId,
            string newEmail,
            string code
        )
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                return Error.Validation(
                    "Email.NewEmailRequired",
                    "New email address is required.",
                    "NewEmail"
                );

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return Error.NotFound("Email.UserNotFound", "User not found.");

            var decodedTokenResult = DecodeToken(code);
            if (decodedTokenResult.IsError)
                return decodedTokenResult.Errors;

            var confirmEmailChangeResult = await _userManager.ChangeEmailAsync(
                user,
                newEmail,
                decodedTokenResult.Value
            );
            if (!confirmEmailChangeResult.Succeeded)
                return confirmEmailChangeResult.Errors.MapIdentityErrors();

            // Refresh the cookie so claims reflect the new email immediately.
            await _signInManager.RefreshSignInAsync(user);

            _logger.LogInformation(
                "Email changed to {NewEmail} for user {UserId}",
                newEmail,
                userId
            );

            return Result.Updated;
        }

        public async Task<Result<Updated>> SendChangeEmailLinkAsync(
            Guid userId,
            string newEmail,
            string confirmEmailChangeBaseUrl,
            string? returnUrl = null
        )
        {
            if (string.IsNullOrWhiteSpace(newEmail))
                return Error.Validation(
                    "Email.NewEmailRequired",
                    "New email address is required.",
                    "NewEmail"
                );

            if (string.IsNullOrWhiteSpace(confirmEmailChangeBaseUrl))
                return Error.Validation(
                    "Email.InvalidConfirmationBaseUrl",
                    "Email change callback URL is required.",
                    "ConfirmEmailChangeBaseUrl"
                );

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return Error.NotFound("Email.UserNotFound", "User not found.");

            if (string.Equals(user.Email, newEmail, StringComparison.OrdinalIgnoreCase))
                return Error.Validation(
                    "Email.Unchanged",
                    "The new email is the same as the current one.",
                    "NewEmail"
                );

            if (await _userManager.FindByEmailAsync(newEmail) is not null)
                return Error.Conflict(
                    "Email.AlreadyTaken",
                    "An account with this email already exists.",
                    "NewEmail"
                );

            var token = await _userManager.GenerateChangeEmailTokenAsync(user, newEmail);
            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var callbackUrl = QueryHelpers.AddQueryString(
                confirmEmailChangeBaseUrl,
                new Dictionary<string, string?>
                {
                    ["userId"] = user.Id.ToString(),
                    ["email"] = newEmail,
                    ["code"] = code,
                    ["returnUrl"] = returnUrl,
                }
            );

            await _emailSender.SendConfirmationLinkAsync(user, newEmail, callbackUrl);

            _logger.LogInformation(
                "Email change confirmation link sent to {NewEmail} for user {CurrentEmail}",
                newEmail,
                user.Email
            );

            return Result.Updated;
        }

        public async Task<Result<Updated>> SendEmailConfirmationAsync(
            string email,
            string confirmationBaseUrl,
            string? returnUrl = null
        )
        {
            if (string.IsNullOrWhiteSpace(confirmationBaseUrl))
                return Error.Validation(
                    "Email.InvalidConfirmationBaseUrl",
                    "Confirmation callback URL is required.",
                    "ConfirmationBaseUrl"
                );

            var user = await _userManager.FindByEmailAsync(email);
            if (user is null || user.EmailConfirmed)
                return Result.Updated;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var callbackUrl = QueryHelpers.AddQueryString(
                confirmationBaseUrl,
                new Dictionary<string, string?>
                {
                    ["userId"] = user.Id.ToString(),
                    ["code"] = code,
                    ["returnUrl"] = returnUrl,
                }
            );

            await _emailSender.SendConfirmationLinkAsync(user, user.Email!, callbackUrl);

            _logger.LogInformation("Verification email sent to {Email}", user.Email);

            return Result.Updated;
        }

        public async Task<Result<Updated>> SendVerificationEmailAsync(
            Guid userId,
            string confirmEmailBaseUrl,
            string? returnUrl = null
        )
        {
            if (string.IsNullOrWhiteSpace(confirmEmailBaseUrl))
                return Error.Validation(
                    "Email.InvalidConfirmationBaseUrl",
                    "Confirmation callback URL is required.",
                    "ConfirmEmailBaseUrl"
                );

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user is null)
                return Error.NotFound("Email.UserNotFound", "User not found.");

            if (user.EmailConfirmed)
                return Result.Updated;

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

            var callbackUrl = QueryHelpers.AddQueryString(
                confirmEmailBaseUrl,
                new Dictionary<string, string?>
                {
                    ["userId"] = user.Id.ToString(),
                    ["code"] = code,
                    ["returnUrl"] = returnUrl,
                }
            );

            await _emailSender.SendConfirmationLinkAsync(user, user.Email!, callbackUrl);

            _logger.LogInformation("Verification email sent to {Email}", user.Email);

            return Result.Updated;
        }

        private static Result<string> DecodeToken(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
                return Error.Validation("Email.TokenRequired", "A token is required.", "Code");

            try
            {
                var decodedBytes = WebEncoders.Base64UrlDecode(code);
                return Encoding.UTF8.GetString(decodedBytes);
            }
            catch (FormatException)
            {
                return Error.Validation("Email.InvalidToken", "Invalid token format.", "Code");
            }
        }
    }
}
