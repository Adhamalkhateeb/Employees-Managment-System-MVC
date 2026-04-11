using EmployeesManager.Application.Features.LeaveApplications.Common;
using EmployeesManager.Application.Features.LeaveApplications.Dtos;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.LeaveApplications.Enums;
using MediatR;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.CreateLeaveApplication;

public sealed record CreateLeaveApplicationCommand(
    Guid EmployeeId,
    Guid LeaveTypeId,
    LeaveApplicationDurations Duration,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    string Description,
    string? Attachment
) : IRequest<Result<Created>>, ILeaveApplicationCommand;
