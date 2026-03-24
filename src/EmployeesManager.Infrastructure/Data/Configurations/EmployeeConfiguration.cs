using EmployeesManager.Domain.Entities.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeeConstantssManager.Infrastructure.Data.Configurations;

public sealed class EmployeeConstantsConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.ToTable("Employees");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.Property(x => x.CreatedAtUtc).IsRequired();
        builder.Property(x => x.CreatedBy).HasMaxLength(100).IsRequired();
        builder
            .Property(x => x.FirstName)
            .IsRequired()
            .HasMaxLength(EmployeeConstants.FirstNameMaxLength);

        builder.Property(x => x.MiddleName).HasMaxLength(EmployeeConstants.MiddleNameMaxLength);

        builder
            .Property(x => x.LastName)
            .IsRequired()
            .HasMaxLength(EmployeeConstants.LastNameMaxLength);

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

        builder
            .Property(x => x.Country)
            .IsRequired()
            .HasMaxLength(EmployeeConstants.CountryMaxLength);

        builder.Property(x => x.DateOfBirth).HasColumnType("date").IsRequired();

        builder.ToTable(t =>
            t.HasCheckConstraint(
                "CK_Employees_DateOfBirth",
                $"DateOfBirth <= DATEADD(year, -{EmployeeConstants.MinAge}, GETDATE())"
            )
        );

        builder.Property(x => x.Address).HasMaxLength(EmployeeConstants.AddressMaxLength);

        builder
            .Property(x => x.Department)
            .IsRequired()
            .HasMaxLength(EmployeeConstants.DepartmentMaxLength);

        builder
            .Property(x => x.Designation)
            .IsRequired()
            .HasMaxLength(EmployeeConstants.DesignationMaxLength);
    }
}
