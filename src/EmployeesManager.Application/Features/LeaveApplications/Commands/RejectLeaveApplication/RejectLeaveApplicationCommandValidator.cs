using EmployeesManager.Domain.Entities.LeaveApplications;
using FluentValidation;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.RejectLeaveApplication;

public sealed class RejectLeaveApplicationCommandValidator
    : AbstractValidator<RejectLeaveApplicationCommand>
{
    public RejectLeaveApplicationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Leave application id is required.");

        RuleFor(x => x.RejectionReason)
            .NotEmpty()
            .WithMessage("Rejection reason is required.")
            .MaximumLength(LeaveApplicationConstants.RejectionReasonMaxLength)
            .WithMessage(
                $"Rejection reason cannot exceed {LeaveApplicationConstants.RejectionReasonMaxLength} characters."
            );
    }
}
