using EmployeesManager.Application.Features.Countries.Common;

namespace EmployeesManager.Application.Features.Countries.Commands.CreateCountry;

public sealed class CreateCountryCommandValidator
    : CountryCommandValidatorBase<CreateCountryCommand>
{
    public CreateCountryCommandValidator()
    {
        CommonRules();
    }
}
