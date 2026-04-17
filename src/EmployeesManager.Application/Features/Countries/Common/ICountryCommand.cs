namespace EmployeesManager.Application.Features.Countries.Common;

public interface ICountryCommand
{
    string Code { get; }
    string Name { get; }
}
