using EmployeesManager.Application.Features.SystemCodes.Dtos;
using EmployeesManager.Domain.Entities.SystemCodes;

namespace EmployeesManager.Application.Features.SystemCodes.Mappings;

public static class SystemCodeMappings
{
    public static SystemCodeDto ToDto(this SystemCode entity) =>
        new(Id: entity.Id, Code: entity.Code, Description: entity.Description);

    public static List<SystemCodeDto> ToDtos(this IEnumerable<SystemCode> entities) =>
        [.. entities.Select(x => x.ToDto())];
}
