using EmployeesManager.Application.Features.Countries.Common;
using FluentValidation;

namespace EmployeesManager.Application.Features.Countries.Commands.UpdateCountry;

public sealed class UpdateCountryCommandValidator
    : CountryCommandValidatorBase<UpdateCountryCommand>
{
    public UpdateCountryCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");

        CommonRules();
    }
}
