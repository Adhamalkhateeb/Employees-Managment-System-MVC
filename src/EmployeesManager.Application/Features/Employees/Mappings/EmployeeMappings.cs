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
            LastName: entity.LastName,
            NationalId: entity.NationalId,
            PhoneNumber: entity.PhoneNumber,
            EmailAddress: entity.EmailAddress,
            HireDate: entity.HireDate,
            Address: entity.Address,
            DepartmentId: entity.DepartmentId,
            DepartmentName: entity.Department?.Name ?? string.Empty,
            BranchId: entity.BranchId,
            BranchName: entity.Branch?.Name
        );
    }

    public static List<EmployeeDto> ToDtos(this IEnumerable<Employee> entities)
    {
        ArgumentNullException.ThrowIfNull(entities, nameof(entities));
        return [.. entities.Select(x => x.ToDto())];
    }
}
