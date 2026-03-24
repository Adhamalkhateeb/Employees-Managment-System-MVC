using EmployeesManager.Application.Features.LeaveTypes.Common;

namespace EmployeesManager.Application.Features.LeaveTypes.Commands.CreateLeaveType;

public sealed class CreateLeaveTypeCommandValidator
    : LeaveTypeCommandValidatorBase<CreateLeaveTypeCommand>
{
    public CreateLeaveTypeCommandValidator()
    {
        CommonRules();
    }
}
