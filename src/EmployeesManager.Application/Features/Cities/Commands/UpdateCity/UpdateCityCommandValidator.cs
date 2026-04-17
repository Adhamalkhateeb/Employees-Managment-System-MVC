using EmployeesManager.Application.Features.Cities.Common;
using FluentValidation;

namespace EmployeesManager.Application.Features.Cities.Commands.UpdateCity;

public sealed class UpdateCityCommandValidator : CityCommandValidatorBase<UpdateCityCommand>
{
    public UpdateCityCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

        CommonRules();
    }
}
