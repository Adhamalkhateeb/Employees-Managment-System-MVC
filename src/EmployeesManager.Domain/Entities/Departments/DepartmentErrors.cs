using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.Departments;

public static class DepartmentErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Department.NotFound", $"Department '{id}' was not found.");

    public static readonly Error NameRequired = Error.Validation(
        "Department.Name.Required",
        "Name is required."
    );

    public static readonly Error NameTooLong = Error.Validation(
        "Department.Name.TooLong",
        "Name is too long."
    );

    public static readonly Error NameAlreadyExists = Error.Conflict(
        "Department.Name.AlreadyExists",
        "Department name already exists."
    );

    public static readonly Error CodeRequired = Error.Validation(
        "Department.Code.Required",
        "Code is required."
    );

    public static readonly Error CodeTooLong = Error.Validation(
        "Department.Code.TooLong",
        "Code is too long."
    );

    public static readonly Error CodeAlreadyExists = Error.Conflict(
        "Department.Code.AlreadyExists",
        "Department code already exists."
    );
}
