using EmployeesManager.Application.Features.Cities.Dtos;
using EmployeesManager.Contracts.Responses.Cities;

namespace EmployeesManager.Web.Mappers;

public static class CityMappers
{
    public static CityResponse ToResponse(this CityDto dto) =>
        new(
            Id: dto.Id,
            Code: dto.Code,
            Name: dto.Name,
            CountryId: dto.CountryId,
            CountryName: dto.CountryName
        );

    public static List<CityResponse> ToResponses(this IEnumerable<CityDto> dtos) =>
        [.. dtos.Select(x => x.ToResponse())];
}
