using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.SystemCodes;

public sealed class SystemCode : AuditableEntity
{
    public string Code { get; private set; } = default!;
    public string? Description { get; set; } = string.Empty;

    private SystemCode() { }

    private SystemCode(Guid id, string code, string? description)
        : base(id)
    {
        Code = code;
        Description = description;
    }

    public static Result<SystemCode> Create(string code, string? description)
    {
        var validationError = Validate(code, description);

        if (validationError is not null)
            return validationError;

        return new SystemCode(Guid.NewGuid(), code, description);
    }

    public Result<Updated> Update(string code, string? description)
    {
        var validationError = Validate(code, description);

        if (validationError is not null)
            return validationError;

        Code = code.Trim();
        Description = description?.Trim();

        return Result.Updated;
    }

    private static Error? Validate(string code, string? description)
    {
        if (string.IsNullOrWhiteSpace(code))
            return SystemCodeErrors.CodeRequired;

        if (code.Trim().Length > SystemCodeConstants.CodeMaxLength)
            return SystemCodeErrors.CodeTooLong;

        if (
            !string.IsNullOrEmpty(description)
            && description.Trim().Length > SystemCodeConstants.DescriptionMaxLength
        )
            return SystemCodeErrors.DescriptionTooLong;

        return null;
    }
}
