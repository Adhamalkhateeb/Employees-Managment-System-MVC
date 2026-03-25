using EmployeesManager.Domain.Entities.LeaveApplications;
using EmployeesManager.Infrastructure.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmployeesManager.Infrastructure.Data.Configurations;

public sealed class LeaveApplicationConfiguration : IEntityTypeConfiguration<LeaveApplication>
{
    public void Configure(EntityTypeBuilder<LeaveApplication> builder)
    {
        builder.ToTable("LeaveApplications");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedNever();

        builder.ConfigureAuditableEntity();

        builder.Property(x => x.EmployeeId).IsRequired();
        builder.Property(x => x.LeaveTypeId).IsRequired();
        builder.Property(x => x.DurationId).IsRequired();
        builder.Property(x => x.StatusId).IsRequired();
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired();

        builder.Ignore(x => x.Days);
        builder.ToTable(x =>
        {
            x.HasCheckConstraint(
                "CK_LeaveApplications_EndDate_GreaterThanOrEqualStartDate",
                "[EndDate] >= [StartDate]"
            );
            x.HasCheckConstraint(
                "CK_LeaveApplications_StartDate_NotInPast",
                "[StartDate] >= CAST(GETUTCDATE() AS DATE)"
            );
        });

        builder
            .Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(LeaveApplicationConstants.DescriptionMaxLength);

        builder
            .Property(x => x.Attachment)
            .HasMaxLength(LeaveApplicationConstants.AttachmentMaxLength);

        builder.Property(x => x.ApprovedBy).HasMaxLength(250);
        builder.Property(x => x.ApprovedAtUtc);

        builder.HasIndex(x => x.EmployeeId);
        builder.HasIndex(x => x.LeaveTypeId);
        builder.HasIndex(x => x.DurationId);
        builder.HasIndex(x => x.StatusId);

        builder
            .HasOne(x => x.Employee)
            .WithMany()
            .HasForeignKey(x => x.EmployeeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.LeaveType)
            .WithMany()
            .HasForeignKey(x => x.LeaveTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Duration)
            .WithMany()
            .HasForeignKey(x => x.DurationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Status)
            .WithMany()
            .HasForeignKey(x => x.StatusId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
