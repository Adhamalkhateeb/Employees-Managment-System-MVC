using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.Departments;

public sealed class CreateDepartmentRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Code is required")]
    [StringLength(50)]
    public string Code { get; set; } = string.Empty;
}
