using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.Employees;

public sealed class CreateEmployeeRequest
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [StringLength(100)]
    public string? MiddleName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [StringLength(25)]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Email address format is invalid")]
    [StringLength(254)]
    [DataType(DataType.EmailAddress)]
    public string EmailAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Country is required")]
    [StringLength(100)]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required")]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required(ErrorMessage = "Address is required")]
    [StringLength(250)]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department is required")]
    [StringLength(100)]
    public string Department { get; set; } = string.Empty;

    [Required(ErrorMessage = "Designation is required")]
    [StringLength(100)]
    public string Designation { get; set; } = string.Empty;
}
