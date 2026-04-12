using System.Linq.Expressions;
using EmployeesManager.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesManager.Infrastructure.Data.Configurations.Common;

public static class EmailValueObjectConfigurationExtensions
{
    public static EntityTypeBuilder<TEntity> OwnsEmail<TEntity>(
        this EntityTypeBuilder<TEntity> builder,
        Expression<Func<TEntity, Email?>> propertyExpression,
        string columnName = "Email"
    )
        where TEntity : class
    {
        builder.OwnsOne(
            propertyExpression,
            email =>
            {
                email
                    .Property(e => e.Value)
                    .HasColumnName(columnName)
                    .IsRequired()
                    .HasMaxLength(EmailConstants.MaxLength);

                email.HasIndex(e => e.Value).IsUnique();
            }
        );

        return builder;
    }
}
