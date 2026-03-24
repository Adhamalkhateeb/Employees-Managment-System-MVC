namespace EmployeesManager.Application.Features.Employees.Common;

public interface IEmployeeCommand
{
    string FirstName { get; }
    string? MiddleName { get; }
    string LastName { get; }
    string PhoneNumber { get; }
    string EmailAddress { get; }
    string Country { get; }
    DateTime DateOfBirth { get; }
    string Address { get; }
    string Department { get; }
    string Designation { get; }
}
