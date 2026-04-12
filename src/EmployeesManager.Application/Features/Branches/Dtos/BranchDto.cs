namespace EmployeesManager.Application.Features.Branches.Dtos;

public sealed record BranchDto(
    Guid Id,
    string Name,
    string Address,
    string PhoneNumber,
    string EmailAddress,
    Guid? ManagerId
);
