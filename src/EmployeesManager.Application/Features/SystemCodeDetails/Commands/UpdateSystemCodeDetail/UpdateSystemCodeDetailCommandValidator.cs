using EmployeesManager.Application.Features.SystemCodeDetails.Common;
using FluentValidation;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Commands.UpdateSystemCodeDetail;

public sealed class UpdateSystemCodeDetailCommandValidator
    : SystemCodeDetailCommandValidatorBase<UpdateSystemCodeDetailCommand>
{
    public UpdateSystemCodeDetailCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

        CommonRules();
    }
}
