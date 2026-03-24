using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.Designations;

public sealed class CreateDesignationRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
}
