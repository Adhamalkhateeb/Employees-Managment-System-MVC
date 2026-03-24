using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.SystemCodes;

public static class SystemCodeErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("SystemCode.NotFound", $"SystemCode '{id}' was not found.");

    public static readonly Error NameRequired = Error.Validation(
        "SystemCode.Name.Required",
        "Name is required."
    );

    public static readonly Error NameTooLong = Error.Validation(
        "SystemCode.Name.TooLong",
        "Name is too long."
    );

    public static readonly Error NameAlreadyExists = Error.Conflict(
        "SystemCode.Name.AlreadyExists",
        "Name already exists."
    );

    public static readonly Error CodeRequired = Error.Validation(
        "SystemCode.Code.Required",
        "Code is required."
    );

    public static readonly Error CodeTooLong = Error.Validation(
        "SystemCode.Code.TooLong",
        "Code is too long."
    );

    public static readonly Error CodeAlreadyExists = Error.Conflict(
        "SystemCode.Code.AlreadyExists",
        "Code already exists."
    );
}
