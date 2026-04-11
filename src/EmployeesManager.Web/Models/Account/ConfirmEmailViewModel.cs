namespace EmployeesManager.Web.Models.Account;

public sealed record ConfirmEmailViewModel
{
    public bool Succeeded { get; init; }
    public string Message { get; init; } = string.Empty;
    public string? ReturnUrl { get; init; }
}
