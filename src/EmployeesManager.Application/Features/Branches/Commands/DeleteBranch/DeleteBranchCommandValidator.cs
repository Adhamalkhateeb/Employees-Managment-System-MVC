using FluentValidation;

namespace EmployeesManager.Application.Features.Branches.Commands.DeleteBranch;

public sealed class DeleteBranchCommandValidator : AbstractValidator<DeleteBranchCommand>
{
    public DeleteBranchCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
