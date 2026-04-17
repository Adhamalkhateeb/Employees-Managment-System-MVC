using EmployeesManager.Application.Features.Countries.Dtos;
using EmployeesManager.Domain.Entities.Countries;

namespace EmployeesManager.Application.Features.Countries.Mappings;

public static class CountryMappings
{
    public static CountryDto ToDto(this Country entity) =>
        new(Id: entity.Id, Code: entity.Code, Name: entity.Name);

    public static List<CountryDto> ToDtos(this IEnumerable<Country> entities) =>
        [.. entities.Select(x => x.ToDto())];
}
