using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.Designations;

public sealed class Designation : AuditableEntity
{
    public string Name { get; private set; } = default!;

    private Designation() { }

    private Designation(Guid id)
        : base(id) { }

    public static Result<Designation> Create(string name)
    {
        var validationError = Validate(name);

        if (validationError is not null)
            return validationError;

        return new Designation(Guid.NewGuid()) { Name = name.Trim() };
    }

    public Result<Updated> Update(string name)
    {
        var validationError = Validate(name);

        if (validationError is not null)
            return validationError;

        Name = name.Trim();

        return Result.Updated;
    }

    private static Error? Validate(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return DesignationErrors.NameRequired;
        if (name.Trim().Length > DesignationConstants.NameMaxLength)
            return DesignationErrors.NameTooLong;

        return null;
    }
}
