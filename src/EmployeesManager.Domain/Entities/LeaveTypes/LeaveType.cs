using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.LeaveTypes;

public sealed class LeaveType : AuditableEntity
{
    public string Name { get; private set; } = default!;

    private LeaveType() { }

    private LeaveType(Guid id)
        : base(id) { }

    public static Result<LeaveType> Create(string name)
    {
        var validationError = Validate(name);

        if (validationError is not null)
            return validationError;

        return new LeaveType(Guid.NewGuid()) { Name = name.Trim() };
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
            return LeaveTypeErrors.NameRequired;
        if (name.Trim().Length > LeaveTypeConstants.NameMaxLength)
            return LeaveTypeErrors.NameTooLong;

        return null;
    }
}
