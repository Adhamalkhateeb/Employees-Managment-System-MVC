namespace EmployeesManager.Application.Features.Employees.Dtos;

public sealed record EmployeeDto(
    Guid Id,
    string FirstName,
    string LastName,
    string NationalId,
    string PhoneNumber,
    string EmailAddress,
    DateTime HireDate,
    string Address,
    Guid DepartmentId,
    string DepartmentName,
    Guid? BranchId,
    string? BranchName
);
