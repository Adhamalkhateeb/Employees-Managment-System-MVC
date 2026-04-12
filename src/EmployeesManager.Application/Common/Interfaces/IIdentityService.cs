using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Identity;

namespace EmployeesManager.Application.Common.Interfaces;

public interface IIdentityService
{
    Task<Result<AuthenticatorSetupDto>> GetAuthenticatorSetupAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );

    Task<Result<IReadOnlyList<string>>> GenerateRecoveryCodesAsync(
        Guid userId,
        int number = 10,
        CancellationToken cancellationToken = default
    );

    Task<Result<Updated>> ResetAuthenticatorAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );

    Task<Result<string?>> GetUserNameByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );

    Task<bool> IsUserNameAvailableAsync(string userName);
}
