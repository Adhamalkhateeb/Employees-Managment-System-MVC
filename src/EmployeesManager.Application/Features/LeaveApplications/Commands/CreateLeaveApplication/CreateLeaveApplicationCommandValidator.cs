using EmployeesManager.Application.Features.LeaveApplications.Common;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.CreateLeaveApplication;

public sealed class CreateLeaveApplicationCommandValidator
    : LeaveApplicationCommandValidatorBase<CreateLeaveApplicationCommand>
{
    public CreateLeaveApplicationCommandValidator()
    {
        CommonRules();
    }
}
