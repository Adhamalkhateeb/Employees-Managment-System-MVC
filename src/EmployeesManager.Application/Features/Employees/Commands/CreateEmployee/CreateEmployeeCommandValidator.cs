using EmployeesManager.Application.Features.Employees.Common;

namespace EmployeesManager.Application.Features.Employees.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandValidator
    : EmployeeCommandValidatorBase<CreateEmployeeCommand>
{
    public CreateEmployeeCommandValidator()
    {
        CommonRules();
    }
}
