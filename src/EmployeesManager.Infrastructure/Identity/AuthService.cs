using EmployeesManager.Application.Common.Interfaces.Identity;
using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace EmployeesManager.Infrastructure.Identity
{
    public sealed class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            IEmailService emailService,
            ILogger<AuthService> logger
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _logger = logger;
        }

        public async Task<bool> IsEmailAvailableAsync(string email) =>
            await _userManager.FindByEmailAsync(email) is null;

        public async Task<bool> IsUserNameAvailableAsync(string userName) =>
            await _userManager.FindByNameAsync(userName) is null;

        public async Task<Result<AppUserDto>> LoginAsync(
            string email,
            string password,
            bool rememberMe = false
        )
        {
            if (await _userManager.FindByEmailAsync(email) is not AppUser user)
                return Error.Unauthorized("Auth.InvalidCredentials", "Invalid email or password.");

            if (!user.EmailConfirmed)
                return Error.Forbidden(
                    "Auth.EmailNotConfirmed",
                    "Please confirm your email before logging in."
                );

            var signResult = await _signInManager.PasswordSignInAsync(
                user,
                password,
                rememberMe,
                lockoutOnFailure: true
            );

            if (signResult.IsLockedOut)
                return Error.Forbidden(
                    "Auth.AccountLocked",
                    "Your account is locked due to multiple failed login attempts. Please try again later."
                );

            if (signResult.RequiresTwoFactor)
                return Error.Forbidden(
                    "Auth.TwoFactorRequired",
                    "Two-factor authentication is required. Please provide a two-factor code."
                );

            if (!signResult.Succeeded)
                return Error.Unauthorized("Auth.InvalidCredentials", "Invalid email or password.");

            _logger.LogInformation("User logged in: {Email}", email);

            var roles = await _userManager.GetRolesAsync(user);
            return new AppUserDto(
                user.Id,
                user.UserName!,
                user.Email!,
                user.PhoneNumber!,
                roles.ToList()
            );
        }

        public async Task<Result<AppUserDto>> LoginWithAuthenticatorCodeAsync(
            string twoFactorCode,
            bool rememberMe,
            bool rememberMachine
        )
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user is null)
                return Error.Unauthorized(
                    "Auth.InvalidTwoFactor",
                    "Invalid two-factor authentication code."
                );

            var normalizedCode = twoFactorCode
                .Replace(" ", string.Empty)
                .Replace("-", string.Empty);

            var signInResult = await _signInManager.TwoFactorAuthenticatorSignInAsync(
                normalizedCode,
                rememberMe,
                rememberMachine
            );

            if (signInResult.IsLockedOut)
                return Error.Forbidden("Auth.LockedOut", "Account is locked. Try again later.");

            if (!signInResult.Succeeded)
                return Error.Validation(
                    "Auth.InvalidAuthenticatorCode",
                    "Invalid authenticator code.",
                    "TwoFactorCode"
                );

            _logger.LogInformation(
                "User completed 2FA login with authenticator: {UserId}",
                user.Id
            );

            var roles = await _userManager.GetRolesAsync(user);

            return new AppUserDto(
                UserId: user.Id,
                UserName: user.UserName ?? string.Empty,
                Email: user.Email ?? string.Empty,
                PhoneNumber: user.PhoneNumber ?? string.Empty,
                Roles: roles
            );
        }

        public async Task<Result<AppUserDto>> LoginWithRecoveryCodeAsync(string recoveryCode)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user is null)
                return Error.Unauthorized(
                    "Auth.TwoFactorSessionExpired",
                    "Two-factor session expired. Please log in again."
                );

            var normalizedCode = recoveryCode.Replace(" ", string.Empty);
            var signInResult = await _signInManager.TwoFactorRecoveryCodeSignInAsync(
                normalizedCode
            );

            if (signInResult.IsLockedOut)
                return Error.Forbidden("Auth.LockedOut", "Account is locked. Try again later.");

            if (!signInResult.Succeeded)
                return Error.Validation(
                    "Auth.InvalidRecoveryCode",
                    "Invalid recovery code.",
                    "RecoveryCode"
                );

            _logger.LogInformation(
                "User completed 2FA login with recovery code: {UserId}",
                user.Id
            );

            var roles = await _userManager.GetRolesAsync(user);

            return new AppUserDto(
                UserId: user.Id,
                UserName: user.UserName ?? string.Empty,
                Email: user.Email ?? string.Empty,
                PhoneNumber: user.PhoneNumber ?? string.Empty,
                Roles: roles
            );
        }

        public async Task LogoutAsync() => await _signInManager.SignOutAsync();

        public async Task<Result<AppUserDto>> RegisterAsync(
            string userName,
            string email,
            string phoneNumber,
            string password,
            string confirmationBaseUrl,
            string? returnUrl = null
        )
        {
            if (await _userManager.FindByEmailAsync(email) is not null)
                return Error.Conflict(
                    "Auth.EmailTaken",
                    "An account with this email already exists.",
                    "Email"
                );

            if (await _userManager.FindByNameAsync(userName) is not null)
                return Error.Conflict(
                    "Auth.UserNameTaken",
                    "An account with this username already exists.",
                    "UserName"
                );

            var user = new AppUser
            {
                UserName = userName,
                Email = email,
                PhoneNumber = phoneNumber,
                CreatedAtUtc = DateTime.UtcNow,
            };

            var createResult = await _userManager.CreateAsync(user, password);

            if (!createResult.Succeeded)
                return createResult.Errors.MapIdentityErrors();

            var roleResult = await _userManager.AddToRoleAsync(user, Role.User.ToString());

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(user);
                return roleResult.Errors.MapIdentityErrors();
            }

            _logger.LogInformation("New user registered: {Email}", email);

            _ = _emailService.SendEmailConfirmationAsync(email, confirmationBaseUrl, returnUrl);

            var roles = await _userManager.GetRolesAsync(user);
            return new AppUserDto(
                user.Id,
                user.UserName,
                user.Email,
                user.PhoneNumber,
                roles.ToList()
            );
        }
    }
}
