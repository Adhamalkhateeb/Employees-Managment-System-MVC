using EmployeesManager.Application.Features.Branches.Common;

namespace EmployeesManager.Application.Features.Branches.Commands.CreateBranch;

public sealed class CreateBranchCommandValidator : BranchCommandValidatorBase<CreateBranchCommand>
{
    public CreateBranchCommandValidator()
    {
        CommonRules();
    }
}
