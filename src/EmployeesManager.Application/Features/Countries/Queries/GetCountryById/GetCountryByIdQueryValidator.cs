using FluentValidation;

namespace EmployeesManager.Application.Features.Countries.Queries.GetCountryById;

public sealed class GetCountryByIdQueryValidator : AbstractValidator<GetCountryByIdQuery>
{
    public GetCountryByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
