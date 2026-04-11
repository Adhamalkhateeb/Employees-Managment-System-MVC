using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.Identity;

public sealed class ManageProfileRequest
{
    [Phone]
    [Display(Name = "Phone number")]
    public string? PhoneNumber { get; set; }
}
