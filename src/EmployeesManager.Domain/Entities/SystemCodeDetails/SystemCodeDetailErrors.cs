using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.SystemCodeDetails;

public static class SystemCodeDetailErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("SystemCodeDetail.NotFound", $"SystemCodeDetail '{id}' was not found.");

    public static readonly Error SystemCodeRequired = Error.Validation(
        "SystemCodeDetail.SystemCode.Required",
        "System code is required."
    );

    public static readonly Error CodeRequired = Error.Validation(
        "SystemCodeDetail.Code.Required",
        "Code is required."
    );

    public static readonly Error CodeTooLong = Error.Validation(
        "SystemCodeDetail.Code.TooLong",
        "Code is too long."
    );

    public static readonly Error DescriptionTooLong = Error.Validation(
        "SystemCodeDetail.Description.TooLong",
        "Description is too long."
    );

    public static readonly Error CodeAlreadyExists = Error.Conflict(
        "SystemCodeDetail.Code.AlreadyExists",
        "Code already exists for this system code."
    );

    public static readonly Error OrderNoInvalid = Error.Validation(
        "SystemCodeDetail.OrderNo.Invalid",
        "Order number is invalid."
    );
}
