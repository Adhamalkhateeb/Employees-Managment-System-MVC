using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.Designations;

public sealed class Designation : AuditableEntity
{
    public string Name { get; private set; } = default!;
    public string Code { get; private set; } = default!;

    private Designation() { }

    private Designation(Guid id, string name, string code)
        : base(id)
    {
        Name = name;
        Code = code;
    }

    public static Result<Designation> Create(string name, string code)
    {
        var validationError = Validate(name, code);

        if (validationError is not null)
            return validationError;

        return new Designation(Guid.NewGuid(), name.Trim(), code.Trim());
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
            return DesignationErrors.NameRequired;
        if (name.Trim().Length > DesignationConstants.NameMaxLength)
            return DesignationErrors.NameTooLong;

        if (string.IsNullOrWhiteSpace(code))
            return DesignationErrors.CodeRequired;
        if (code.Trim().Length > DesignationConstants.CodeMaxLength)
            return DesignationErrors.CodeTooLong;

        return null;
    }
}
