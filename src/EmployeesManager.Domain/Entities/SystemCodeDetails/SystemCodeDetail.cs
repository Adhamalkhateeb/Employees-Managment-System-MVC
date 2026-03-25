using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.SystemCodes;

namespace EmployeesManager.Domain.Entities.SystemCodeDetails;

public sealed class SystemCodeDetail : AuditableEntity
{
    public Guid SystemCodeId { get; private set; }
    public SystemCode SystemCode { get; private set; } = default!;
    public string Code { get; private set; } = default!;
    public string? Description { get; private set; } = default!;
    public int? OrderNo { get; private set; }

    private SystemCodeDetail() { }

    private SystemCodeDetail(
        Guid id,
        Guid systemCodeId,
        string code,
        string? description,
        int? orderNo
    )
        : base(id)
    {
        SystemCodeId = systemCodeId;
        Code = code;
        Description = description;
        OrderNo = orderNo;
    }

    public static Result<SystemCodeDetail> Create(
        Guid systemCodeId,
        string code,
        string? description,
        int? orderNo
    )
    {
        var validationError = Validate(systemCodeId, code, description, orderNo);

        if (validationError is not null)
            return validationError;

        return new SystemCodeDetail(Guid.NewGuid(), systemCodeId, code, description, orderNo);
    }

    public Result<Updated> Update(Guid systemCodeId, string code, string? description, int? orderNo)
    {
        var validationError = Validate(systemCodeId, code, description, orderNo);

        if (validationError is not null)
            return validationError;

        SystemCodeId = systemCodeId;
        Code = code.Trim();
        Description = description?.Trim();
        OrderNo = orderNo;

        return Result.Updated;
    }

    private static Error? Validate(
        Guid systemCodeId,
        string code,
        string? description,
        int? orderNo
    )
    {
        if (systemCodeId == Guid.Empty)
            return SystemCodeDetailErrors.SystemCodeRequired;

        if (string.IsNullOrWhiteSpace(code))
            return SystemCodeDetailErrors.CodeRequired;

        if (code.Trim().Length > SystemCodeDetailConstants.CodeMaxLength)
            return SystemCodeDetailErrors.CodeTooLong;

        if (
            description is not null
            && description.Trim().Length > SystemCodeDetailConstants.DescriptionMaxLength
        )
            return SystemCodeDetailErrors.DescriptionTooLong;

        if (orderNo is < 0)
            return SystemCodeDetailErrors.OrderNoInvalid;

        return null;
    }
}
