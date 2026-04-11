using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.Identity;

public sealed class ExternalLoginConfirmationRequest
{
    [Required]
    [EmailAddress]
    [Display(Name = "Email")]
    public string Email { get; set; } = string.Empty;
}
