using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Application.Common.Interfaces.Identity
{
    public interface IAuthService
    {
        Task<Result<AppUserDto>> RegisterAsync(
            string userName,
            string email,
            string phoneNumber,
            string password,
            string confirmationBaseUrl,
            string? returnUrl = null
        );

        Task<Result<AppUserDto>> LoginAsync(string email, string password, bool rememberMe = false);

        Task LogoutAsync();

        Task<Result<AppUserDto>> LoginWithAuthenticatorCodeAsync(
            string twoFactorCode,
            bool rememberMe,
            bool rememberMachine
        );

        Task<Result<AppUserDto>> LoginWithRecoveryCodeAsync(string recoveryCode);

        Task<bool> IsUserNameAvailableAsync(string userName);

        Task<bool> IsEmailAvailableAsync(string email);
    }
}
