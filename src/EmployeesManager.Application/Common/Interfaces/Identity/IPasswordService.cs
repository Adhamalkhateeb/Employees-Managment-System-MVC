using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Application.Common.Interfaces.Identity;

public interface IPasswordService
{
    Task<Result<Updated>> SendPasswordResetAsync(
        string email,
        string resetPasswordBaseUrl,
        string? returnUrl = null,
        CancellationToken cancellationToken = default
    );

    Task<Result<Updated>> ResetPasswordAsync(
        string email,
        string code,
        string newPassword,
        CancellationToken cancellationToken = default
    );

    Task<Result<bool>> HasPasswordAsync(Guid userId, CancellationToken cancellationToken = default);

    Task<Result<Updated>> ChangePasswordAsync(
        Guid userId,
        string currentPassword,
        string newPassword,
        CancellationToken cancellationToken = default
    );

    Task<Result<Updated>> SetPasswordAsync(
        Guid userId,
        string newPassword,
        CancellationToken cancellationToken = default
    );
}
