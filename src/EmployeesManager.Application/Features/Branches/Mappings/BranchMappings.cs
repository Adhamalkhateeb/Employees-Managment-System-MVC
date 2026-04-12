using EmployeesManager.Application.Features.Branches.Dtos;
using EmployeesManager.Domain.Entities.Branches;

namespace EmployeesManager.Application.Features.Branches.Mappings;

public static class BranchMappings
{
    public static BranchDto ToDto(this Branch entity) =>
        new(
            Id: entity.Id,
            Name: entity.Name,
            Address: entity.Address,
            PhoneNumber: entity.Phone.Value,
            EmailAddress: entity.Email.Value,
            ManagerId: entity.ManagerId
        );

    public static List<BranchDto> ToDtos(this IEnumerable<Branch> entities) =>
        [.. entities.Select(x => x.ToDto())];
}
