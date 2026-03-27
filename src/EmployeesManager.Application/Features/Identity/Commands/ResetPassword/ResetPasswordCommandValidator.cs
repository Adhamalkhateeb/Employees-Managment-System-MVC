// using FluentValidation;

// namespace EmployeesManager.Application.Features.Identity.Commands.ResetPassword;

// public sealed class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
// {
//     public ResetPasswordCommandValidator()
//     {
//         RuleFor(x => x.Email)
//             .NotEmpty()
//             .WithMessage("Email is required")
//             .EmailAddress()
//             .WithMessage("Enter a valid email address");

//         RuleFor(x => x.Code).NotEmpty().WithMessage("Reset token is required");

//         RuleFor(x => x.Password)
//             .NotEmpty()
//             .WithMessage("Password is required")
//             .MinimumLength(8)
//             .WithMessage("Password must be at least 8 characters");
//     }
// }
