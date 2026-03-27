using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Email.Commands.SendEmailConfirmation;

public sealed record SendEmailConfirmationCommand(
    string Email,
    string ConfirmationBaseUrl,
    string? ReturnUrl
) : IRequest<Result<Updated>>;
