using EmployeesManager.Contracts.Requests.Identity;

namespace EmployeesManager.Web.Models.Account;

public sealed class ManageProfileViewModel
{
    public string UserName { get; set; } = string.Empty;
    public ManageProfileRequest Input { get; set; } = new();
}
