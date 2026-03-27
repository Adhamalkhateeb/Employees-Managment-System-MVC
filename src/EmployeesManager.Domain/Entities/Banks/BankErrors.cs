using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.Banks;

public static class BankErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Bank.NotFound", $"Bank '{id}' was not found.");

    public static readonly Error CodeRequired = Error.Validation(
        "Bank.Code.Required",
        "Code is required.",
        "Code"
    );

    public static readonly Error CodeTooLong = Error.Validation(
        "Bank.Code.TooLong",
        "Code is too long.",
        "Code"
    );

    public static readonly Error CodeAlreadyExists = Error.Conflict(
        "Bank.Code.AlreadyExists",
        "Code already exists.",
        "Code"
    );

    public static readonly Error NameRequired = Error.Validation(
        "Bank.Name.Required",
        "Name is required.",
        "Name"
    );

    public static readonly Error NameTooLong = Error.Validation(
        "Bank.Name.TooLong",
        "Name is too long.",
        "Name"
    );

    public static readonly Error NameAlreadyExists = Error.Conflict(
        "Bank.Name.AlreadyExists",
        "Name already exists.",
        "Name"
    );

    public static readonly Error AccountNoRequired = Error.Validation(
        "Bank.AccountNo.Required",
        "Account number is required.",
        "AccountNo"
    );

    public static readonly Error AccountNoTooLong = Error.Validation(
        "Bank.AccountNo.TooLong",
        "Account number is too long.",
        "AccountNo"
    );

    public static readonly Error AccountNoAlreadyExists = Error.Conflict(
        "Bank.AccountNo.AlreadyExists",
        "Account number already exists.",
        "AccountNo"
    );
}
