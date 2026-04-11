// using System.Globalization;
// using System.Reflection;
// using System.Security.Claims;
// using System.Text;
// using System.Text.Encodings.Web;
// using System.Text.Json;
// using EmployeesManager.Application.Common.Interfaces;
// using EmployeesManager.Application.Features.Identity.Dtos;
// using EmployeesManager.Domain.Common.Results;
// using EmployeesManager.Domain.Identity;
// using Microsoft.AspNetCore.Authentication;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.WebUtilities;
// using Microsoft.Extensions.Logging;

// namespace EmployeesManager.Infrastructure.Identity;

// public sealed class IdentityService : IIdentityService
// {
//     // Cache reflected personal-data properties — reflection is expensive and AppUser's
//     // properties never change at runtime.
//     private static readonly IReadOnlyList<PropertyInfo> PersonalDataProperties = typeof(AppUser)
//         .GetProperties()
//         .Where(p => Attribute.IsDefined(p, typeof(PersonalDataAttribute)))
//         .ToArray();

//     private readonly UserManager<AppUser> _userManager;
//     private readonly SignInManager<AppUser> _signInManager;
//     private readonly RoleManager<AppRole> _roleManager;
//     private readonly IHttpContextAccessor _httpContextAccessor;
//     private readonly IEmailSender<AppUser> _emailSender;
//     private readonly ILogger<IdentityService> _logger;

//     public IdentityService(
//         UserManager<AppUser> userManager,
//         SignInManager<AppUser> signInManager,
//         RoleManager<AppRole> roleManager,
//         IHttpContextAccessor httpContextAccessor,
//         IEmailSender<AppUser> emailSender,
//         ILogger<IdentityService> logger
//     )
//     {
//         _userManager = userManager;
//         _signInManager = signInManager;
//         _roleManager = roleManager;
//         _httpContextAccessor = httpContextAccessor;
//         _emailSender = emailSender;
//         _logger = logger;
//     }

//     public async Task<
//         Result<IReadOnlyList<ExternalAuthenticationSchemeDto>>
//     > GetExternalAuthenticationSchemesAsync(CancellationToken cancellationToken = default)
//     {
//         var schemes = (await _signInManager.GetExternalAuthenticationSchemesAsync())
//             .Where(s => !string.IsNullOrWhiteSpace(s.DisplayName))
//             .Select(s => new ExternalAuthenticationSchemeDto(s.Name, s.DisplayName!))
//             .ToArray();

//         return schemes;
//     }

//     public async Task<Result<Updated>> ClearExternalLoginCookieAsync(
//         CancellationToken cancellationToken = default
//     )
//     {
//         var context = _httpContextAccessor.HttpContext;
//         if (context is null)
//             return Result.Updated;

//         await context.SignOutAsync(IdentityConstants.ExternalScheme);
//         return Result.Updated;
//     }

//     public async Task<Result<ExternalLoginCallbackDto>> HandleExternalLoginCallbackAsync(
//         string? remoteError,
//         CancellationToken cancellationToken = default
//     )
//     {
//         if (!string.IsNullOrWhiteSpace(remoteError))
//             return new ExternalLoginCallbackDto(
//                 ExternalLoginCallbackStatus.Failure,
//                 ErrorMessage: $"Error from external provider: {remoteError}"
//             );

//         var info = await _signInManager.GetExternalLoginInfoAsync();
//         if (info is null)
//             return new ExternalLoginCallbackDto(
//                 ExternalLoginCallbackStatus.Failure,
//                 ErrorMessage: "Error loading external login information."
//             );

//         var result = await _signInManager.ExternalLoginSignInAsync(
//             info.LoginProvider,
//             info.ProviderKey,
//             isPersistent: false,
//             bypassTwoFactor: true
//         );

//         if (result.Succeeded)
//             return new ExternalLoginCallbackDto(ExternalLoginCallbackStatus.Succeeded);

//         if (result.IsLockedOut)
//             return new ExternalLoginCallbackDto(ExternalLoginCallbackStatus.LockedOut);

