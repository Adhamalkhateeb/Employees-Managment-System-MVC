namespace EmployeesManager.Application.Features.Cities.Common;

public interface ICityCommand
{
    string Code { get; }
    string Name { get; }
    Guid CountryId { get; }
}
