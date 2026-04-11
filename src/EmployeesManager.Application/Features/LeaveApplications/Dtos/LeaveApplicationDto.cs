using EmployeesManager.Domain.Entities.LeaveApplications.Enums;

namespace EmployeesManager.Application.Features.LeaveApplications.Dtos;

public sealed record LeaveApplicationDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeName,
    Guid LeaveTypeId,
    string LeaveTypeName,
    LeaveApplicationDurations Duration,
    LeaveApplicationStatus Status,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int Days,
    string Description,
    string? Attachment,
    string? RejectionReason,
    Guid? DecisionById,
    string? DecisionBy,
    DateTimeOffset? DecisionAtUtc
);
