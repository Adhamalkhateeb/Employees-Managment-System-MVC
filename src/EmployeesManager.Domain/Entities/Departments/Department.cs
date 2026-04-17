using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.Departments;

public sealed class Department : AuditableEntity
{
    public string Name { get; private set; } = default!;
    public string Code { get; private set; } = default!;

    private Department() { }

    private Department(Guid id, string name, string code)
        : base(id)
    {
        Name = name;
        Code = code;
    }

    public static Result<Department> Create(string name, string code)
    {
        var validationError = Validate(name, code);
        if (validationError != null)
            return validationError;

        return new Department(Guid.NewGuid(), name.Trim(), code.Trim());
    }

    public Result<Updated> Update(string name, string code)
    {
        var validationError = Validate(name, code);
        if (validationError != null)
            return validationError;

        Name = name.Trim();
        Code = code.Trim();

        return Result.Updated;
    }

    private static Error? Validate(string name, string code)
    {
        if (string.IsNullOrWhiteSpace(name))
            return DepartmentErrors.NameRequired;
        if (name.Trim().Length > DepartmentConstants.NameMaxLength)
            return DepartmentErrors.NameTooLong;

        if (string.IsNullOrWhiteSpace(code))
            return DepartmentErrors.CodeRequired;
        if (code.Trim().Length > DepartmentConstants.CodeMaxLength)
            return DepartmentErrors.CodeTooLong;

        return null;
    }
}
