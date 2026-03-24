using EmployeesManager.Application.Features.LeaveTypes.Dtos;
using EmployeesManager.Domain.Entities.LeaveTypes;

namespace EmployeesManager.Application.Features.LeaveTypes.Mappings;

public static class LeaveTypeMappings
{
    public static LeaveTypeDto ToDto(this LeaveType entity) =>
        new(Id: entity.Id, Name: entity.Name);

    public static List<LeaveTypeDto> ToDtos(this IEnumerable<LeaveType> entities) =>
        [.. entities.Select(x => x.ToDto())];
}
