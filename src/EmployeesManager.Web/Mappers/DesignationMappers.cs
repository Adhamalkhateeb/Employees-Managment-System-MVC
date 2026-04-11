using EmployeesManager.Application.Features.Designations.Dtos;
using EmployeesManager.Contracts.Responses.Designations;

namespace EmployeesManager.Web.Mappers;

public static class DesignationMappers
{
    public static DesignationResponse ToResponse(this DesignationDto dto) =>
        new(Id: dto.Id, Name: dto.Name, Code: dto.Code);

    public static List<DesignationResponse> ToResponses(this IEnumerable<DesignationDto> dtos) =>
        [.. dtos.Select(x => x.ToResponse())];
}
