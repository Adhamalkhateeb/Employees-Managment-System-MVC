using EmployeesManager.Application.Features.LeaveApplications.Common;
using EmployeesManager.Application.Features.LeaveApplications.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.CreateLeaveApplication;

public sealed record CreateLeaveApplicationCommand(
    Guid EmployeeId,
    Guid LeaveTypeId,
    Guid DurationId,
    Guid StatusId,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string Description,
    string? Attachment
) : IRequest<Result<LeaveApplicationDto>>, ILeaveApplicationCommand;
