using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.Designations;

public static class DesignationErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Designation.NotFound", $"Designation '{id}' was not found.");

    public static readonly Error NameRequired = Error.Validation(
        "Designation.Name.Required",
        "Name is required.",
        "Name"
    );

    public static readonly Error NameTooLong = Error.Validation(
        "Designation.Name.TooLong",
        "Name is too long.",
        "Name"
    );

    public static readonly Error NameAlreadyExists = Error.Conflict(
        "Designation.Name.AlreadyExists",
        "Name already exists.",
        "Name"
    );

    public static readonly Error CodeRequired = Error.Validation(
        "Designation.Code.Required",
        "Code is required.",
        "Code"
    );

    public static readonly Error CodeTooLong = Error.Validation(
        "Designation.Code.TooLong",
        "Code is too long.",
        "Code"
    );

    public static readonly Error CodeAlreadyExists = Error.Conflict(
        "Designation.Code.AlreadyExists",
        "Designation code already exists.",
        "Code"
    );
}
