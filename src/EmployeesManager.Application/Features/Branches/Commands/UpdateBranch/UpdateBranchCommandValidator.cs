using EmployeesManager.Application.Features.Branches.Common;
using FluentValidation;

namespace EmployeesManager.Application.Features.Branches.Commands.UpdateBranch;

public sealed class UpdateBranchCommandValidator : BranchCommandValidatorBase<UpdateBranchCommand>
{
    public UpdateBranchCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        CommonRules();
    }
}
