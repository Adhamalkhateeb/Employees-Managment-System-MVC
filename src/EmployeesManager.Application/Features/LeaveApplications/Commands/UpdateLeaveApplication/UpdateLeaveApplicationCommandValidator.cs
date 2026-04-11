using EmployeesManager.Application.Features.LeaveApplications.Common;
using FluentValidation;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.UpdateLeaveApplication;

public sealed class UpdateLeaveApplicationCommandValidator
    : LeaveApplicationCommandValidatorBase<UpdateLeaveApplicationCommand>
{
    public UpdateLeaveApplicationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        CommonRules();
    }
}
