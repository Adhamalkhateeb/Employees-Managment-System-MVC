using System.ComponentModel.DataAnnotations;

namespace EmployeesManager.Contracts.Requests.Branchs;

public sealed class CreateBranchRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required")]
    [StringLength(255)]
    public string Address { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone number is required")]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email address is required")]
    [EmailAddress(ErrorMessage = "Email address is invalid")]
    [StringLength(100)]
    public string EmailAddress { get; set; } = string.Empty;

    public Guid? ManagerId { get; set; }
}
