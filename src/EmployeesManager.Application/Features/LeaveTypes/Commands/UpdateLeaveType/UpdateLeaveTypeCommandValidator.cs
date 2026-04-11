using EmployeesManager.Application.Features.LeaveTypes.Common;
using FluentValidation;

namespace EmployeesManager.Application.Features.LeaveTypes.Commands.UpdateLeaveType;

public sealed class UpdateLeaveTypeCommandValidator
    : LeaveTypeCommandValidatorBase<UpdateLeaveTypeCommand>
{
    public UpdateLeaveTypeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

        CommonRules();
    }
}
