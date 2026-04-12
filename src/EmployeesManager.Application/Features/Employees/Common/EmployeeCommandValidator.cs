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

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage("Last name is required")
            .MaximumLength(EmployeeConstants.LastNameMaxLength);

        RuleFor(x => x.NationalId)
            .NotEmpty()
            .WithMessage("National ID is required")
            .MaximumLength(EmployeeConstants.NationalIdMaxLength);

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

        RuleFor(x => x.HireDate)
            .NotEmpty()
            .WithMessage("Hire date is required")
            .LessThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Hire date is not valid");

        RuleFor(x => x.Address)
            .NotEmpty()
            .WithMessage("Address is required")
            .MaximumLength(EmployeeConstants.AddressMaxLength);

        RuleFor(x => x.DepartmentId)
            .NotEmpty()
            .WithMessage("Department is required")
            .NotEqual(Guid.Empty)
            .WithMessage("Department is required");

        RuleFor(x => x.BranchId)
            .Must(x => x is null || x != Guid.Empty)
            .WithMessage("Branch is invalid");
    }
}
