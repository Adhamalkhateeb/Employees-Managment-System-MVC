using EmployeesManager.Domain.Entities.SystemCodes;
using FluentValidation;

namespace EmployeesManager.Application.Features.SystemCodes.Common;

public abstract class SystemCodeCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : ISystemCodeCommand
{
    protected void CommonRules()
    {
        RuleFor(x => x.Description)
            .MaximumLength(SystemCodeConstants.DescriptionMaxLength)
            .WithMessage(
                $"Description must not exceed {SystemCodeConstants.DescriptionMaxLength} characters."
            );

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required")
            .MaximumLength(SystemCodeConstants.CodeMaxLength)
            .WithMessage($"Code must not exceed {SystemCodeConstants.CodeMaxLength} characters.");
    }
}
