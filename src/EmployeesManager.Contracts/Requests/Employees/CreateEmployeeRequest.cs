using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.Employees;

public sealed class CreateEmployeeRequest
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "National ID is required")]
    [StringLength(50)]
    public string NationalId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [StringLength(25)]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Email address format is invalid")]
    [StringLength(254)]
    [DataType(DataType.EmailAddress)]
    public string EmailAddress { get; set; } = string.Empty;

    [Required(ErrorMessage = "Hire date is required")]
    [DataType(DataType.Date)]
    public DateTime HireDate { get; set; }

    [Required(ErrorMessage = "Address is required")]
    [StringLength(250)]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department is required")]
    public Guid DepartmentId { get; set; }

    public Guid? BranchId { get; set; }
}
