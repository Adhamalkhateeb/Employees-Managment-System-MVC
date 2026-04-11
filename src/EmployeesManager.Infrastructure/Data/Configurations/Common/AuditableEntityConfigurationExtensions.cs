using EmployeesManager.Domain.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesManager.Infrastructure.Data.Configurations.Common;

internal static class AuditableEntityConfigurationExtensions
{
    public static void ConfigureAuditableEntity<TEntity>(this EntityTypeBuilder<TEntity> builder)
        where TEntity : AuditableEntity
    {
        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.CreatedBy).IsRequired();
        builder.Property(x => x.LastModifiedUtc).IsRequired(false);
        builder.Property(x => x.LastModifiedBy).IsRequired(false);
    }
}