//         if (result.RequiresTwoFactor)
//             return new ExternalLoginCallbackDto(ExternalLoginCallbackStatus.RequiresTwoFactor);

//         var email = info.Principal.FindFirstValue(ClaimTypes.Email);

//         return new ExternalLoginCallbackDto(
//             ExternalLoginCallbackStatus.RequiresConfirmation,
//             ProviderDisplayName: info.ProviderDisplayName ?? info.LoginProvider,
//             Email: email
//         );
//     }

//     public async Task<Result<ExternalLoginConfirmationDto>> ConfirmExternalLoginAsync(
//         string email,
//         string confirmationBaseUrl,
//         string? returnUrl = null,
//         CancellationToken cancellationToken = default
//     )
//     {
//         if (string.IsNullOrWhiteSpace(email))
//             return Error.Validation("Identity.EmailRequired", "Email is required.");

//         if (string.IsNullOrWhiteSpace(confirmationBaseUrl))
//             return Error.Validation(
//                 "Identity.InvalidConfirmationBaseUrl",
//                 "Confirmation callback URL is required."
//             );

//         var info = await _signInManager.GetExternalLoginInfoAsync();
//         if (info is null)
//             return Error.Failure(
//                 "Identity.ExternalInfoUnavailable",
//                 "Error loading external login information during confirmation."
//             );

//         var user = new AppUser
//         {
//             UserName = email,
//             Email = email,
//             CreatedAtUtc = DateTime.UtcNow,
//         };

//         var createResult = await _userManager.CreateAsync(user);
//         if (!createResult.Succeeded)
//             return MapIdentityErrors(createResult.Errors);

//         var addLoginResult = await _userManager.AddLoginAsync(user, info);
//         if (!addLoginResult.Succeeded)
//             return MapIdentityErrors(addLoginResult.Errors);

//         var roleResult = await _userManager.AddToRoleAsync(user, Role.User.ToString());
//         if (!roleResult.Succeeded)
//             return MapIdentityErrors(roleResult.Errors);

//         var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
//         var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

//         var callbackUrl = QueryHelpers.AddQueryString(
//             confirmationBaseUrl,
//             new Dictionary<string, string?>
//             {
//                 ["userId"] = user.Id.ToString(),
//                 ["code"] = code,
//                 ["returnUrl"] = returnUrl,
//             }
//         );

//         await _emailSender.SendConfirmationLinkAsync(user, user.Email!, callbackUrl);

//         _logger.LogInformation(
//             "Email confirmation link sent to external-login user {Email}",
//             user.Email
//         );

//         if (_userManager.Options.SignIn.RequireConfirmedAccount)
//             return new ExternalLoginConfirmationDto(RequiresConfirmedAccount: true);

//         await _signInManager.SignInAsync(user, isPersistent: false, info.LoginProvider);
//         return new ExternalLoginConfirmationDto(RequiresConfirmedAccount: false);
//     }

//     public async Task<Result<Updated>> SendPasswordResetAsync(
//         string email,
//         string resetPasswordBaseUrl,
//         string? returnUrl = null,
//         CancellationToken cancellationToken = default
//     )
//     {
//         if (string.IsNullOrWhiteSpace(resetPasswordBaseUrl))
//             return Error.Validation(
//                 "Identity.InvalidResetBaseUrl",
//                 "Reset password callback URL is required."
//             );

//         var user = await _userManager.FindByEmailAsync(email);
//         if (user is null || !user.EmailConfirmed)
//             return Result.Updated;

//         var token = await _userManager.GeneratePasswordResetTokenAsync(user);
//         var code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

//         var callbackUrl = QueryHelpers.AddQueryString(
//             resetPasswordBaseUrl,
//             new Dictionary<string, string?>
//             {
//                 ["email"] = user.Email,
//                 ["code"] = code,
//                 ["returnUrl"] = returnUrl,
//             }
//         );

//         await _emailSender.SendPasswordResetLinkAsync(user, user.Email!, callbackUrl);

//         _logger.LogInformation("Password reset link sent to {Email}", user.Email);

