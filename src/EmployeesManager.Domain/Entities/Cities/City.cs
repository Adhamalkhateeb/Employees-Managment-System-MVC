using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Countries;

namespace EmployeesManager.Domain.Entities.Cities;

public sealed class City : AuditableEntity
{
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;

    public Guid CountryId { get; private set; }
    public Country Country { get; private set; } = default!;

    private City() { }

    private City(Guid id)
        : base(id) { }

    public static Result<City> Create(string code, string name, Guid countryId)
    {
        var validationError = Validate(code, name, countryId);

        if (validationError is not null)
            return validationError;

        return new City(Guid.NewGuid())
        {
            Code = code.Trim(),
            Name = name.Trim(),
            CountryId = countryId,
        };
    }

    public Result<Updated> Update(string code, string name, Guid countryId)
    {
        var validationError = Validate(code, name, countryId);

        if (validationError is not null)
            return validationError;

        Code = code.Trim();
        Name = name.Trim();
        CountryId = countryId;

        return Result.Updated;
    }

    private static Error? Validate(string code, string name, Guid countryId)
    {
        if (string.IsNullOrWhiteSpace(code))
            return CityErrors.CodeRequired;
        if (code.Trim().Length > CityConstants.CodeMaxLength)
            return CityErrors.CodeTooLong;

        if (string.IsNullOrWhiteSpace(name))
            return CityErrors.NameRequired;
        if (name.Trim().Length > CityConstants.NameMaxLength)
            return CityErrors.NameTooLong;

        if (countryId == Guid.Empty)
            return CityErrors.CountryRequired;

        return null;
    }
}
