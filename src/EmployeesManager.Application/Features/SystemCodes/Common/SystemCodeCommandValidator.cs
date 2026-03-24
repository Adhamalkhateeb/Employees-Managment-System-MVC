using EmployeesManager.Domain.Entities.SystemCodes;
using FluentValidation;

namespace EmployeesManager.Application.Features.SystemCodes.Common;

public abstract class SystemCodeCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : ISystemCodeCommand
{
    protected void CommonRules()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(SystemCodeConstants.NameMaxLength);

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required")
            .MaximumLength(SystemCodeConstants.CodeMaxLength);
    }
}
