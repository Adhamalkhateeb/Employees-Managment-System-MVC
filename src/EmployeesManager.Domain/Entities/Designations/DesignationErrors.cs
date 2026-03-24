using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.Designations;

public static class DesignationErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Designation.NotFound", $"Designation '{id}' was not found.");

    public static readonly Error NameRequired = Error.Validation(
        "Designation.Name.Required",
        "Name is required."
    );

    public static readonly Error NameTooLong = Error.Validation(
        "Designation.Name.TooLong",
        "Name is too long."
    );

    public static readonly Error NameAlreadyExists = Error.Conflict(
        "Designation.Name.AlreadyExists",
        "Name already exists."
    );
}
