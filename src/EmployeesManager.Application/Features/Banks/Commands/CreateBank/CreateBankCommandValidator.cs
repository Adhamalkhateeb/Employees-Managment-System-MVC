using EmployeesManager.Application.Features.Banks.Common;

namespace EmployeesManager.Application.Features.Banks.Commands.CreateBank;

public sealed class CreateBankCommandValidator : BankCommandValidatorBase<CreateBankCommand>
{
    public CreateBankCommandValidator()
    {
        CommonRules();
    }
}
