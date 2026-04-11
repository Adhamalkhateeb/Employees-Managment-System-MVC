using EmployeesManager.Contracts.Requests.Identity;

namespace EmployeesManager.Web.Models.Account;

public sealed class ManageEmailViewModel
{
    public string Email { get; set; } = string.Empty;
    public bool IsEmailConfirmed { get; set; }
    public ChangeEmailRequest Input { get; set; } = new();
}
