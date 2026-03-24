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
                FullName =
                    $"{dto.FirstName} {(string.IsNullOrEmpty(dto.MiddleName) ? "" : dto.MiddleName + " ")}{dto.LastName}".Trim(),
                PhoneNumber = dto.PhoneNumber,
                EmailAddress = dto.EmailAddress,
                Country = dto.Country,
                DateOfBirth = dto.DateOfBirth,
            };

        public static List<EmployeeResponse> ToResponses(this IEnumerable<EmployeeDto> dtos) =>
            [.. dtos.Select(x => x.ToResponse())];
    }
}
