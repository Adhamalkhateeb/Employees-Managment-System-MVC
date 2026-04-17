namespace EmployeesManager.Contracts.Responses.Employees;

public sealed class EmployeeResponse
{
    public Guid Id { get; set; }
    public string EmployeeNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    public string EmailAddress { get; set; } = string.Empty;

    public Guid CountryId { get; set; }
    public string CountryName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    public string Address { get; set; } = string.Empty;

    public Guid DepartmentId { get; set; }
    public string DepartmentName { get; set; } = string.Empty;

    public Guid DesignationId { get; set; }
    public string DesignationName { get; set; } = string.Empty;
}
