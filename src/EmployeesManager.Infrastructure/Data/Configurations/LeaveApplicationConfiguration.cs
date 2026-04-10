using EmployeesManager.Domain.Entities.LeaveApplications;
using EmployeesManager.Infrastructure.Data.Configurations.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

        builder.Property(x => x.Duration).HasConversion<string>().IsRequired().HasMaxLength(50);
        builder.Property(x => x.Status).HasConversion<string>().IsRequired().HasMaxLength(50);

        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.EndDate).IsRequired();

        builder.ToTable(t =>
            t.HasCheckConstraint(
                "CK_LeaveApplications_EndDate_GreaterThanOrEqualStartDate",
                "CAST([EndDate] AS DATE) >= CAST([StartDate] AS DATE)"
            )
        );

        builder
            .Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(LeaveApplicationConstants.DescriptionMaxLength);
        builder
            .Property(x => x.Attachment)
            .HasMaxLength(LeaveApplicationConstants.AttachmentMaxLength);
        builder
            .Property(x => x.RejectionReason)
            .HasMaxLength(LeaveApplicationConstants.RejectionReasonMaxLength);

        builder.Property(x => x.DecisionById).IsRequired(false);
        builder.Property(x => x.DecisionAtUtc).IsRequired(false);

        builder.HasIndex(x => x.EmployeeId);
        builder.HasIndex(x => x.LeaveTypeId);
        builder.HasIndex(x => x.Duration);
        builder.HasIndex(x => x.Status);
        builder.HasIndex(x => x.DecisionById);

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
    }
}
