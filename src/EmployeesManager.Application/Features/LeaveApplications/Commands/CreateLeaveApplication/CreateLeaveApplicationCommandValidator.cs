using EmployeesManager.Application.Features.LeaveApplications.Common;
using FluentValidation;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.CreateLeaveApplication;

public sealed class CreateLeaveApplicationCommandValidator
    : LeaveApplicationCommandValidatorBase<CreateLeaveApplicationCommand>
{
    public CreateLeaveApplicationCommandValidator()
    {
        CommonRules();

        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTimeOffset.UtcNow.Date)
            .WithMessage("Start date cannot be in the past.");
    }
}
