namespace EmployeesManager.Web.Models.Account;

public sealed class ExternalLoginsViewModel
{
    public IReadOnlyList<ExternalLoginItemViewModel> CurrentLogins { get; set; } =
        Array.Empty<ExternalLoginItemViewModel>();

    public IReadOnlyList<ExternalProviderItemViewModel> OtherLogins { get; set; } =
        Array.Empty<ExternalProviderItemViewModel>();

    public bool ShowRemoveButton { get; set; }
}

public sealed class ExternalLoginItemViewModel
{
    public string LoginProvider { get; set; } = string.Empty;
    public string ProviderKey { get; set; } = string.Empty;
    public string ProviderDisplayName { get; set; } = string.Empty;
}

public sealed class ExternalProviderItemViewModel
{
    public string Name { get; set; } = string.Empty;
    public string DisplayName { get; set; } = string.Empty;
}
