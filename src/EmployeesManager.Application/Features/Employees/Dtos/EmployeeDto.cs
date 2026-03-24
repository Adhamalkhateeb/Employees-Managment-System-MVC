namespace EmployeesManager.Application.Features.Employees.Dtos;

public sealed record EmployeeDto(
    Guid Id,
    string FirstName,
    string? MiddleName,
    string LastName,
    string PhoneNumber,
    string EmailAddress,
    string Country,
    DateTime DateOfBirth,
    string Address,
    string Department,
    string Designation
);
