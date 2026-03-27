using FluentValidation;

namespace EmployeesManager.Application.Features.Identity.Email.Commands.ConfirmEmailChange;

public sealed class ConfirmEmailChangeCommandValidator
    : AbstractValidator<ConfirmEmailChangeCommand>
{
    public ConfirmEmailChangeCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("Invalid email address.");
        RuleFor(x => x.Code).NotEmpty().WithMessage("Confirmation code is required.");
    }
}
