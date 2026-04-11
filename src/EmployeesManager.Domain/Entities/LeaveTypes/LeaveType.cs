using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.LeaveTypes;

public sealed class LeaveType : AuditableEntity
{
    public string Name { get; private set; } = default!;
    public string Code { get; private set; } = default!;

    private LeaveType() { }

    private LeaveType(Guid id, string name, string code)
        : base(id)
    {
        Name = name;
        Code = code;
    }

    public static Result<LeaveType> Create(string name, string code)
    {
        var validationError = Validate(name, code);

        if (validationError is not null)
            return validationError;

        return new LeaveType(Guid.NewGuid(), name.Trim(), code.Trim());
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
            return LeaveTypeErrors.NameRequired;
        if (name.Trim().Length > LeaveTypeConstants.NameMaxLength)
            return LeaveTypeErrors.NameTooLong;

        if (string.IsNullOrWhiteSpace(code))
            return LeaveTypeErrors.CodeRequired;
        if (code.Trim().Length > LeaveTypeConstants.CodeMaxLength)
            return LeaveTypeErrors.CodeTooLong;

        return null;
    }
}
