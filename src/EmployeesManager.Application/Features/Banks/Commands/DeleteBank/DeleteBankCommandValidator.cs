using FluentValidation;

namespace EmployeesManager.Application.Features.Banks.Commands.DeleteBank;

public sealed class DeleteBankCommandValidator : AbstractValidator<DeleteBankCommand>
{
    public DeleteBankCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
