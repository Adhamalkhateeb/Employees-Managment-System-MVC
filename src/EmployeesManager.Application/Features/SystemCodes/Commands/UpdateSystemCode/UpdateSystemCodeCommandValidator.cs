using EmployeesManager.Application.Features.SystemCodes.Common;
using FluentValidation;

namespace EmployeesManager.Application.Features.SystemCodes.Commands.UpdateSystemCode;

public sealed class UpdateSystemCodeCommandValidator
    : SystemCodeCommandValidatorBase<UpdateSystemCodeCommand>
{
    public UpdateSystemCodeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

        CommonRules();
    }
}
