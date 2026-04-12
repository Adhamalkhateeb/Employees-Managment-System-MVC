using EmployeesManager.Application.Features.Employees.Dtos;
using EmployeesManager.Contracts.Responses.Employees;

namespace EmployeesManager.Web.Mappers
{
    public static class EmployeeMapper
    {
        public static EmployeeResponse ToResponse(this EmployeeDto dto) =>
            new EmployeeResponse
            {
                Id = dto.Id,
                EmployeeNumber = $"EMP-{dto.Id.ToString().Substring(0, 8).ToUpper()}",
                FullName = $"{dto.FirstName} {dto.LastName}".Trim(),
                NationalId = dto.NationalId,
                PhoneNumber = dto.PhoneNumber,
                EmailAddress = dto.EmailAddress,
                HireDate = dto.HireDate,
                Address = dto.Address,
                DepartmentId = dto.DepartmentId,
                DepartmentName = dto.DepartmentName,
                BranchId = dto.BranchId,
                BranchName = dto.BranchName,
            };

        public static List<EmployeeResponse> ToResponses(this IEnumerable<EmployeeDto> dtos) =>
            [.. dtos.Select(x => x.ToResponse())];
    }
}
