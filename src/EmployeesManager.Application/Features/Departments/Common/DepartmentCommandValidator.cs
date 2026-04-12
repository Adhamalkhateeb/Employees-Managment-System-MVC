using EmployeesManager.Domain.Entities.Departments;
using FluentValidation;

namespace EmployeesManager.Application.Features.Departments.Common;

public abstract class DepartmentCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : IDepartmentCommand
{
    protected void CommonRules()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(DepartmentConstants.NameMaxLength);
    }
}
