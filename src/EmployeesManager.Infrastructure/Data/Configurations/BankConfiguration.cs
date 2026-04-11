using EmployeesManager.Domain.Entities.Banks;
using EmployeesManager.Infrastructure.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesManager.Infrastructure.Data.Configurations;

public sealed class BankConfiguration : IEntityTypeConfiguration<Bank>
{
    public void Configure(EntityTypeBuilder<Bank> builder)
    {
        builder.ToTable("Banks");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.Code).IsRequired().HasMaxLength(BankConstants.CodeMaxLength);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(BankConstants.NameMaxLength);
        builder
            .Property(x => x.AccountNo)
            .IsRequired()
            .HasMaxLength(BankConstants.AccountNoMaxLength);

        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => x.Name).IsUnique();
        builder.HasIndex(x => x.AccountNo).IsUnique();
    }
}
