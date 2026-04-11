using FluentValidation;

namespace EmployeesManager.Application.Features.Identity.Authentication.Commands.Login;

public sealed class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Enter a valid email address");

        RuleFor(x => x.Password).NotEmpty().WithMessage("Password is required");
    }
}
