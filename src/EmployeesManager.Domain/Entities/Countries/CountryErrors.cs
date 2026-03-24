using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.Countries;

public static class CountryErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Country.NotFound", $"Country '{id}' was not found.");

    public static readonly Error CodeRequired = Error.Validation(
        "Country.Code.Required",
        "Code is required."
    );

    public static readonly Error CodeTooLong = Error.Validation(
        "Country.Code.TooLong",
        "Code is too long."
    );

    public static readonly Error CodeAlreadyExists = Error.Conflict(
        "Country.Code.AlreadyExists",
        "Code already exists."
    );

    public static readonly Error NameRequired = Error.Validation(
        "Country.Name.Required",
        "Name is required."
    );

    public static readonly Error NameTooLong = Error.Validation(
        "Country.Name.TooLong",
        "Name is too long."
    );

    public static readonly Error NameAlreadyExists = Error.Conflict(
        "Country.Name.AlreadyExists",
        "Name already exists."
    );
}
