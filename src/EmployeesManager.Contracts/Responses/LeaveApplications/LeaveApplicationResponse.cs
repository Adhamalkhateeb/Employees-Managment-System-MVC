namespace EmployeesManager.Contracts.Responses.LeaveApplications;

public sealed record LeaveApplicationResponse(
    Guid Id,
    Guid EmployeeId,
    string EmployeeName,
    Guid LeaveTypeId,
    string LeaveTypeName,
    Guid DurationId,
    string DurationName,
    string Status,
    DateTimeOffset StartDate,
    DateTimeOffset EndDate,
    int Days,
    string Description,
    string? Attachment,
    string? RejectionReason,
    string? ApprovedBy,
    DateTimeOffset? ApprovedAtUtc
);
