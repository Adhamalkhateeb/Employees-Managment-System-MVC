using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Employees;
using EmployeesManager.Domain.Entities.LeaveTypes;
using EmployeesManager.Domain.Entities.SystemCodeDetails;

namespace EmployeesManager.Domain.Entities.LeaveApplications;

public sealed class LeaveApplication : ApprovalActivity
{
    public Guid EmployeeId { get; private set; }
    public Guid LeaveTypeId { get; private set; }
    public Guid DurationId { get; private set; }
    public Guid StatusId { get; private set; }
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }
    public int Days => (EndDate.Date - StartDate.Date).Days + 1;
    public string? Attachment { get; private set; }
    public string Description { get; private set; } = default!;
    public SystemCodeDetail Duration { get; private set; } = default!;
    public SystemCodeDetail Status { get; private set; } = default!;
    public Employee Employee { get; private set; } = default!;
    public LeaveType LeaveType { get; private set; } = default!;

    private LeaveApplication() { }

    private LeaveApplication(
        Guid id,
        Guid employeeId,
        Guid leaveTypeId,
        Guid durationId,
        Guid statusId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string description,
        string? attachment
    )
        : base(id) { }

    public static Result<LeaveApplication> Create(
        Guid employeeId,
        Guid leaveTypeId,
        Guid durationId,
        Guid statusId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string description,
        string? attachment
    )
    {
        var validationError = Validate(
            employeeId,
            leaveTypeId,
            durationId,
            statusId,
            startDate,
            endDate,
            description,
            attachment
        );

        if (validationError is not null)
            return validationError;

        return new LeaveApplication(
            Guid.NewGuid(),
            employeeId,
            leaveTypeId,
            durationId,
            statusId,
            startDate,
            endDate,
            description.Trim(),
            attachment?.Trim()
        );
    }

    public Result<Updated> Update(
        Guid employeeId,
        Guid leaveTypeId,
        Guid durationId,
        Guid statusId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string description,
        string? attachment
    )
    {
        var validationError = Validate(
            employeeId,
            leaveTypeId,
            durationId,
            statusId,
            startDate,
            endDate,
            description,
            attachment
        );

        if (validationError is not null)
            return validationError;

        EmployeeId = employeeId;
        LeaveTypeId = leaveTypeId;
        DurationId = durationId;
        StatusId = statusId;
        StartDate = startDate;
        EndDate = endDate;
        Description = description.Trim();
        Attachment = string.IsNullOrWhiteSpace(attachment) ? null : attachment.Trim();

        return Result.Updated;
    }

    private static Error? Validate(
        Guid employeeId,
        Guid leaveTypeId,
        Guid durationId,
        Guid statusId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string description,
        string? attachment
    )
    {
        if (employeeId == Guid.Empty)
            return LeaveApplicationErrors.EmployeeRequired;

        if (leaveTypeId == Guid.Empty)
            return LeaveApplicationErrors.LeaveTypeRequired;

        if (durationId == Guid.Empty)
            return LeaveApplicationErrors.DurationRequired;

        if (statusId == Guid.Empty)
            return LeaveApplicationErrors.StatusRequired;

        if (startDate == DateTimeOffset.MinValue)
            return LeaveApplicationErrors.StartDateRequired;

        if (endDate == DateTimeOffset.MinValue)
            return LeaveApplicationErrors.EndDateRequired;

        if (startDate.Date < DateTimeOffset.UtcNow.Date)
            return LeaveApplicationErrors.StartDateInPast;

        if (endDate.Date < startDate.Date)
            return LeaveApplicationErrors.InvalidDateRange;

        if (string.IsNullOrWhiteSpace(description))
            return LeaveApplicationErrors.DescriptionRequired;

        if (description.Trim().Length > LeaveApplicationConstants.DescriptionMaxLength)
            return LeaveApplicationErrors.DescriptionTooLong;

        if (
            !string.IsNullOrWhiteSpace(attachment)
            && attachment.Trim().Length > LeaveApplicationConstants.AttachmentMaxLength
        )
            return LeaveApplicationErrors.AttachmentTooLong;

        return null;
    }
}
