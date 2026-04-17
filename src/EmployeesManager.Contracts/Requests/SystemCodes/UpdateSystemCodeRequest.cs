using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.SystemCodes;

public sealed class UpdateSystemCodeRequest
{
    [StringLength(500)]
    public string? Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Code is required")]
    [StringLength(100)]
    public string Code { get; set; } = string.Empty;
}
