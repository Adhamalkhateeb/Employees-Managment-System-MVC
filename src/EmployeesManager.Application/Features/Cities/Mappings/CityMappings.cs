using EmployeesManager.Application.Features.Cities.Dtos;
using EmployeesManager.Domain.Entities.Cities;

namespace EmployeesManager.Application.Features.Cities.Mappings;

public static class CityMappings
{
    public static CityDto ToDto(this City entity) =>
        new(
            Id: entity.Id,
            Code: entity.Code,
            Name: entity.Name,
            CountryId: entity.CountryId,
            CountryName: entity.Country?.Name ?? string.Empty
        );

    public static List<CityDto> ToDtos(this IEnumerable<City> entities) =>
        [.. entities.Select(x => x.ToDto())];
}
