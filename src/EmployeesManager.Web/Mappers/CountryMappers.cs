using EmployeesManager.Application.Features.Countries.Dtos;
using EmployeesManager.Contracts.Responses.Countries;

namespace EmployeesManager.Web.Mappers;

public static class CountryMappers
{
    public static CountryResponse ToResponse(this CountryDto dto) =>
        new(Id: dto.Id, Code: dto.Code, Name: dto.Name);

    public static List<CountryResponse> ToResponses(this IEnumerable<CountryDto> dtos) =>
        [.. dtos.Select(x => x.ToResponse())];
}
