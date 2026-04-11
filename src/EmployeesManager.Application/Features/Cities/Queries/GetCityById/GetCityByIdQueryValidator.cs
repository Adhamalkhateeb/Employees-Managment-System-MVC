using FluentValidation;

namespace EmployeesManager.Application.Features.Cities.Queries.GetCityById;

public sealed class GetCityByIdQueryValidator : AbstractValidator<GetCityByIdQuery>
{
    public GetCityByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}
