namespace EmployeesManager.Contracts.Responses.LeaveApplications;

public sealed record LeaveApplicationResponse(
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
