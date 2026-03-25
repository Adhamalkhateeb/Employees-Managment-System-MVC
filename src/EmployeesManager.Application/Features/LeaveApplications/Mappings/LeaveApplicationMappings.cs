using EmployeesManager.Application.Features.LeaveApplications.Dtos;
using EmployeesManager.Domain.Entities.LeaveApplications;

namespace EmployeesManager.Application.Features.LeaveApplications.Mappings;

public static class LeaveApplicationMappings
{
    public static LeaveApplicationDto ToDto(this LeaveApplication entity) =>
        new(
            Id: entity.Id,
            EmployeeId: entity.EmployeeId,
            EmployeeName: (
                (entity.Employee?.FirstName ?? string.Empty)
                + " "
                + (entity.Employee?.LastName ?? string.Empty)
            ).Trim(),
            LeaveTypeId: entity.LeaveTypeId,
            LeaveTypeName: entity.LeaveType?.Name ?? string.Empty,
            DurationId: entity.DurationId,
            DurationName: entity.Duration?.Description ?? entity.Duration?.Code ?? string.Empty,
            StatusId: entity.StatusId,
            StatusName: entity.Status?.Description ?? entity.Status?.Code ?? string.Empty,
            StartDate: entity.StartDate,
            EndDate: entity.EndDate,
            Days: entity.Days,
            Description: entity.Description,
            Attachment: entity.Attachment,
            ApprovedBy: entity.ApprovedBy,
            ApprovedAtUtc: entity.ApprovedAtUtc
        );

    public static List<LeaveApplicationDto> ToDtos(this IEnumerable<LeaveApplication> entities) =>
        [.. entities.Select(x => x.ToDto())];
}
