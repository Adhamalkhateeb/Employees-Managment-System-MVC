using EmployeesManager.Application.Features.Employees.Dtos;
using EmployeesManager.Domain.Entities.Employees;

namespace EmployeesManager.Application.Features.Employees.Mappings;

public static class EmployeeMappings
{
    public static EmployeeDto ToDto(this Employee entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        return new(
            Id: entity.Id,
            FirstName: entity.FirstName,
            MiddleName: entity.MiddleName,
            LastName: entity.LastName,
            PhoneNumber: entity.PhoneNumber,
            EmailAddress: entity.EmailAddress,
            Country: entity.Country,
            DateOfBirth: entity.DateOfBirth,
            Address: entity.Address,
            Department: entity.Department,
            Designation: entity.Designation
        );
    }

    public static List<EmployeeDto> ToDtos(this IEnumerable<Employee> entities)
    {
        ArgumentNullException.ThrowIfNull(entities, nameof(entities));
        return [.. entities.Select(x => x.ToDto())];
    }
}
