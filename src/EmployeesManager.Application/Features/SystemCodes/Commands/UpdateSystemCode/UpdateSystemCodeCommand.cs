using EmployeesManager.Application.Features.SystemCodes.Common;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.SystemCodes.Commands.UpdateSystemCode;

public sealed record UpdateSystemCodeCommand(Guid Id, string Name, string Code)
    : IRequest<Result<Updated>>,
        ISystemCodeCommand;
