using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.Cities;

public static class CityErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("City.NotFound", $"City '{id}' was not found.");

    public static readonly Error CodeRequired = Error.Validation(
        "City.Code.Required",
        "Code is required.",
        "Code"
    );

    public static readonly Error CodeTooLong = Error.Validation(
        "City.Code.TooLong",
        "Code is too long.",
        "Code"
    );

    public static readonly Error CodeAlreadyExists = Error.Conflict(
        "City.Code.AlreadyExists",
        "Code already exists.",
        "Code"
    );

    public static readonly Error NameRequired = Error.Validation(
        "City.Name.Required",
        "Name is required.",
        "Name"
    );

    public static readonly Error NameTooLong = Error.Validation(
        "City.Name.TooLong",
        "Name is too long.",
        "Name"
    );

    public static readonly Error NameAlreadyExists = Error.Conflict(
        "City.Name.AlreadyExists",
        "Name already exists.",
        "Name"
    );

    public static readonly Error CountryRequired = Error.Validation(
        "City.Country.Required",
        "Country is required.",
        "CountryId"
    );
}
