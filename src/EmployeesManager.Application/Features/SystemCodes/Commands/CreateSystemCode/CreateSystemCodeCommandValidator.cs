using EmployeesManager.Application.Features.SystemCodes.Common;

namespace EmployeesManager.Application.Features.SystemCodes.Commands.CreateSystemCode;

public sealed class CreateSystemCodeCommandValidator
    : SystemCodeCommandValidatorBase<CreateSystemCodeCommand>
{
    public CreateSystemCodeCommandValidator()
    {
        CommonRules();
    }
}
