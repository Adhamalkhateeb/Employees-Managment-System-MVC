using EmployeesManager.Domain.Entities.Cities;
using EmployeesManager.Infrastructure.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesManager.Infrastructure.Data.Configurations;

public sealed class CityConfiguration : IEntityTypeConfiguration<City>
{
    public void Configure(EntityTypeBuilder<City> builder)
    {
        builder.ToTable("Cities");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();
        builder.ConfigureAuditableEntity();

        builder.Property(x => x.Code).IsRequired().HasMaxLength(CityConstants.CodeMaxLength);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(CityConstants.NameMaxLength);

        builder.HasIndex(x => x.Code).IsUnique();
        builder.HasIndex(x => new { x.CountryId, x.Name }).IsUnique();

        builder
            .HasOne(x => x.Country)
            .WithMany()
            .HasForeignKey(x => x.CountryId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
