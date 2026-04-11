using EmployeesManager.Domain.Entities.LeaveApplications;
using FluentValidation;

namespace EmployeesManager.Application.Features.LeaveApplications.Common;

public abstract class LeaveApplicationCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : ILeaveApplicationCommand
{
    protected void CommonRules()
    {
        RuleFor(x => x.EmployeeId).NotEmpty().WithMessage("Employee is required.");

        RuleFor(x => x.LeaveTypeId).NotEmpty().WithMessage("Leave type is required.");

        RuleFor(x => x.Duration).IsInEnum().WithMessage("Invalid duration.");

        RuleFor(x => x.StartDate).NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .WithMessage("End date is required.")
            .GreaterThanOrEqualTo(x => x.StartDate)
            .WithMessage("End date must be on or after start date.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(LeaveApplicationConstants.DescriptionMaxLength)
            .WithMessage(
                $"Description cannot exceed {LeaveApplicationConstants.DescriptionMaxLength} characters."
            );

        RuleFor(x => x.Attachment)
            .MaximumLength(LeaveApplicationConstants.AttachmentMaxLength)
            .WithMessage(
                $"Attachment cannot exceed {LeaveApplicationConstants.AttachmentMaxLength} characters."
            )
            .When(x => !string.IsNullOrWhiteSpace(x.Attachment));
    }
}
