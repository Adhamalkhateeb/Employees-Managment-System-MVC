using EmployeesManager.Domain.Entities.Designations;
using EmployeesManager.Infrastructure.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesManager.Infrastructure.Data.Configurations;

public sealed class DesignationConfiguration : IEntityTypeConfiguration<Designation>
{
    public void Configure(EntityTypeBuilder<Designation> builder)
    {
        builder.ToTable("Designations");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(DesignationConstants.NameMaxLength);

        builder.Property(x => x.Code).IsRequired().HasMaxLength(DesignationConstants.CodeMaxLength);

        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasIndex(x => x.Code).IsUnique();
    }
}
