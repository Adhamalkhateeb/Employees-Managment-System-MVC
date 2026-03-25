using EmployeesManager.Application.Features.LeaveApplications.Common;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.UpdateLeaveApplication;

public sealed record UpdateLeaveApplicationCommand(
    Guid Id,
    Guid EmployeeId,
    Guid LeaveTypeId,
    Guid DurationId,
    Guid StatusId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string Description,
    string? Attachment
) : IRequest<Result<Updated>>, ILeaveApplicationCommand;
