namespace EmployeesManager.Web.Models.Account;

public sealed class RecoveryCodesViewModel
{
    public IReadOnlyList<string> RecoveryCodes { get; set; } = Array.Empty<string>();
}
