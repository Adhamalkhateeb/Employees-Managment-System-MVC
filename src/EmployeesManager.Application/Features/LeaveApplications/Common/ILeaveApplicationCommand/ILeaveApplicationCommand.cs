using EmployeesManager.Domain.Entities.LeaveApplications.Enums;

namespace EmployeesManager.Application.Features.LeaveApplications.Common;

public interface ILeaveApplicationCommand
{
    Guid EmployeeId { get; }
    Guid LeaveTypeId { get; }
    LeaveApplicationDurations Duration { get; }
    DateTimeOffset StartDate { get; }
    DateTimeOffset EndDate { get; }
    string Description { get; }
    string? Attachment { get; }
}
