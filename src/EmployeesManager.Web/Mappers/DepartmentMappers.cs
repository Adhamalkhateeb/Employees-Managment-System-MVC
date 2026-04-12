using EmployeesManager.Application.Features.Departments.Dtos;
using EmployeesManager.Contracts.Responses.Departments;

namespace EmployeesManager.Web.Mappers;

public static class DepartmentMappers
{
    public static DepartmentResponse ToResponse(this DepartmentDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        return new(dto.Id, Name: dto.Name, dto.EmployeesCount, dto.ManagerId, dto.ManagerFullName);
    }

    public static DepartmentResponse ToResponse(this DepartmentWithoutEmployeesDto dto)
    {
        ArgumentNullException.ThrowIfNull(dto, nameof(dto));

        return new(dto.Id, Name: dto.Name, dto.EmployeesCount, dto.ManagerId, dto.ManagerFullName);
    }

    public static List<DepartmentResponse> ToResponses(this IEnumerable<DepartmentDto> dtos)
    {
        ArgumentNullException.ThrowIfNull(dtos, nameof(dtos));
        return [.. dtos.Select(x => x.ToResponse())];
    }

    public static List<DepartmentResponse> ToResponses(
        this IEnumerable<DepartmentWithoutEmployeesDto> dtos
    )
    {
        ArgumentNullException.ThrowIfNull(dtos, nameof(dtos));
        return [.. dtos.Select(x => x.ToResponse())];
    }
}
