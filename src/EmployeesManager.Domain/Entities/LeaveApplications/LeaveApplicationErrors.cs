using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.LeaveApplications;

public static class LeaveApplicationErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("LeaveApplication.NotFound", $"Leave application '{id}' was not found.");

    public static readonly Error EmployeeRequired = Error.Validation(
        "LeaveApplication.Employee.Required",
        "Employee is required.",
        "EmployeeId"
    );
    public static readonly Error LeaveTypeRequired = Error.Validation(
        "LeaveApplication.LeaveType.Required",
        "Leave type is required.",
        "LeaveTypeId"
    );
    public static readonly Error StartDateRequired = Error.Validation(
        "LeaveApplication.StartDate.Required",
        "Start date is required.",
        "StartDate"
    );
    public static readonly Error EndDateRequired = Error.Validation(
        "LeaveApplication.EndDate.Required",
        "End date is required.",
        "EndDate"
    );
    public static readonly Error StartDateInPast = Error.Validation(
        "LeaveApplication.StartDate.InPast",
        "Start date cannot be in the past.",
        "StartDate"
    );
    public static readonly Error InvalidDateRange = Error.Validation(
        "LeaveApplication.DateRange.Invalid",
        "End date must be on or after start date.",
        "EndDate"
    );
    public static readonly Error DescriptionRequired = Error.Validation(
        "LeaveApplication.Description.Required",
        "Description is required.",
        "Description"
    );
    public static readonly Error DescriptionTooLong = Error.Validation(
        "LeaveApplication.Description.TooLong",
        $"Description cannot exceed {LeaveApplicationConstants.DescriptionMaxLength} characters.",
        "Description"
    );
    public static readonly Error AttachmentTooLong = Error.Validation(
        "LeaveApplication.Attachment.TooLong",
        $"Attachment path cannot exceed {LeaveApplicationConstants.AttachmentMaxLength} characters.",
        "Attachment"
    );
    public static readonly Error RejectionReasonRequired = Error.Validation(
        "LeaveApplication.RejectionReason.Required",
        "A reason is required when rejecting a leave application.",
        "RejectionReason"
    );
    public static readonly Error OverlappingLeave = Error.Conflict(
        "LeaveApplication.OverlappingLeave",
        "Employee already has a leave application overlapping the requested date range.",
        "StartDate"
    );
    public static readonly Error NotEditable = Error.Conflict(
        "LeaveApplication.NotEditable",
        "This leave application can no longer be edited because it is not pending.",
        "Status"
    );
    public static readonly Error NotApprovable = Error.Conflict(
        "LeaveApplication.NotApprovable",
        "Only pending leave applications can be approved.",
        "Status"
    );
    public static readonly Error NotRejectable = Error.Conflict(
        "LeaveApplication.NotRejectable",
        "Only pending leave applications can be rejected.",
        "Status"
    );
    public static readonly Error NotCancellable = Error.Conflict(
        "LeaveApplication.NotCancellable",
        "Only pending leave applications can be cancelled.",
        "Status"
    );

    public static Error DecisionerRequired(string decisioner) =>
        Error.Validation(
            $"LeaveApplication.{decisioner}.Required",
            $"{decisioner} is required when making a decision on a leave application.",
            decisioner
        );
}
