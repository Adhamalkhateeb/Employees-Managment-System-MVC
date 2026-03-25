using EmployeesManager.Domain.Entities.LeaveApplications;
using FluentValidation;

namespace EmployeesManager.Application.Features.LeaveApplications.Common;

public abstract class LeaveApplicationCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : ILeaveApplicationCommand
{
    protected void CommonRules()
    {
        RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Employee is required");

        RuleFor(x => x.LeaveTypeId).NotEmpty().WithMessage("Leave type is required");

        RuleFor(x => x.DurationId).NotEmpty().WithMessage("Duration is required");

        RuleFor(x => x.StatusId).NotEmpty().WithMessage("Status is required");

        RuleFor(x => x.StartDate)
            .NotEqual(DateTimeOffset.MinValue)
            .WithMessage("Start date is required");

        RuleFor(x => x.EndDate)
            .NotEqual(DateTimeOffset.MinValue)
            .WithMessage("End date is required")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be greater than or equal to start date");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(LeaveApplicationConstants.DescriptionMaxLength);

        RuleFor(x => x.Attachment)
            .MaximumLength(LeaveApplicationConstants.AttachmentMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.Attachment));
    }
}
