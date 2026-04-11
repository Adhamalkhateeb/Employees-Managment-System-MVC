namespace EmployeesManager.Web.Models.Account;

public sealed class TwoFactorAuthenticationViewModel
{
    public bool HasAuthenticator { get; set; }
    public bool IsTwoFactorEnabled { get; set; }
    public int RecoveryCodesLeft { get; set; }
    public bool IsMachineRemembered { get; set; }
}
