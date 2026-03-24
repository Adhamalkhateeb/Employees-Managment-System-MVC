namespace EmployeesManager.Application.Features.Cities.Dtos;

public sealed record CityDto(Guid Id, string Code, string Name, Guid CountryId, string CountryName);
