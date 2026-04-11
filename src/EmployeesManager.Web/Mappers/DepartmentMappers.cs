using EmployeesManager.Application.Features.Departments.Dtos;
using EmployeesManager.Contracts.Responses.Departments;

namespace EmployeesManager.Web.Mappers;

public static class DepartmentMappers
{
    public static DepartmentResponse ToResponse(this DepartmentDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        return new(Id: dto.Id, Name: dto.Name, Code: dto.Code);
    }

    public static List<DepartmentResponse> ToResponses(this IEnumerable<DepartmentDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos, nameof(dtos));
        return [.. dtos.Select(x => x.ToResponse())];
    }
}
