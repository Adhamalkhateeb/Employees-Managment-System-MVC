using EmployeesManager.Application.Features.LeaveTypes.Common;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.LeaveTypes.Commands.UpdateLeaveType;

public sealed record UpdateLeaveTypeCommand(Guid Id, string Name, string Code)
    : IRequest<Result<Updated>>,
        ILeaveTypeCommand;
