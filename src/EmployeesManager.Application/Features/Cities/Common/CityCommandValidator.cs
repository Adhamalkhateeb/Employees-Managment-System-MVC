using EmployeesManager.Domain.Entities.Cities;
using FluentValidation;

namespace EmployeesManager.Application.Features.Cities.Common;

public abstract class CityCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : ICityCommand
{
    protected void CommonRules()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required")
            .MaximumLength(CityConstants.CodeMaxLength);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(CityConstants.NameMaxLength);

        RuleFor(x => x.CountryId).NotEmpty().WithMessage("Country is required");
    }
}
