using EmployeesManager.Domain.Entities.Banks;
using FluentValidation;

namespace EmployeesManager.Application.Features.Banks.Common;

public abstract class BankCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : IBankCommand
{
    protected void CommonRules()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required")
            .MaximumLength(BankConstants.CodeMaxLength);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(BankConstants.NameMaxLength);

        RuleFor(x => x.AccountNo)
            .NotEmpty()
            .WithMessage("Account number is required")
            .MaximumLength(BankConstants.AccountNoMaxLength);
    }
}
