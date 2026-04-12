using EmployeesManager.Domain.Entities.Branches;
using FluentValidation;

namespace EmployeesManager.Application.Features.Branches.Common;

public abstract class BranchCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : IBranchCommand
{
    protected void CommonRules()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required.")
            .MaximumLength(BranchConstants.NameMaxLength)
            .WithMessage($"Name must not exceed {BranchConstants.NameMaxLength} characters.");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required.")
            .MaximumLength(BranchConstants.AddressMaxLength)
            .WithMessage($"Address must not exceed {BranchConstants.AddressMaxLength} characters.");

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required.")
            .Matches(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$")
            .WithMessage("Invalid phone number format.")
            .MaximumLength(BranchConstants.PhoneMaxLength)
            .WithMessage(
                $"Phone number must not exceed {BranchConstants.PhoneMaxLength} characters."
            );

        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .WithMessage("Email address is required.")
            .EmailAddress()
            .WithMessage("Invalid email address format.")
            .MaximumLength(BranchConstants.EmailMaxLength)
            .WithMessage(
                $"Email address must not exceed {BranchConstants.EmailMaxLength} characters."
            );
    }
}
