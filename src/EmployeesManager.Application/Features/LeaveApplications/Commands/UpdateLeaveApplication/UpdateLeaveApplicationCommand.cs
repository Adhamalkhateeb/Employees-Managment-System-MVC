using EmployeesManager.Application.Features.LeaveApplications.Common;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.LeaveApplications.Enums;
using MediatR;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.UpdateLeaveApplication;

public sealed record UpdateLeaveApplicationCommand(
    Guid Id,
    Guid EmployeeId,
    Guid LeaveTypeId,
    LeaveApplicationDurations Duration,
    LeaveApplicationStatus Status,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string Description,
    string? Attachment
) : IRequest<Result<Updated>>, ILeaveApplicationCommand;
