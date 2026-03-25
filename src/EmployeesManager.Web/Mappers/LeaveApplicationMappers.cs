using EmployeesManager.Application.Features.LeaveApplications.Dtos;
using EmployeesManager.Contracts.Responses.LeaveApplications;

namespace EmployeesManager.Web.Mappers;

public static class LeaveApplicationMappers
{
    public static LeaveApplicationResponse ToResponse(this LeaveApplicationDto dto) =>
        new(
            Id: dto.Id,
            EmployeeId: dto.EmployeeId,
            EmployeeName: dto.EmployeeName,
            LeaveTypeId: dto.LeaveTypeId,
            LeaveTypeName: dto.LeaveTypeName,
            DurationId: dto.DurationId,
            DurationName: dto.DurationName,
            StatusId: dto.StatusId,
            StatusName: dto.StatusName,
            StartDate: dto.StartDate,
            EndDate: dto.EndDate,
            Days: dto.Days,
            Description: dto.Description,
            Attachment: dto.Attachment,
            ApprovedBy: dto.ApprovedBy,
            ApprovedAtUtc: dto.ApprovedAtUtc
        );

    public static List<LeaveApplicationResponse> ToResponses(
        this IEnumerable<LeaveApplicationDto> dtos
    ) => [.. dtos.Select(x => x.ToResponse())];
}
