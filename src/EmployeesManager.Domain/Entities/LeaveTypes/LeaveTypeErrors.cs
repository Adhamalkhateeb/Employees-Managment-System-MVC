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
}
