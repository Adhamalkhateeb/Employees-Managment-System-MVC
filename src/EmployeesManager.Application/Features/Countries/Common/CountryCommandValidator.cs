using EmployeesManager.Domain.Entities.Countries;
using FluentValidation;

namespace EmployeesManager.Application.Features.Countries.Common;

public abstract class CountryCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : ICountryCommand
{
    protected void CommonRules()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .WithMessage("Code is required")
            .MaximumLength(CountryConstants.CodeMaxLength);

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(CountryConstants.NameMaxLength);
    }
}
