using EmployeesManager.Domain.Entities.LeaveTypes;
using FluentValidation;

namespace EmployeesManager.Application.Features.LeaveTypes.Common;

public abstract class LeaveTypeCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : ILeaveTypeCommand
{
    protected void CommonRules()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(LeaveTypeConstants.NameMaxLength);

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required")
            .MaximumLength(LeaveTypeConstants.CodeMaxLength);
    }
}
