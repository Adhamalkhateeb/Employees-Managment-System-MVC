using EmployeesManager.Domain.Entities.Employees;
using FluentValidation;

namespace EmployeesManager.Application.Features.Employees.Common;

public abstract class EmployeeCommandValidatorBase<TCommand> : AbstractValidator<TCommand>
    where TCommand : IEmployeeCommand
{
    protected void CommonRules()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage("First name is required")
            .MaximumLength(EmployeeConstants.FirstNameMaxLength);

        RuleFor(x => x.MiddleName)
            .MaximumLength(EmployeeConstants.MiddleNameMaxLength)
            .When(x => !string.IsNullOrWhiteSpace(x.MiddleName));

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(EmployeeConstants.LastNameMaxLength);

        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .WithMessage("Phone number is required")
            .MaximumLength(EmployeeConstants.PhoneNumberMaxLength);

        RuleFor(x => x.EmailAddress)
            .NotEmpty()
            .WithMessage("Email address is required")
            .EmailAddress()
            .WithMessage("Email address format is invalid")
            .MaximumLength(EmployeeConstants.EmailAddressMaxLength);

        RuleFor(x => x.CountryId)
            .NotEmpty()
            .WithMessage("Country is required")
            .NotEqual(Guid.Empty)
            .WithMessage("Country is required");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .WithMessage("Date of birth is required")
            .LessThanOrEqualTo(DateTime.UtcNow.Date.AddYears(-EmployeeConstants.MinAge))
            .WithMessage("Date of birth is not valid");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MaximumLength(EmployeeConstants.AddressMaxLength);

        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .WithMessage("Department is required")
            .NotEqual(Guid.Empty)
            .WithMessage("Department is required");

        RuleFor(x => x.DesignationId)
            .NotEmpty()
            .WithMessage("Designation is required")
            .NotEqual(Guid.Empty)
            .WithMessage("Designation is required");
    }
}
