namespace EmployeesManager.Contracts.Responses.Branchs;

public sealed record BranchResponse(
    Guid Id,
    string Name,
    string Address,
    string PhoneNumber,
    string EmailAddress,
    Guid? ManagerId
);
