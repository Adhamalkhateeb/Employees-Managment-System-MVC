namespace EmployeesManager.Application.Features.Employees.Dtos;

public sealed record EmployeeDto(
    Guid Id,
    string FirstName,
    string? MiddleName,
    string LastName,
    string PhoneNumber,
    string EmailAddress,
    DateTime DateOfBirth,
    string Address,
    Guid CountryId,
    string CountryName,
    Guid DepartmentId,
    string DepartmentName,
    Guid DesignationId,
    string DesignationName
);
