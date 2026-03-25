namespace EmployeesManager.Application.Features.LeaveApplications.Dtos;

public sealed record LeaveApplicationDto(
    Guid Id,
    Guid EmployeeId,
    string EmployeeName,
    Guid LeaveTypeId,
    string LeaveTypeName,
    Guid DurationId,
    string DurationName,
    Guid StatusId,
    string StatusName,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int Days,
    string Description,
    string? Attachment,
    string? ApprovedBy,
    DateTimeOffset? ApprovedAtUtc
);
