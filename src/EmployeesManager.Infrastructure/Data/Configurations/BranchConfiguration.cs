using EmployeesManager.Domain.Entities.Branches;
using EmployeesManager.Infrastructure.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesManager.Infrastructure.Data.Configurations;

public sealed class BranchConfiguration : IEntityTypeConfiguration<Branch>
{
    public void Configure(EntityTypeBuilder<Branch> builder)
    {
        builder.ToTable("Branches");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.Name).HasMaxLength(BranchConstants.NameMaxLength).IsRequired();

        builder.HasIndex(x => x.Name).IsUnique();

        builder
            .Property(x => x.Address)
            .HasMaxLength(BranchConstants.AddressMaxLength)
            .IsRequired();

        builder.OwnsEmail(x => x.Email);

        builder.OwnsPhone(x => x.Phone);

        builder.Property(x => x.ManagerId).IsRequired(false);

        builder
            .HasMany(x => x.Employees)
            .WithOne(x => x.Branch)
            .HasForeignKey(x => x.BranchId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
