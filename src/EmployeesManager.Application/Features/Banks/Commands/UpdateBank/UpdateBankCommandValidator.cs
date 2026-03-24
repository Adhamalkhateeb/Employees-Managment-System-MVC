using EmployeesManager.Application.Features.Banks.Common;
using FluentValidation;

namespace EmployeesManager.Application.Features.Banks.Commands.UpdateBank;

public sealed class UpdateBankCommandValidator : BankCommandValidatorBase<UpdateBankCommand>
{
    public UpdateBankCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

        CommonRules();
    }
}