//         return Result.Updated;
//     }

//     public async Task<Result<Updated>> ResetPasswordAsync(
//         string email,
//         string code,
//         string newPassword,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var user = await _userManager.FindByEmailAsync(email);
//         if (user is null)
//             return Error.NotFound("Identity.UserNotFound", "User not found.");

//         var tokenResult = DecodeToken(code);
//         if (tokenResult.IsError)
//             return tokenResult.Errors;

//         var result = await _userManager.ResetPasswordAsync(user, tokenResult.Value, newPassword);
//         if (!result.Succeeded)
//             return MapIdentityErrors(result.Errors);

//         return Result.Updated;
//     }

//     public async Task<Result<ManageProfileDto>> GetProfileAsync(
//         Guid userId,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         var user = userResult.Value;
//         return new ManageProfileDto(user.UserName ?? string.Empty, user.PhoneNumber);
//     }

//     public async Task<Result<Updated>> UpdateProfilePhoneNumberAsync(
//         Guid userId,
//         string? phoneNumber,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         var user = userResult.Value;
//         if (string.Equals(user.PhoneNumber, phoneNumber, StringComparison.Ordinal))
//             return Result.Updated;

//         var result = await _userManager.SetPhoneNumberAsync(user, phoneNumber);
//         if (!result.Succeeded)
//             return MapIdentityErrors(result.Errors);

//         await _signInManager.RefreshSignInAsync(user);
//         return Result.Updated;
//     }

//     public async Task<Result<ManageEmailDto>> GetEmailAsync(
//         Guid userId,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         var user = userResult.Value;
//         return new ManageEmailDto(user.Email ?? string.Empty, user.EmailConfirmed);
//     }

//     public async Task<Result<ExternalLoginsDto>> GetExternalLoginsAsync(
//         Guid userId,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         var user = userResult.Value;
//         var currentLogins = (await _userManager.GetLoginsAsync(user))
//             .Select(l => new ExternalUserLoginDto(
//                 l.LoginProvider,
//                 l.ProviderKey,
//                 l.ProviderDisplayName ?? l.LoginProvider
//             ))
//             .ToArray();

//         var allProvidersResult = await GetExternalAuthenticationSchemesAsync(cancellationToken);
//         if (allProvidersResult.IsError)
//             return allProvidersResult.Errors;

//         var otherLogins = allProvidersResult
//             .Value.Where(p => currentLogins.All(c => c.LoginProvider != p.Name))
//             .ToArray();

//         var hasPassword = await _userManager.HasPasswordAsync(user);
//         var showRemoveButton = hasPassword || currentLogins.Length > 1;

//         return new ExternalLoginsDto(currentLogins, otherLogins, showRemoveButton);
//     }

//     public async Task<Result<Updated>> RemoveExternalLoginAsync(
//         Guid userId,
//         string loginProvider,
//         string providerKey,
//         CancellationToken cancellationToken = default
//     )
//     {
//         if (string.IsNullOrWhiteSpace(loginProvider) || string.IsNullOrWhiteSpace(providerKey))
//             return Error.Validation(
//                 "Identity.ExternalLoginRequired",
//                 "External login provider and key are required."
//             );

//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         var user = userResult.Value;
//         var result = await _userManager.RemoveLoginAsync(user, loginProvider, providerKey);
//         if (!result.Succeeded)
//             return MapIdentityErrors(result.Errors);

//         await _signInManager.RefreshSignInAsync(user);
//         return Result.Updated;
//     }

//     public async Task<Result<Updated>> AddExternalLoginAsync(
//         Guid userId,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         var user = userResult.Value;
//         var info = await _signInManager.GetExternalLoginInfoAsync();
//         if (info is null)
//             return Error.Failure(
//                 "Identity.ExternalInfoUnavailable",
//                 "Unexpected error occurred loading external login info."
//             );

//         var result = await _userManager.AddLoginAsync(user, info);
//         if (!result.Succeeded)
//             return MapIdentityErrors(result.Errors);

