using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Application.Common.Interfaces.Identity;

public interface IProfileService
{
    Task<Result<ManageProfileDto>> GetProfileAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );

    Task<Result<Updated>> UpdateProfilePhoneNumberAsync(
        Guid userId,
        string? phoneNumber,
        CancellationToken cancellationToken = default
    );

    Task<Result<ManageEmailDto>> GetEmailAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );

    Task<Result<PersonalDataFileDto>> DownloadPersonalDataAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );

    Task<Result<Deleted>> DeleteAccountAsync(
        Guid userId,
        string? password,
        CancellationToken cancellationToken = default
    );

    Task<Result<string?>> GetUserNameByIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    );
}
