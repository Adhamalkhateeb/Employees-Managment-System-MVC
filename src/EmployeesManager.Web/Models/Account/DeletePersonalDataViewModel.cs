using EmployeesManager.Contracts.Requests.Identity;

namespace EmployeesManager.Web.Models.Account;

public sealed class DeletePersonalDataViewModel
{
    public bool HasPassword { get; set; }
    public DeletePersonalDataRequest Input { get; set; } = new();
}
