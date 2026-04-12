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
            LeaveTypeName: entity.LeaveType?.Code ?? entity.LeaveType?.Name ?? string.Empty,
            Duration: entity.Duration,
            Status: entity.Status,
            StartDate: entity.StartDate,
            EndDate: entity.EndDate,
            Days: entity.EndDate.Date.Subtract(entity.StartDate.Date).Days + 1,
            Description: entity.Description,
            Attachment: entity.Attachment,
            RejectionReason: entity.RejectionReason,
            DecisionById: entity.DecisionById,
            DecisionBy: null,
            DecisionAtUtc: entity.DecisionAtUtc
        );

    public static List<LeaveApplicationDto> ToDtos(this IEnumerable<LeaveApplication> entities) =>
        [.. entities.Select(x => x.ToDto())];
}
