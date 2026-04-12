using FluentValidation;

namespace EmployeesManager.Application.Features.Branches.Queries.GetBranchById;

public sealed class GetBranchByIdQueryValidator : AbstractValidator<GetBranchByIdQuery>
{
    public GetBranchByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
