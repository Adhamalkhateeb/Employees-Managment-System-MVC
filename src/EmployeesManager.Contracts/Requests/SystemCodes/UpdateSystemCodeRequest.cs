using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.SystemCodes;

public sealed class UpdateSystemCodeRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Code is required")]
    [StringLength(50)]
    public string Code { get; set; } = string.Empty;
}
