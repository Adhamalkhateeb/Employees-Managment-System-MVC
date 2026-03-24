using EmployeesManager.Domain.Entities.SystemCodeDetails;
using EmployeesManager.Infrastructure.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesManager.Infrastructure.Data.Configurations;

public sealed class SystemCodeDetailConfiguration : IEntityTypeConfiguration<SystemCodeDetail>
{
    public void Configure(EntityTypeBuilder<SystemCodeDetail> builder)
    {
        builder.ToTable("SystemCodeDetails");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.ConfigureAuditableEntity();

        builder
            .Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(SystemCodeDetailConstants.CodeMaxLength);
        builder
            .Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(SystemCodeDetailConstants.DescriptionMaxLength);

        builder.HasIndex(x => new { x.SystemCodeId, x.Code }).IsUnique();

        builder
            .HasOne(x => x.SystemCode)
            .WithMany()
            .HasForeignKey(x => x.SystemCodeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
