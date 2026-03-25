namespace EmployeesManager.Contracts.Responses.Cities;

public sealed record CityResponse(
    Guid Id,
    string Code,
    string Name,
    Guid CountryId,
    string CountryName
);
