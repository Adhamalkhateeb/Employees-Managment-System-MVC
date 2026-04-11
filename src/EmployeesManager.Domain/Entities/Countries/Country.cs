using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.Countries;

public sealed class Country : AuditableEntity
{
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;

    private Country() { }

    private Country(Guid id)
        : base(id) { }

    public static Result<Country> Create(string code, string name)
    {
        var validationError = Validate(code, name);

        if (validationError is not null)
            return validationError;

        return new Country(Guid.NewGuid()) { Code = code.Trim(), Name = name.Trim() };
    }

    public Result<Updated> Update(string code, string name)
    {
        var validationError = Validate(code, name);

        if (validationError is not null)
            return validationError;

        Code = code.Trim();
        Name = name.Trim();

        return Result.Updated;
    }

    private static Error? Validate(string code, string name)
    {
        if (string.IsNullOrWhiteSpace(code))
            return CountryErrors.CodeRequired;
        if (code.Trim().Length > CountryConstants.CodeMaxLength)
            return CountryErrors.CodeTooLong;

        if (string.IsNullOrWhiteSpace(name))
            return CountryErrors.NameRequired;
        if (name.Trim().Length > CountryConstants.NameMaxLength)
            return CountryErrors.NameTooLong;

        return null;
    }
}
