using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Identity.Email.Commands.ConfirmEmail;

public sealed record ConfirmEmailCommand(Guid UserId, string Code) : IRequest<Result<Updated>>;
