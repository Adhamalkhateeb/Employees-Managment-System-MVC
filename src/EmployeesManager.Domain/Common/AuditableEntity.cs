namespace EmployeesManager.Domain.Common;

public abstract class AuditableEntity : BaseEntity
{
    protected AuditableEntity() { }

    protected AuditableEntity(Guid id)
        : base(id) { }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public string? CreatedBy { get; set; }

    public DateTimeOffset LastModifiedUtc { get; set; }
    public string? LastModifiedBy { get; set; }
}
