using EmployeesManager.Application.Features.SystemCodes.Dtos;
using EmployeesManager.Contracts.Responses.SystemCodes;

namespace EmployeesManager.Web.Mappers;

public static class SystemCodeMappers
{
    public static SystemCodeResponse ToResponse(this SystemCodeDto dto) =>
        new(Id: dto.Id, Description: dto.Description, Code: dto.Code);

    public static List<SystemCodeResponse> ToResponses(this IEnumerable<SystemCodeDto> dtos) =>
        [.. dtos.Select(x => x.ToResponse())];
}
