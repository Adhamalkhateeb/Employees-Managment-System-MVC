using FluentValidation;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.DeleteLeaveApplication;

public sealed class DeleteLeaveApplicationCommandValidator : AbstractValidator<DeleteLeaveApplicationCommand>
{
    public DeleteLeaveApplicationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
