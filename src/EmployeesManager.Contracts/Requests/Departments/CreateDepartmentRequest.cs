using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.Departments;

public sealed class CreateDepartmentRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    public Guid? ManagerId { get; set; }
}
