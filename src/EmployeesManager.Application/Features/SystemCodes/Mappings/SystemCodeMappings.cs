using EmployeesManager.Application.Features.SystemCodes.Dtos;
using EmployeesManager.Domain.Entities.SystemCodes;

namespace EmployeesManager.Application.Features.SystemCodes.Mappings;

public static class SystemCodeMappings
{
    public static SystemCodeDto ToDto(this SystemCode entity) =>
        new(Id: entity.Id, Name: entity.Name, Code: entity.Code);

    public static List<SystemCodeDto> ToDtos(this IEnumerable<SystemCode> entities) =>
        [.. entities.Select(x => x.ToDto())];
}
