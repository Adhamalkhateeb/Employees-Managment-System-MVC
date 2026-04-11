// Web/Mappers/LeaveApplicationMapper.cs
using EmployeesManager.Application.Features.LeaveApplications.Commands.CreateLeaveApplication;
using EmployeesManager.Application.Features.LeaveApplications.Commands.UpdateLeaveApplication;
using EmployeesManager.Application.Features.LeaveApplications.Dtos;
using EmployeesManager.Contracts.Requests.LeaveApplications;
using EmployeesManager.Contracts.Responses.LeaveApplications;
using EmployeesManager.Domain.Entities.LeaveApplications.Enums;

namespace EmployeesManager.Web.Mappers;

public static class LeaveApplicationMapper
{
    public static CreateLeaveApplicationCommand ToCommand(
        this CreateLeaveApplicationRequest request
    ) =>
        new(
            request.EmployeeId,
            request.LeaveTypeId,
            ParseDuration(request.Duration),
            request.StartDate,
            request.EndDate,
            request.Description,
            request.Attachment
        );

    public static UpdateLeaveApplicationCommand ToCommand(
        this UpdateLeaveApplicationRequest request,
        Guid id,
        LeaveApplicationStatus status
    ) =>
        new(
            Id: id,
            EmployeeId: request.EmployeeId,
            LeaveTypeId: request.LeaveTypeId,
            Duration: ParseDuration(request.Duration),
            Status: status,
            StartDate: request.StartDate,
            EndDate: request.EndDate,
            Description: request.Description,
            Attachment: request.Attachment
        );

    public static LeaveApplicationResponse ToResponse(this LeaveApplicationDto dto) =>
        new(
            Id: dto.Id,
            EmployeeId: dto.EmployeeId,
            EmployeeName: dto.EmployeeName,
            LeaveTypeId: dto.LeaveTypeId,
            LeaveTypeName: dto.LeaveTypeName,
            DurationId: Guid.Empty,
            DurationName: dto.Duration.ToString(),
            Status: dto.Status.ToString(),
            StartDate: dto.StartDate,
            EndDate: dto.EndDate,
            Days: dto.Days,
            Description: dto.Description,
            Attachment: dto.Attachment,
            RejectionReason: dto.RejectionReason,
            ApprovedBy: dto.DecisionBy,
            ApprovedAtUtc: dto.DecisionAtUtc
        );

    public static List<LeaveApplicationResponse> ToResponses(
        this IEnumerable<LeaveApplicationDto> dtos
    ) => [.. dtos.Select(dto => dto.ToResponse())];

    public static UpdateLeaveApplicationRequest ToUpdateRequest(this LeaveApplicationDto dto) =>
        new()
        {
            EmployeeId = dto.EmployeeId,
            LeaveTypeId = dto.LeaveTypeId,
            Duration = dto.Duration.ToString(),
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            Description = dto.Description,
            Attachment = dto.Attachment,
        };

    private static LeaveApplicationDurations ParseDuration(string duration) =>
        Enum.Parse<LeaveApplicationDurations>(duration, ignoreCase: true);
}
