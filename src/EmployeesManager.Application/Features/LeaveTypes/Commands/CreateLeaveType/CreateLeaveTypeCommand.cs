using EmployeesManager.Application.Features.LeaveTypes.Common;
using EmployeesManager.Application.Features.LeaveTypes.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.LeaveTypes.Commands.CreateLeaveType;

public sealed record CreateLeaveTypeCommand(string Name, string Code)
    : IRequest<Result<Created>>,
        ILeaveTypeCommand;
