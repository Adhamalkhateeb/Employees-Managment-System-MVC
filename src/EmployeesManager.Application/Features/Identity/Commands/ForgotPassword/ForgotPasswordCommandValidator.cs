// using FluentValidation;

// namespace EmployeesManager.Application.Features.Identity.Commands.ForgotPassword;

// public sealed class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
// {
//     public ForgotPasswordCommandValidator()
//     {
//         RuleFor(x => x.Email)
//             .NotEmpty()
//             .WithMessage("Email is required")
//             .EmailAddress()
//             .WithMessage("Enter a valid email address");

//         RuleFor(x => x.ResetPasswordBaseUrl)
//             .NotEmpty()
//             .WithMessage("Reset password callback URL is required");
//     }
// }
