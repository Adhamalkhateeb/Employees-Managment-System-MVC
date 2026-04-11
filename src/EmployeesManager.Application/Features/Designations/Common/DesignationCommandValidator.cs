using EmployeesManager.Domain.Entities.Designations;
using FluentValidation;

namespace EmployeesManager.Application.Features.Designations.Common;

public abstract class DesignationCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : IDesignationCommand
{
    protected void CommonRules()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(DesignationConstants.NameMaxLength);

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required")
            .MaximumLength(DesignationConstants.CodeMaxLength);
    }
}
