using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.SystemCodes.Commands.DeleteSystemCode;

public sealed record DeleteSystemCodeCommand(Guid Id) : IRequest<Result<Deleted>>;