//         var context = _httpContextAccessor.HttpContext;
//         if (context is not null)
//             await context.SignOutAsync(IdentityConstants.ExternalScheme);

//         await _signInManager.RefreshSignInAsync(user);
//         return Result.Updated;
//     }

//     public async Task<Result<bool>> HasPasswordAsync(
//         Guid userId,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         return await _userManager.HasPasswordAsync(userResult.Value);
//     }

//     public async Task<Result<Updated>> ChangePasswordAsync(
//         Guid userId,
//         string currentPassword,
//         string newPassword,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         var user = userResult.Value;
//         var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
//         if (!result.Succeeded)
//             return MapIdentityErrors(result.Errors);

//         await _signInManager.RefreshSignInAsync(user);
//         return Result.Updated;
//     }

//     public async Task<Result<Updated>> SetPasswordAsync(
//         Guid userId,
//         string newPassword,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         var user = userResult.Value;
//         var result = await _userManager.AddPasswordAsync(user, newPassword);
//         if (!result.Succeeded)
//             return MapIdentityErrors(result.Errors);

//         await _signInManager.RefreshSignInAsync(user);
//         return Result.Updated;
//     }

//     public async Task<Result<TwoFactorStatusDto>> GetTwoFactorStatusAsync(
//         Guid userId,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         var user = userResult.Value;
//         var authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);
//         var hasAuthenticator = !string.IsNullOrWhiteSpace(authenticatorKey);
//         var isTwoFactorEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
//         var recoveryCodesLeft = await _userManager.CountRecoveryCodesAsync(user);
//         var isMachineRemembered = await _signInManager.IsTwoFactorClientRememberedAsync(user);

//         return new TwoFactorStatusDto(
//             hasAuthenticator,
//             isTwoFactorEnabled,
//             recoveryCodesLeft,
//             isMachineRemembered
//         );
//     }

//     public async Task<Result<Updated>> ForgetTwoFactorClientAsync(
//         Guid userId,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         await _signInManager.ForgetTwoFactorClientAsync();
//         return Result.Updated;
//     }

//     public async Task<Result<AuthenticatorSetupDto>> GetAuthenticatorSetupAsync(
//         Guid userId,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         var user = userResult.Value;
//         var key = await _userManager.GetAuthenticatorKeyAsync(user);

//         if (string.IsNullOrWhiteSpace(key))
//         {
//             await _userManager.ResetAuthenticatorKeyAsync(user);
//             key = await _userManager.GetAuthenticatorKeyAsync(user);
//         }

//         if (string.IsNullOrWhiteSpace(key))
//             return Error.Unexpected(
//                 "Identity.AuthenticatorKeyUnavailable",
//                 "Authenticator key could not be generated."
//             );

//         var email = user.Email ?? user.UserName ?? "user";
//         return new AuthenticatorSetupDto(FormatKey(key), GenerateQrCodeUri(email, key));
//     }

//     public async Task<Result<IReadOnlyList<string>>> EnableTwoFactorAsync(
//         Guid userId,
//         string verificationCode,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         var user = userResult.Value;
//         var normalizedCode = verificationCode.Replace(" ", string.Empty).Replace("-", string.Empty);

//         var isValid = await _userManager.VerifyTwoFactorTokenAsync(
//             user,
//             _userManager.Options.Tokens.AuthenticatorTokenProvider,
//             normalizedCode
//         );

//         if (!isValid)
//             return Error.Validation(
//                 "Identity.InvalidAuthenticatorCode",
//                 "Verification code is invalid."
//             );

//         var enableResult = await _userManager.SetTwoFactorEnabledAsync(user, true);
//         if (!enableResult.Succeeded)
//             return MapIdentityErrors(enableResult.Errors);

//         // Always regenerate recovery codes on enable so previously-disabled users
//         // do not retain stale codes from a prior session.
//         var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
//         return (recoveryCodes ?? Array.Empty<string>()).ToArray();
//     }

