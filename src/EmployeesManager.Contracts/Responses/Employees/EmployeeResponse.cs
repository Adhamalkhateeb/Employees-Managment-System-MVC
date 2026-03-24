namespace EmployeesManager.Contracts.Responses.Employees;

public sealed class EmployeeResponse
{
    public Guid Id { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string Address { get; set; } = string.Empty;

    public string Department { get; set; } = string.Empty;

    public string Designation { get; set; } = string.Empty;
}
