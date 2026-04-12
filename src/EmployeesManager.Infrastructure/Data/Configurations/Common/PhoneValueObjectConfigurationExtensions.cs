using System.Linq.Expressions;
using EmployeesManager.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesManager.Infrastructure.Data.Configurations.Common;

public static class PhoneValueObjectConfigurationExtensions
{
    public static EntityTypeBuilder<TEntity> OwnsPhone<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, Phone?>> propertyExpression,
        string columnName = "Phone"
    )
        where TEntity : class
    {
        builder.OwnsOne(
            propertyExpression,
            phone =>
            {
                phone
                    .Property(p => p.Value)
                    .HasColumnName(columnName)
                    .IsRequired()
                    .HasMaxLength(PhoneConstants.MaxLength);
            }
        );

        return builder;
    }
}
