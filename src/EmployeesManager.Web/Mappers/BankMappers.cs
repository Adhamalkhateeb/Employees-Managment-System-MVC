using EmployeesManager.Application.Features.Banks.Dtos;
using EmployeesManager.Contracts.Responses.Banks;

namespace EmployeesManager.Web.Mappers;

public static class BankMappers
{
    public static BankResponse ToResponse(this BankDto dto) =>
        new(Id: dto.Id, Code: dto.Code, Name: dto.Name, AccountNo: dto.AccountNo);

    public static List<BankResponse> ToResponses(this IEnumerable<BankDto> dtos) =>
        [.. dtos.Select(x => x.ToResponse())];
}
