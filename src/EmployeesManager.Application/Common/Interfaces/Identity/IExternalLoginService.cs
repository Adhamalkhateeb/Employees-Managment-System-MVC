// using EmployeesManager.Application.Features.Identity.Dtos;
// using EmployeesManager.Domain.Common.Results;

// namespace EmployeesManager.Application.Common.Interfaces.Identity;

// public interface IExternalLoginService
// {
//     Task<
//         Result<IReadOnlyList<ExternalAuthenticationSchemeDto>>
//     > GetExternalAuthenticationSchemesAsync(CancellationToken cancellationToken = default);

//     Task<Result<Updated>> ClearExternalLoginCookieAsync(
//         CancellationToken cancellationToken = default
//     );

//     Task<Result<ExternalLoginCallbackDto>> HandleExternalLoginCallbackAsync(
//         string? remoteError,
//         CancellationToken cancellationToken = default
//     );

//     Task<Result<ExternalLoginConfirmationDto>> ConfirmExternalLoginAsync(
//         string email,
//         string confirmationBaseUrl,
//         string? returnUrl = null,
//         CancellationToken cancellationToken = default
//     );

//     Task<Result<ExternalLoginsDto>> GetExternalLoginsAsync(
//         Guid userId,
//         CancellationToken cancellationToken = default
//     );

//     Task<Result<Updated>> RemoveExternalLoginAsync(
//         Guid userId,
//         string loginProvider,
//         string providerKey,
//         CancellationToken cancellationToken = default
//     );

//     Task<Result<Updated>> AddExternalLoginAsync(
//         Guid userId,
//         CancellationToken cancellationToken = default
//     );
// }
