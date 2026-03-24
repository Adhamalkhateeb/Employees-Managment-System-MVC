using FluentValidation;

namespace EmployeesManager.Application.Features.Designations.Queries.GetDesignationById;

public sealed class GetDesignationByIdQueryValidator : AbstractValidator<GetDesignationByIdQuery>
{
    public GetDesignationByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
