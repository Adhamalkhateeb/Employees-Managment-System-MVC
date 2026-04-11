using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Application.Common.Interfaces.Identity;

public interface ITwoFactorService
{
    Task<Result<AuthenticatorSetupDto>> GetAuthenticatorSetupAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );

    Task<Result<Updated>> ResetAuthenticatorAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );

    Task<Result<TwoFactorStatusDto>> GetTwoFactorStatusAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );

    Task<Result<IReadOnlyList<string>>> EnableTwoFactorAsync(
        Guid userId,
        string verificationCode,
        CancellationToken cancellationToken = default
    );

    Task<Result<Updated>> DisableTwoFactorAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );

    Task<Result<Updated>> ForgetTwoFactorClientAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );

    Task<Result<IReadOnlyList<string>>> GenerateRecoveryCodesAsync(
        Guid userId,
        int number = 10,
        CancellationToken cancellationToken = default
    );
}
