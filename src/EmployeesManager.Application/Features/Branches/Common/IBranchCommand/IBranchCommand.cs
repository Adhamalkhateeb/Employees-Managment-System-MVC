namespace EmployeesManager.Application.Features.Branches.Common;

public interface IBranchCommand
{
    string Name { get; init; }
    string Address { get; init; }
    string PhoneNumber { get; init; }
    string EmailAddress { get; init; }
    Guid? ManagerId { get; init; }
}
