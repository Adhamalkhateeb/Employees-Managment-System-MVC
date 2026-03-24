namespace EmployeesManager.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; }

    protected BaseEntity() { }

    protected BaseEntity(Guid id)
    {
        Id = id == Guid.Empty ? Guid.NewGuid() : id;
    }
}
