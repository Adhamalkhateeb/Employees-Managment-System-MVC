using EmployeesManager.Application.Features.Designations.Common;

namespace EmployeesManager.Application.Features.Designations.Commands.CreateDesignation;

public sealed class CreateDesignationCommandValidator
    : DesignationCommandValidatorBase<CreateDesignationCommand>
{
    public CreateDesignationCommandValidator()
    {
        CommonRules();
    }
}
