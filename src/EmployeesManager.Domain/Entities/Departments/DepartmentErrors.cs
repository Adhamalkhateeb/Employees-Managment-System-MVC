using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.Departments;

public static class DepartmentErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Department.NotFound", $"Department '{id}' was not found.");

    public static readonly Error NameRequired = Error.Validation(
        "Department.Name.Required",
        "Name is required.",
        "Name"
    );

    public static readonly Error NameTooLong = Error.Validation(
        "Department.Name.TooLong",
        "Name is too long.",
        "Name"
    );

    public static readonly Error NameAlreadyExists = Error.Conflict(
        "Department.Name.AlreadyExists",
        "Department name already exists.",
        "Name"
    );

    public static readonly Error ManagerAlreadyAssigned = Error.Conflict(
        "Department.Manager.AlreadyAssigned",
        "The specified manager is already assigned to another department.",
        "ManagerId"
    );

    public static readonly Error ManagerNotFound = Error.NotFound(
        "Department.Manager.NotFound",
        "The specified manager was not found.",
        "ManagerId"
    );
}
