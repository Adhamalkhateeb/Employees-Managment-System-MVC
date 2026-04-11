using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.SystemCodeDetails;

public sealed class UpdateSystemCodeDetailRequest
{
    [Required(ErrorMessage = "System code is required")]
    public Guid SystemCodeId { get; set; }

    [Required(ErrorMessage = "Code is required")]
    [StringLength(100)]
    public string Code { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; } = string.Empty;

    [Range(0, int.MaxValue, ErrorMessage = "Order number must be greater than or equal to zero")]
    public int? OrderNo { get; set; }
}