//     public async Task<Result<Updated>> DisableTwoFactorAsync(
//         Guid userId,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         var user = userResult.Value;
//         var isEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
//         if (!isEnabled)
//             return Error.Validation(
//                 "Identity.TwoFactorNotEnabled",
//                 "Two-factor authentication is not enabled for this user."
//             );

//         var disableResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
//         if (!disableResult.Succeeded)
//             return MapIdentityErrors(disableResult.Errors);

//         return Result.Updated;
//     }

//     public async Task<Result<IReadOnlyList<string>>> GenerateRecoveryCodesAsync(
//         Guid userId,
//         int number = 10,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         var user = userResult.Value;
//         var isEnabled = await _userManager.GetTwoFactorEnabledAsync(user);
//         if (!isEnabled)
//             return Error.Validation(
//                 "Identity.TwoFactorNotEnabled",
//                 "Cannot generate recovery codes because 2FA is not enabled."
//             );

//         var recoveryCodes = await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, number);
//         return (recoveryCodes ?? Array.Empty<string>()).ToArray();
//     }

//     public async Task<Result<Updated>> ResetAuthenticatorAsync(
//         Guid userId,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         var user = userResult.Value;

//         var disableResult = await _userManager.SetTwoFactorEnabledAsync(user, false);
//         if (!disableResult.Succeeded)
//             return MapIdentityErrors(disableResult.Errors);

//         await _userManager.ResetAuthenticatorKeyAsync(user);
//         await _signInManager.RefreshSignInAsync(user);
//         return Result.Updated;
//     }

//     public async Task<Result<Deleted>> DeleteAccountAsync(
//         Guid userId,
//         string? password,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         var user = userResult.Value;
//         var hasPassword = await _userManager.HasPasswordAsync(user);

//         if (hasPassword)
//         {
//             if (string.IsNullOrWhiteSpace(password))
//                 return Error.Validation("Identity.PasswordRequired", "Password is required.");

//             var passwordOk = await _userManager.CheckPasswordAsync(user, password);
//             if (!passwordOk)
//                 return Error.Validation("Identity.PasswordIncorrect", "Incorrect password.");
//         }

//         var result = await _userManager.DeleteAsync(user);
//         if (!result.Succeeded)
//             return MapIdentityErrors(result.Errors);

//         // Guard against null HTTP context (background jobs, tests, expired sessions).
//         var context = _httpContextAccessor.HttpContext;
//         if (context is not null)
//             await _signInManager.SignOutAsync();

//         return Result.Deleted;
//     }

//     public async Task<Result<PersonalDataFileDto>> DownloadPersonalDataAsync(
//         Guid userId,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var userResult = await GetUserByIdAsync(userId);
//         if (userResult.IsError)
//             return userResult.Errors;

//         var user = userResult.Value;
//         var personalData = new Dictionary<string, string?>();

//         // Use the cached reflected properties (computed once at startup).
//         foreach (var property in PersonalDataProperties)
//             personalData.Add(property.Name, property.GetValue(user)?.ToString() ?? "null");

//         var logins = await _userManager.GetLoginsAsync(user);
//         foreach (var login in logins)
//             personalData.Add(
//                 $"{login.LoginProvider} external login provider key",
//                 login.ProviderKey
//             );

//         var authenticatorKey = await _userManager.GetAuthenticatorKeyAsync(user);
//         personalData.Add("Authenticator Key", authenticatorKey);

//         var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(personalData));

//         return new PersonalDataFileDto(
//             Content: bytes,
//             FileName: $"PersonalData-{user.UserName ?? user.Id.ToString()}.json",
//             ContentType: "application/json"
//         );
//     }

//     public async Task<Result<Updated>> AssignRoleAsync(Guid userId, Role role)
//     {
//         // Check user existence before role existence to avoid leaking role information.
//         var user = await _userManager.FindByIdAsync(userId.ToString());
//         if (user is null)
//             return Error.NotFound("Identity.UserNotFound", "User not found.");

//         var roleName = role.ToString();
//         if (!await _roleManager.RoleExistsAsync(roleName))
//             return Error.Validation("Identity.RoleNotFound", $"Role '{roleName}' does not exist.");

