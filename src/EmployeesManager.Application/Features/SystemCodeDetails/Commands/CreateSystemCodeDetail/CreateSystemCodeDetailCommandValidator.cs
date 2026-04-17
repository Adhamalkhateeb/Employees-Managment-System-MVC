using EmployeesManager.Application.Features.SystemCodeDetails.Common;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Commands.CreateSystemCodeDetail;

public sealed class CreateSystemCodeDetailCommandValidator
    : SystemCodeDetailCommandValidatorBase<CreateSystemCodeDetailCommand>
{
    public CreateSystemCodeDetailCommandValidator()
    {
        CommonRules();
    }
}
