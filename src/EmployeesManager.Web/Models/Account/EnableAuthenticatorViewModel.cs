using EmployeesManager.Contracts.Requests.Identity;

namespace EmployeesManager.Web.Models.Account;

public sealed class EnableAuthenticatorViewModel
{
    public string SharedKey { get; set; } = string.Empty;
    public string AuthenticatorUri { get; set; } = string.Empty;
    public EnableAuthenticatorRequest Input { get; set; } = new();
}