//         if (await _userManager.IsInRoleAsync(user, roleName))
//             return Error.Conflict(
//                 "Identity.AlreadyInRole",
//                 $"User is already in role '{roleName}'."
//             );

//         var result = await _userManager.AddToRoleAsync(user, roleName);
//         if (!result.Succeeded)
//             return MapIdentityErrors(result.Errors);

//         return Result.Updated;
//     }

//     public async Task<Result<Updated>> RemoveRoleAsync(Guid userId, Role role)
//     {
//         // Check user existence before role existence to avoid leaking role information.
//         var user = await _userManager.FindByIdAsync(userId.ToString());
//         if (user is null)
//             return Error.NotFound("Identity.UserNotFound", "User not found.");

//         var roleName = role.ToString();
//         if (!await _roleManager.RoleExistsAsync(roleName))
//             return Error.Validation("Identity.RoleNotFound", $"Role '{roleName}' does not exist.");

//         var currentRoles = await _userManager.GetRolesAsync(user);
//         if (currentRoles.Count == 1 && currentRoles[0] == roleName)
//             return Error.Conflict("Identity.LastRole", "Cannot remove the user's only role.");

//         var result = await _userManager.RemoveFromRoleAsync(user, roleName);
//         if (!result.Succeeded)
//             return MapIdentityErrors(result.Errors);

//         return Result.Updated;
//     }

//     public async Task<Result<string?>> GetUserNameByIdAsync(
//         Guid userId,
//         CancellationToken cancellationToken = default
//     )
//     {
//         var user = await _userManager.FindByIdAsync(userId.ToString());
//         return user?.UserName ?? string.Empty;
//     }

//     // -------------------------------------------------------------------------
//     // Private helpers
//     // -------------------------------------------------------------------------

//     private async Task<Result<AppUser>> GetUserByIdAsync(Guid userId)
//     {
//         if (userId == Guid.Empty)
//             return Error.Validation("Identity.UserIdRequired", "User id is required.");

//         var user = await _userManager.FindByIdAsync(userId.ToString());
//         if (user is null)
//             return Error.NotFound("Identity.UserNotFound", "User not found.");

//         return user;
//     }

//     private static Result<string> DecodeToken(string code)
//     {
//         if (string.IsNullOrWhiteSpace(code))
//             return Error.Validation("Identity.TokenRequired", "A token is required.");

//         try
//         {
//             var decodedBytes = WebEncoders.Base64UrlDecode(code);
//             return Encoding.UTF8.GetString(decodedBytes);
//         }
//         catch (FormatException)
//         {
//             return Error.Validation("Identity.InvalidToken", "Invalid token format.");
//         }
//     }

//     private static List<Error> MapIdentityErrors(IEnumerable<IdentityError> errors) =>
//         [
//             .. errors.Select(e =>
//                 e.Code is "DuplicateEmail" or "DuplicateUserName"
//                     ? Error.Conflict(e.Code, e.Description)
//                     : Error.Validation(e.Code, e.Description)
//             ),
//         ];

//     private static string FormatKey(string unformattedKey)
//     {
//         var result = new StringBuilder();
//         var currentPosition = 0;

//         while (currentPosition + 4 < unformattedKey.Length)
//         {
//             result.Append(unformattedKey.AsSpan(currentPosition, 4)).Append(' ');
//             currentPosition += 4;
//         }

//         if (currentPosition < unformattedKey.Length)
//             result.Append(unformattedKey.AsSpan(currentPosition));

//         return result.ToString().ToLowerInvariant();
//     }

//     private static string GenerateQrCodeUri(string email, string unformattedKey)
//     {
//         const string issuer = "EmployeesManager";
//         var encodedIssuer = UrlEncoder.Default.Encode(issuer);
//         var encodedEmail = UrlEncoder.Default.Encode(email);

//         return $"otpauth://totp/{encodedIssuer}:{encodedEmail}?secret={unformattedKey}&issuer={encodedIssuer}&digits=6";
//     }
// }
