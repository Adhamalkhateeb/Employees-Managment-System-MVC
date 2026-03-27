using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Email.Commands.ConfirmEmailChange;

public sealed record ConfirmEmailChangeCommand(Guid UserId, string Email, string Code)
    : IRequest<Result<Updated>>;
