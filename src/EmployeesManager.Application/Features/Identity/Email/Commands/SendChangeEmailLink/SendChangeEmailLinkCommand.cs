using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Email.Commands.SendChangeEmailLink;

public sealed record SendChangeEmailLinkCommand(
    string NewEmail,
    string ConfirmEmailChangeBaseUrl,
    string? ReturnUrl
) : IRequest<Result<Updated>>;
