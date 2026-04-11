using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.Identity;

public sealed class ChangeEmailRequest
{
    [Required]
    [EmailAddress]
    [Display(Name = "New email")]
    public string NewEmail { get; set; } = string.Empty;
}
