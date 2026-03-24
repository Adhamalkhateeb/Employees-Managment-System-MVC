using EmployeesManager.Application.Features.SystemCodeDetails.Dtos;
using EmployeesManager.Domain.Entities.SystemCodeDetails;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Mappings;

public static class SystemCodeDetailMappings
{
    public static SystemCodeDetailDto ToDto(this SystemCodeDetail entity) =>
        new(
            Id: entity.Id,
            SystemCodeId: entity.SystemCodeId,
            SystemCodeName: entity.SystemCode?.Name ?? string.Empty,
            Code: entity.Code,
            Description: entity.Description,
            OrderNo: entity.OrderNo
        );

    public static List<SystemCodeDetailDto> ToDtos(this IEnumerable<SystemCodeDetail> entities) =>
        [.. entities.Select(x => x.ToDto())];
}
