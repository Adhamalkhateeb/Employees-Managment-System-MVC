namespace EmployeesManager.Application.Features.Identity.Dtos;

public sealed record ManageProfileDto(string UserName, string? PhoneNumber);

public sealed record ManageEmailDto(string Email, bool IsEmailConfirmed);

public sealed record TwoFactorStatusDto(
    bool HasAuthenticator,
    bool IsTwoFactorEnabled,
    int RecoveryCodesLeft,
    bool IsMachineRemembered
);

public sealed record AuthenticatorSetupDto(string SharedKey, string AuthenticatorUri);

public sealed record PersonalDataFileDto(byte[] Content, string FileName, string ContentType);

public sealed record ExternalAuthenticationSchemeDto(string Name, string DisplayName);

public sealed record ExternalUserLoginDto(string LoginProvider, string ProviderKey, string ProviderDisplayName);

public sealed record ExternalLoginsDto(
    IReadOnlyList<ExternalUserLoginDto> CurrentLogins,
    IReadOnlyList<ExternalAuthenticationSchemeDto> OtherLogins,
    bool ShowRemoveButton
);

public enum ExternalLoginCallbackStatus
{
    Succeeded,
    LockedOut,
    RequiresTwoFactor,
    RequiresConfirmation,
    Failure,
}

public sealed record ExternalLoginCallbackDto(
    ExternalLoginCallbackStatus Status,
    string? ProviderDisplayName = null,
    string? Email = null,
    string? ErrorMessage = null
);

public sealed record ExternalLoginConfirmationDto(bool RequiresConfirmedAccount);
