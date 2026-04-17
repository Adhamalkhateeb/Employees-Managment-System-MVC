using EmployeesManager.Application.Features.Designations.Common;
using FluentValidation;

namespace EmployeesManager.Application.Features.Designations.Commands.UpdateDesignation;

public sealed class UpdateDesignationCommandValidator
    : DesignationCommandValidatorBase<UpdateDesignationCommand>
{
    public UpdateDesignationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

        CommonRules();
    }
}
