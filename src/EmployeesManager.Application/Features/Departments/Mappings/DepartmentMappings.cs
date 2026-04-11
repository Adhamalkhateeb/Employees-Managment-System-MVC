using EmployeesManager.Application.Features.Departments.Dtos;
using EmployeesManager.Domain.Entities.Departments;

namespace EmployeesManager.Application.Features.Departments.Mappings;

public static class DepartmentMappings
{
    public static DepartmentDto ToDto(this Department entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        return new(Id: entity.Id, Name: entity.Name, Code: entity.Code);
    }

    public static List<DepartmentDto> ToDtos(this IEnumerable<Department> entities)
    {
        ArgumentNullException.ThrowIfNull(entities, nameof(entities));
        return [.. entities.Select(x => x.ToDto())];
    }
}
