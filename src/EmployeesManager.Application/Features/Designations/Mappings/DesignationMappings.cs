using EmployeesManager.Application.Features.Designations.Dtos;
using EmployeesManager.Domain.Entities.Designations;

namespace EmployeesManager.Application.Features.Designations.Mappings;

public static class DesignationMappings
{
    public static DesignationDto ToDto(this Designation entity) =>
        new(Id: entity.Id, Name: entity.Name, Code: entity.Code);

    public static List<DesignationDto> ToDtos(this IEnumerable<Designation> entities) =>
        [.. entities.Select(x => x.ToDto())];
}
