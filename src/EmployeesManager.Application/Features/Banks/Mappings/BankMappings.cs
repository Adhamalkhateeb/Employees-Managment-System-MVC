using EmployeesManager.Application.Features.Banks.Dtos;
using EmployeesManager.Domain.Entities.Banks;

namespace EmployeesManager.Application.Features.Banks.Mappings;

public static class BankMappings
{
    public static BankDto ToDto(this Bank entity) =>
        new(Id: entity.Id, Code: entity.Code, Name: entity.Name, AccountNo: entity.AccountNo);

    public static List<BankDto> ToDtos(this IEnumerable<Bank> entities) =>
        [.. entities.Select(x => x.ToDto())];
}
