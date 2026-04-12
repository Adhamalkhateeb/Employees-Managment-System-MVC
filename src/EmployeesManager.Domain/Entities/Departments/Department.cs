using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Employees;

namespace EmployeesManager.Domain.Entities.Departments;

public sealed class Department : AuditableEntity
{
    public string Name { get; private set; } = default!;
    public Guid? ManagerId { get; private set; }
    public Employee? Manager { get; private set; }
    private readonly List<Employee> _employees = [];
    public IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

    private Department() { }

    private Department(Guid id, string name, Guid? managerId)
        : base(id)
    {
        Name = name;
        ManagerId = managerId;
    }

    public static Result<Department> Create(string name, Guid? managerId)
    {
        name = name?.Trim() ?? string.Empty;
        var validationError = Validate(name);
        if (validationError is not null)
            return validationError;

        return new Department(Guid.NewGuid(), name.Trim(), managerId);
    }

    public Result<Updated> Update(string name, Guid? managerId)
    {
        var validationError = Validate(name);
        if (validationError is not null)
            return validationError;

        Name = name.Trim();
        ManagerId = managerId;

        return Result.Updated;
    }

    public Result<Success> AssignManager(Guid managerId)
    {
        if (ManagerId == managerId)
            return Result.Success;

        ManagerId = managerId;
        return Result.Success;
    }

    public Result<Success> RemoveManager()
    {
        ManagerId = null;
        return Result.Success;
    }

    private static Error? Validate(string name)
    {
        name = name?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(name))
            return DepartmentErrors.NameRequired;
        if (name.Length > DepartmentConstants.NameMaxLength)
            return DepartmentErrors.NameTooLong;

        return null;
    }
}
