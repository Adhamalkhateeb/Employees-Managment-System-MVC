using EmployeesManager.Application.Features.Identity.Dtos;
using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Application.Common.Interfaces.Identity
{
    public interface IEmailService
    {
        Task<Result<Updated>> SendEmailConfirmationAsync(
            string email,
            string confirmationBaseUrl,
            string? returnUrl = null
        );

        Task<Result<Updated>> ConfirmEmailAsync(Guid userId, string code);

        Task<Result<Updated>> ConfirmEmailChangeAsync(Guid userId, string newEmail, string code);

        Task<Result<Updated>> SendVerificationEmailAsync(
            Guid userId,
            string confirmEmailBaseUrl,
            string? returnUrl = null
        );

        Task<Result<Updated>> SendChangeEmailLinkAsync(
            Guid userId,
            string newEmail,
            string confirmEmailChangeBaseUrl,
            string? returnUrl = null
        );
    }
}
