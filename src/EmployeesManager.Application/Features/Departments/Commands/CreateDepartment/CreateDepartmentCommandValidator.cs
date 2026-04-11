using EmployeesManager.Application.Features.Departments.Common;

namespace EmployeesManager.Application.Features.Departments.Commands.CreateDepartment;

public sealed class CreateDepartmentCommandValidator
    : DepartmentCommandValidatorBase<CreateDepartmentCommand>
{
    public CreateDepartmentCommandValidator()
    {
        CommonRules();
    }
}
