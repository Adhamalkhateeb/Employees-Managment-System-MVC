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
<<<<<<< HEAD
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
=======
                FullName =
                    $"{dto.FirstName} {(string.IsNullOrEmpty(dto.MiddleName) ? "" : dto.MiddleName + " ")}{dto.LastName}".Trim(),
                PhoneNumber = dto.PhoneNumber,
                EmailAddress = dto.EmailAddress,
                CountryId = dto.CountryId,
                CountryName = dto.CountryName,
                DateOfBirth = dto.DateOfBirth,
                Address = dto.Address,
                DepartmentId = dto.DepartmentId,
                DepartmentName = dto.DepartmentName,
                DesignationId = dto.DesignationId,
                DesignationName = dto.DesignationName,
>>>>>>> main
            };

        public static List<EmployeeResponse> ToResponses(this IEnumerable<EmployeeDto> dtos) =>
            [.. dtos.Select(x => x.ToResponse())];
    }
}
