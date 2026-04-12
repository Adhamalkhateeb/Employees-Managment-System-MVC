using EmployeesManager.Domain.Entities.Departments;
using EmployeesManager.Infrastructure.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesManager.Infrastructure.Data.Configurations;

public sealed class DepartmentConfiguration : IEntityTypeConfiguration<Department>
{
    public void Configure(EntityTypeBuilder<Department> builder)
    {
        builder.ToTable("Departments");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.Name).IsRequired().HasMaxLength(DepartmentConstants.NameMaxLength);

        builder.HasIndex(x => x.Name).IsUnique();

        builder
            .HasOne(x => x.Manager)
            .WithOne()
            .HasForeignKey<Department>(x => x.ManagerId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
