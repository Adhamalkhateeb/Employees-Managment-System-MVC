using EmployeesManager.Application.Features.SystemCodeDetails.Dtos;
using EmployeesManager.Contracts.Responses.SystemCodeDetails;

namespace EmployeesManager.Web.Mappers;

public static class SystemCodeDetailMappers
{
    public static SystemCodeDetailResponse ToResponse(this SystemCodeDetailDto dto) =>
        new(
            Id: dto.Id,
            SystemCodeId: dto.SystemCodeId,
            SystemCode: dto.SystemCode,
            Code: dto.Code,
            Description: dto.Description,
            OrderNo: dto.OrderNo
        );

    public static List<SystemCodeDetailResponse> ToResponses(
        this IEnumerable<SystemCodeDetailDto> dtos
    ) => [.. dtos.Select(x => x.ToResponse())];
}
