using FluentValidation;

namespace EmployeesManager.Application.Features.Identity.Email.Commands.ConfirmEmail;

public sealed class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User ID is required.");
        RuleFor(x => x.Code).NotEmpty().WithMessage("Confirmation code is required.");
    }
}
