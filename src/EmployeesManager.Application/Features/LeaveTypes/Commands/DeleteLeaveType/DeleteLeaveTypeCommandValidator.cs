using FluentValidation;

namespace EmployeesManager.Application.Features.LeaveTypes.Commands.DeleteLeaveType;

public sealed class DeleteLeaveTypeCommandValidator : AbstractValidator<DeleteLeaveTypeCommand>
{
    public DeleteLeaveTypeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
