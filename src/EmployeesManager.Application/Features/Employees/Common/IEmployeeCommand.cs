namespace EmployeesManager.Application.Features.Employees.Common;

public interface IEmployeeCommand
{
    string FirstName { get; }
    string? MiddleName { get; }
    string LastName { get; }
    string PhoneNumber { get; }
    string EmailAddress { get; }
    DateTime DateOfBirth { get; }
    string Address { get; }
    Guid CountryId { get; }
    Guid DepartmentId { get; }
    Guid DesignationId { get; }
}
