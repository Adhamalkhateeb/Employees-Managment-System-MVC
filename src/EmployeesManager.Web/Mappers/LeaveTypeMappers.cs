using EmployeesManager.Application.Features.LeaveTypes.Dtos;
using EmployeesManager.Contracts.Responses.LeaveTypes;

namespace EmployeesManager.Web.Mappers;

public static class LeaveTypeMappers
{
    public static LeaveTypeResponse ToResponse(this LeaveTypeDto dto) =>
        new(Id: dto.Id, Name: dto.Name);

    public static List<LeaveTypeResponse> ToResponses(this IEnumerable<LeaveTypeDto> dtos) =>
        [.. dtos.Select(x => x.ToResponse())];
}
