using EmployeesManager.Application.Features.SystemCodes.Common;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.SystemCodes.Commands.UpdateSystemCode;

public sealed record UpdateSystemCodeCommand(Guid Id, string Code, string? Description)
    : IRequest<Result<Updated>>,
        ISystemCodeCommand;
