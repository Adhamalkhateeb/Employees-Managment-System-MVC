using FluentValidation;

namespace EmployeesManager.Application.Features.Identity.Email.Commands.SendChangeEmailLink;

public sealed class SendChangeEmailLinkCommandValidator
    : AbstractValidator<SendChangeEmailLinkCommand>
{
    public SendChangeEmailLinkCommandValidator()
    {
        RuleFor(x => x.NewEmail)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("A valid email address is required.");

        RuleFor(x => x.ConfirmEmailChangeBaseUrl)
            .NotEmpty()
            .Must(uri => Uri.IsWellFormedUriString(uri, UriKind.Absolute))
            .WithMessage("A valid URL is required.");
    }
}
