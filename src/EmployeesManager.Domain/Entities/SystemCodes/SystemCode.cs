using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.SystemCodes;

public sealed class SystemCode : AuditableEntity
{
    public string Name { get; private set; } = default!;
    public string Code { get; private set; } = default!;

    private SystemCode() { }

    private SystemCode(Guid id)
        : base(id) { }

    public static Result<SystemCode> Create(string name, string code)
    {
        var validationError = Validate(name, code);

        if (validationError is not null)
            return validationError;

        return new SystemCode(Guid.NewGuid()) { Name = name.Trim(), Code = code.Trim() };
    }

    public Result<Updated> Update(string name, string code)
    {
        var validationError = Validate(name, code);

        if (validationError is not null)
            return validationError;

        Name = name.Trim();
        Code = code.Trim();

        return Result.Updated;
    }

    private static Error? Validate(string name, string code)
    {
        if (string.IsNullOrWhiteSpace(name))
            return SystemCodeErrors.NameRequired;
        if (name.Trim().Length > SystemCodeConstants.NameMaxLength)
            return SystemCodeErrors.NameTooLong;

        if (string.IsNullOrWhiteSpace(code))
            return SystemCodeErrors.CodeRequired;
        if (code.Trim().Length > SystemCodeConstants.CodeMaxLength)
            return SystemCodeErrors.CodeTooLong;

        return null;
    }
}
