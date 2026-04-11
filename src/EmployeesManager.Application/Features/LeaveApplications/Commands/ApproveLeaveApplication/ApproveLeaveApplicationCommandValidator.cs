using FluentValidation;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.ApproveLeaveApplication;

public sealed class ApproveLeaveApplicationCommandValidator
    : AbstractValidator<ApproveLeaveApplicationCommand>
{
    public ApproveLeaveApplicationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Leave application id is required.");
    }
}
