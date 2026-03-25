    

namespace EmployeesManager.Application.Features.LeaveApplications.Common;

public interface ILeaveApplicationCommand
{
    Guid EmployeeId { get; }
    Guid LeaveTypeId { get; }
    Guid DurationId { get; }
    Guid StatusId { get; }
    DateTimeOffset StartDate { get; }
    DateTimeOffset EndDate { get; }
    string Description { get; }
    string? Attachment { get; }
}
