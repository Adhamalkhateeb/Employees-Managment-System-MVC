using FluentValidation;

namespace EmployeesManager.Application.Features.Identity.Email.Commands.SendVerificationEmail;

public sealed class SendVerificationEmailCommandValidator
    : AbstractValidator<SendVerificationEmailCommand>
{
    public SendVerificationEmailCommandValidator()
    {
        RuleFor(x => x.ConfirmEmailBaseUrl)
            .NotEmpty()
            .WithMessage("Confirmation email base URL is required")
            .Must(BeAValidUrl)
            .WithMessage("Enter a valid confirmation email base URL");

        RuleFor(x => x.ReturnUrl)
            .NotEmpty()
            .WithMessage("Return URL is required")
            .Must(BeAValidUrl!)
            .WithMessage("Enter a valid return URL");
    }

    private bool BeAValidUrl(string url) => Uri.TryCreate(url, UriKind.Absolute, out _);
}
