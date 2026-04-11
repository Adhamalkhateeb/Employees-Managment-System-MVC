using EmployeesManager.Application.Features.SystemCodeDetails.Dtos;
using EmployeesManager.Domain.Entities.SystemCodeDetails;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Mappings;

public static class SystemCodeDetailMappings
{
    public static SystemCodeDetailDto ToDto(this SystemCodeDetail entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));
        return new(
            Id: entity.Id,
            SystemCodeId: entity.SystemCodeId,
            SystemCode: entity.SystemCode.Code,
            Code: entity.Code,
            Description: entity.Description,
            OrderNo: entity.OrderNo
        );
    }

    public static List<SystemCodeDetailDto> ToDtos(this IEnumerable<SystemCodeDetail> entities)
    {
        ArgumentNullException.ThrowIfNull(entities, nameof(entities));
        return [.. entities.Select(x => x.ToDto())];
    }
}
