using EmployeesManager.Application.Features.Employees.Common;
using FluentValidation;

namespace EmployeesManager.Application.Features.Employees.Commands.UpdateEmployee;

public sealed class UpdateEmployeeCommandValidator
    : EmployeeCommandValidatorBase<UpdateEmployeeCommand>
{
    public UpdateEmployeeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

        CommonRules();
    }
}
