using FluentValidation;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.CancelLeaveApplication;

public sealed class CancelLeaveApplicationCommandValidator
    : AbstractValidator<CancelLeaveApplicationCommand>
{
    public CancelLeaveApplicationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Leave application id is required.");
    }
}
