using EmployeesManager.Application.Features.Departments.Dtos;
using EmployeesManager.Application.Features.Employees.Dtos;
using EmployeesManager.Domain.Entities.Departments;

namespace EmployeesManager.Application.Features.Departments.Mappings;

public static class DepartmentMappings
{
    public static DepartmentDto ToDto(this Department entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var employees = entity
            .Employees.Select(emp => new EmployeeDto(
                emp.Id,
                emp.FirstName,
                emp.LastName,
                emp.NationalId,
                emp.PhoneNumber,
                emp.EmailAddress,
                emp.HireDate,
                emp.Address,
                emp.DepartmentId,
                entity.Name,
                emp.BranchId,
                emp.Branch?.Name
            ))
            .ToList();

        return new DepartmentDto(
            entity.Id,
            entity.Name,
            employees.Count,
            entity.ManagerId,
            entity.Manager is null ? null : $"{entity.Manager.FirstName} {entity.Manager.LastName}",
            employees
        );
    }

    public static List<DepartmentDto> ToDtos(this IEnumerable<Department> entities)
    {
        ArgumentNullException.ThrowIfNull(entities, nameof(entities));
        return [.. entities.Select(x => x.ToDto())];
    }
}
