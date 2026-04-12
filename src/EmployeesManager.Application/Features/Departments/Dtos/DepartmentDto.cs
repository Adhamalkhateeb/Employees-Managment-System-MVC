using EmployeesManager.Application.Features.Employees.Dtos;

namespace EmployeesManager.Application.Features.Departments.Dtos;

public sealed record DepartmentDto(
    Guid Id,
    string Name,
    int EmployeesCount,
    Guid? ManagerId,
    string? ManagerFullName,
    IEnumerable<EmployeeDto> Employees
);

public sealed record DepartmentWithoutEmployeesDto(
    Guid Id,
    string Name,
    int EmployeesCount,
    Guid? ManagerId,
    string? ManagerFullName
);
