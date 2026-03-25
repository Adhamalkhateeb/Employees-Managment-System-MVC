using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.LeaveApplications;

public static class LeaveApplicationErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("LeaveApplication.NotFound", $"Leave application '{id}' was not found.");

    public static readonly Error EmployeeRequired = Error.Validation(
        "LeaveApplication.Employee.Required",
        "Employee is required."
    );

    public static readonly Error LeaveTypeRequired = Error.Validation(
        "LeaveApplication.LeaveType.Required",
        "Leave type is required."
    );

    public static readonly Error DurationRequired = Error.Validation(
        "LeaveApplication.Duration.Required",
        "Duration is required."
    );

    public static readonly Error StatusRequired = Error.Validation(
        "LeaveApplication.Status.Required",
        "Status is required."
    );

    public static readonly Error StartDateRequired = Error.Validation(
        "LeaveApplication.StartDate.Required",
        "Start date is required."
    );

    public static readonly Error EndDateRequired = Error.Validation(
        "LeaveApplication.EndDate.Required",
        "End date is required."
    );

    public static readonly Error StartDateInPast = Error.Validation(
        "LeaveApplication.StartDate.InPast",
        "Start date cannot be in the past."
    );

    public static readonly Error InvalidDateRange = Error.Validation(
        "LeaveApplication.DateRange.Invalid",
        "End date must be greater than or equal to start date."
    );

    public static readonly Error DescriptionRequired = Error.Validation(
        "LeaveApplication.Description.Required",
        "Description is required."
    );

    public static readonly Error DescriptionTooLong = Error.Validation(
        "LeaveApplication.Description.TooLong",
        "Description is too long."
    );

    public static readonly Error AttachmentTooLong = Error.Validation(
        "LeaveApplication.Attachment.TooLong",
        "Attachment is too long."
    );
}
