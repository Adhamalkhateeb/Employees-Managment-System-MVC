using EmployeesManager.Application.Features.Cities.Common;

namespace EmployeesManager.Application.Features.Cities.Commands.CreateCity;

public sealed class CreateCityCommandValidator : CityCommandValidatorBase<CreateCityCommand>
{
    public CreateCityCommandValidator()
    {
        CommonRules();
    }
}
