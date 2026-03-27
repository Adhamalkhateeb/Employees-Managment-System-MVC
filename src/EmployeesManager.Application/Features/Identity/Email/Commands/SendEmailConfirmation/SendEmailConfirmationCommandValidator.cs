using FluentValidation;

namespace EmployeesManager.Application.Features.Identity.Email.Commands.SendEmailConfirmation;

public sealed class SendEmailConfirmationCommandValidator
    : AbstractValidator<SendEmailConfirmationCommand>
{
    public SendEmailConfirmationCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required")
            .EmailAddress()
            .WithMessage("Enter a valid email address");

        RuleFor(x => x.ConfirmationBaseUrl)
            .NotEmpty()
            .WithMessage("Confirmation callback URL is required")
            .Must(BeAValidUrl)
            .WithMessage("Enter a valid confirmation callback URL");
    }

    private bool BeAValidUrl(string url) => Uri.TryCreate(url, UriKind.Absolute, out _);
}
