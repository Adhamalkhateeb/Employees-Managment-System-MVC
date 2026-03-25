using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.LeaveTypes;

public static class LeaveTypeErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("LeaveType.NotFound", $"LeaveType '{id}' was not found.");

    public static readonly Error NameRequired = Error.Validation(
        "LeaveType.Name.Required",
        "Name is required."
    );

    public static readonly Error NameTooLong = Error.Validation(
        "LeaveType.Name.TooLong",
        "Name is too long."
    );

    public static readonly Error NameAlreadyExists = Error.Conflict(
        "LeaveType.Name.AlreadyExists",
        "Name already exists."
    );

    public static readonly Error CodeRequired = Error.Validation(
        "LeaveType.Code.Required",
        "Code is required."
    );

    public static readonly Error CodeTooLong = Error.Validation(
        "LeaveType.Code.TooLong",
        "Code is too long."
    );

    public static readonly Error CodeAlreadyExists = Error.Conflict(
        "LeaveType.Code.AlreadyExists",
        "Leave type code already exists."
    );
}
