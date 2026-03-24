using EmployeesManager.Application.Features.Departments.Common;
using FluentValidation;

namespace EmployeesManager.Application.Features.Departments.Commands.UpdateDepartment;

public sealed class UpdateDepartmentCommandValidator
    : DepartmentCommandValidatorBase<UpdateDepartmentCommand>
{
    public UpdateDepartmentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

        CommonRules();
    }
}
