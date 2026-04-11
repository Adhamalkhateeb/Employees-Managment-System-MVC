using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Employees;
using EmployeesManager.Domain.Entities.LeaveApplications;
using EmployeesManager.Domain.Entities.LeaveApplications.Enums;
using EmployeesManager.Domain.Entities.LeaveTypes;

public sealed class LeaveApplication : DecisionActivity
{
    public Guid EmployeeId { get; private set; }
    public Guid LeaveTypeId { get; private set; }
    public LeaveApplicationStatus Status { get; private set; }
    public LeaveApplicationDurations Duration { get; private set; } = default!;
    public DateTimeOffset StartDate { get; private set; }
    public DateTimeOffset EndDate { get; private set; }
    public string Description { get; private set; } = default!;
    public string? Attachment { get; private set; }
    public string? RejectionReason { get; private set; }

    public Employee Employee { get; private set; } = default!;
    public LeaveType LeaveType { get; private set; } = default!;

    private LeaveApplication()
    {
        // Required by EF Core
    }

    private LeaveApplication(
        Guid id,
        Guid employeeId,
        Guid leaveTypeId,
        LeaveApplicationDurations duration,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string description,
        string? attachment
    )
        : base(id)
    {
        EmployeeId = employeeId;
        LeaveTypeId = leaveTypeId;
        Duration = duration;
        Status = LeaveApplicationStatus.Pending;
        StartDate = startDate;
        EndDate = endDate;
        Description = description.Trim();
        Attachment = string.IsNullOrWhiteSpace(attachment) ? null : attachment.Trim();
    }

    public static Result<LeaveApplication> Create(
        Guid employeeId,
        Guid leaveTypeId,
        LeaveApplicationDurations duration,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string description,
        string? attachment
    )
    {
        var error = ValidateForCreate(
            employeeId,
            leaveTypeId,
            startDate,
            endDate,
            description,
            attachment
        );

        if (error is not null)
            return error;

        return new LeaveApplication(
            Guid.NewGuid(),
            employeeId,
            leaveTypeId,
            duration,
            startDate,
            endDate,
            description,
            attachment
        );
    }

    public Result<Updated> Update(
        Guid employeeId,
        Guid leaveTypeId,
        LeaveApplicationDurations duration,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string description,
        string? attachment
    )
    {
        if (!IsPending())
            return LeaveApplicationErrors.NotEditable;

        var error = ValidateForUpdate(
            employeeId,
            leaveTypeId,
            startDate,
            endDate,
            description,
            attachment
        );

        if (error is not null)
            return error;

        EmployeeId = employeeId;
        LeaveTypeId = leaveTypeId;
        Duration = duration;
        StartDate = startDate;
        EndDate = endDate;
        Description = description.Trim();
        Attachment = string.IsNullOrWhiteSpace(attachment) ? null : attachment.Trim();

        return Result.Updated;
    }

    public Result<Success> Approve(Guid approvedBy)
    {
        if (!IsPending())
            return LeaveApplicationErrors.NotApprovable;

        if (approvedBy == Guid.Empty)
            return LeaveApplicationErrors.DecisionerRequired("Approver");

        Status = LeaveApplicationStatus.Approved;
        SetDecision(approvedBy);
        return Result.Success;
    }

    public Result<Success> Reject(Guid rejectedBy, string reason)
    {
        if (!IsPending())
            return LeaveApplicationErrors.NotRejectable;

        if (string.IsNullOrWhiteSpace(reason))
            return LeaveApplicationErrors.RejectionReasonRequired;

        if (rejectedBy == Guid.Empty)
            return LeaveApplicationErrors.DecisionerRequired("Rejecter");

        Status = LeaveApplicationStatus.Rejected;
        RejectionReason = reason.Trim();
        SetDecision(rejectedBy);
        return Result.Success;
    }

    public Result<Success> Cancel(Guid cancelledBy)
    {
        if (!IsPending())
            return LeaveApplicationErrors.NotCancellable;

        if (cancelledBy == Guid.Empty)
            return LeaveApplicationErrors.DecisionerRequired("Canceler");

        Status = LeaveApplicationStatus.Cancelled;
        SetDecision(cancelledBy);
        return Result.Success;
    }

    public bool IsPending() => Status == LeaveApplicationStatus.Pending;

    private static Error? ValidateForCreate(
        Guid employeeId,
        Guid leaveTypeId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string description,
        string? attachment
    )
    {
        var error = ValidateShared(
            employeeId,
            leaveTypeId,
            startDate,
            endDate,
            description,
            attachment
        );

        if (error is not null)
            return error;

        if (startDate.Date < DateTimeOffset.UtcNow.Date)
            return LeaveApplicationErrors.StartDateInPast;

        return null;
    }

    private static Error? ValidateForUpdate(
        Guid employeeId,
        Guid leaveTypeId,
        DateTimeOffset startDate,
        DateTimeOffset endDate,
        string description,
        string? attachment
    ) => ValidateShared(employeeId, leaveTypeId, startDate, endDate, description, attachment);

    private static Error? ValidateShared(
        Guid employeeId,
        Guid leaveTypeId,
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

        if (startDate == DateTimeOffset.MinValue)
            return LeaveApplicationErrors.StartDateRequired;
        if (endDate == DateTimeOffset.MinValue)
            return LeaveApplicationErrors.EndDateRequired;
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
