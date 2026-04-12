using EmployeesManager.Domain.Entities.Employees;
using EmployeesManager.Infrastructure.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesManager.Infrastructure.Data.Configurations;

public sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.ConfigureAuditableEntity();
        builder
            .Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(EmployeeConstants.FirstNameMaxLength);

        builder
            .Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(EmployeeConstants.LastNameMaxLength);

        builder
            .Property(x => x.NationalId)
            .IsRequired()
            .HasMaxLength(EmployeeConstants.NationalIdMaxLength);

        builder.HasIndex(x => x.NationalId).IsUnique();

        builder
            .Property(x => x.PhoneNumber)
            .IsRequired()
            .HasMaxLength(EmployeeConstants.PhoneNumberMaxLength);

        builder.HasIndex(x => x.PhoneNumber).IsUnique();

        builder
            .Property(x => x.EmailAddress)
            .IsRequired()
            .HasMaxLength(EmployeeConstants.EmailAddressMaxLength);

        builder.HasIndex(x => x.EmailAddress).IsUnique();

        builder.Property(x => x.HireDate).HasColumnType("date").IsRequired();

        builder.Property(x => x.Address).HasMaxLength(EmployeeConstants.AddressMaxLength);

        builder.HasIndex(x => x.DepartmentId);
        builder.HasIndex(x => x.BranchId);

        builder
            .HasOne(x => x.Department)
            .WithMany(x => x.Employees)
            .HasForeignKey(x => x.DepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Branch)
            .WithMany(x => x.Employees)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
