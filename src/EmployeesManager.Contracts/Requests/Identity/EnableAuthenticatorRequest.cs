using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.Identity;

public sealed class EnableAuthenticatorRequest
{
    [Required]
    [StringLength(7, MinimumLength = 6)]
    [Display(Name = "Verification code")]
    public string Code { get; set; } = string.Empty;
}
