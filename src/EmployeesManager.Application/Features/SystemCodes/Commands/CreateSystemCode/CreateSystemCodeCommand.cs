using EmployeesManager.Application.Features.SystemCodes.Common;
using EmployeesManager.Application.Features.SystemCodes.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.SystemCodes.Commands.CreateSystemCode;

public sealed record CreateSystemCodeCommand(string Name, string Code)
    : IRequest<Result<SystemCodeDto>>,
        ISystemCodeCommand;
