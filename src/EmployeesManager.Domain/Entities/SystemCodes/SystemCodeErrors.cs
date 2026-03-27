using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.SystemCodes;

public static class SystemCodeErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("SystemCode.NotFound", $"SystemCode '{id}' was not found.");

    public static readonly Error CodeRequired = Error.Validation(
        "SystemCode.Code.Required",
        "Code is required.",
        "Code"
    );

    public static readonly Error CodeTooLong = Error.Validation(
        "SystemCode.Code.TooLong",
        "Code is too long.",
        "Code"
    );

    public static readonly Error CodeAlreadyExists = Error.Conflict(
        "SystemCode.Code.AlreadyExists",
        "Code already exists.",
        "Code"
    );

    public static readonly Error DescriptionTooLong = Error.Validation(
        "SystemCode.Description.TooLong",
        "Description is too long.",
        "Description"
    );
}
