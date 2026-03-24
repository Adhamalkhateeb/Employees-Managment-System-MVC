using EmployeesManager.Domain.Entities.SystemCodes;
using EmployeesManager.Infrastructure.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesManager.Infrastructure.Data.Configurations;

public sealed class SystemCodeConfiguration : IEntityTypeConfiguration<SystemCode>
{
    public void Configure(EntityTypeBuilder<SystemCode> builder)
    {
        builder.ToTable("SystemCodes");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(SystemCodeConstants.NameMaxLength);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(SystemCodeConstants.CodeMaxLength);

        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasIndex(x => x.Code).IsUnique();
    }
}
