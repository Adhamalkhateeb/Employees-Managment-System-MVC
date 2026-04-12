using EmployeesManager.Application.Features.Branches.Dtos;
using EmployeesManager.Contracts.Responses.Branchs;

namespace EmployeesManager.Web.Mappers;

public static class BranchMappers
{
    public static BranchResponse ToResponse(this BranchDto dto) =>
        new(
            Id: dto.Id,
            Name: dto.Name,
            Address: dto.Address,
            PhoneNumber: dto.PhoneNumber,
            EmailAddress: dto.EmailAddress,
            ManagerId: dto.ManagerId
        );

    public static List<BranchResponse> ToResponses(this IEnumerable<BranchDto> dtos) =>
        [.. dtos.Select(x => x.ToResponse())];
}
