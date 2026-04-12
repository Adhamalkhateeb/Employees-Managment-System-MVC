namespace EmployeesManager.Application.Features.Employees.Common;

public interface IEmployeeCommand
{
    string FirstName { get; }
    string LastName { get; }
    string NationalId { get; }
    string PhoneNumber { get; }
    string EmailAddress { get; }
    DateTime HireDate { get; }
    string Address { get; }
    Guid DepartmentId { get; }
    Guid? BranchId { get; }
}
