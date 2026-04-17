using EmployeesManager.Domain.Entities.SystemCodeDetails;
using FluentValidation;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Common;

public abstract class SystemCodeDetailCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : ISystemCodeDetailCommand
{
    protected void CommonRules()
    {
        RuleFor(x => x.SystemCodeId).NotEmpty().WithMessage("System code is required");

        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required")
            .MaximumLength(SystemCodeDetailConstants.CodeMaxLength)
            .WithMessage(
                $"Code must not exceed {SystemCodeDetailConstants.CodeMaxLength} characters."
            );

        RuleFor(x => x.Description)
            .MaximumLength(SystemCodeDetailConstants.DescriptionMaxLength)
            .WithMessage(
                $"Description must not exceed {SystemCodeDetailConstants.DescriptionMaxLength} characters."
            );

        RuleFor(x => x.OrderNo)
            .GreaterThanOrEqualTo(0)
            .When(x => x.OrderNo.HasValue)
            .WithMessage("Order number must be greater than or equal to zero");
    }
}
