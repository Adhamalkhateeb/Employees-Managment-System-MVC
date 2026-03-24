using System.Net.Mail;
using System.Text.RegularExpressions;
using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.Employees;

public sealed class Employee : AuditableEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string? MiddleName { get; private set; }
    public string LastName { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string EmailAddress { get; private set; } = string.Empty;
    public string Country { get; private set; } = string.Empty;
    public DateTime DateOfBirth { get; private set; }
    public string Address { get; private set; } = string.Empty;
    public string Department { get; private set; } = string.Empty;
    public string Designation { get; private set; } = string.Empty;

    private Employee() { }

    private Employee(
        Guid id,
        string firstName,
        string? middleName,
        string lastName,
        string phoneNumber,
        string emailAddress,
        string country,
        DateTime dateOfBirth,
        string address,
        string department,
        string designation
    )
        : base(id)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        EmailAddress = emailAddress;
        Country = country;
        DateOfBirth = dateOfBirth;
        Address = address;
        Department = department;
        Designation = designation;
    }

    public static Result<Employee> Create(
        string firstName,
        string? middleName,
        string lastName,
        string phoneNumber,
        string emailAddress,
        string country,
        DateTime dateOfBirth,
        string address,
        string department,
        string designation
    )
    {
        var validationError = Validate(
            firstName,
            middleName,
            lastName,
            phoneNumber,
            emailAddress,
            country,
            dateOfBirth,
            address,
            department,
            designation
        );

        if (validationError != null)
            return validationError;

        return new Employee(
            Guid.NewGuid(),
            firstName,
            middleName,
            lastName,
            phoneNumber,
            emailAddress,
            country,
            dateOfBirth,
            address,
            department,
            designation
        );
    }

    public Result<Updated> Update(
        string firstName,
        string? middleName,
        string lastName,
        string phoneNumber,
        string emailAddress,
        string country,
        DateTime dateOfBirth,
        string address,
        string department,
        string designation
    )
    {
        var validationError = Validate(
            firstName,
            middleName,
            lastName,
            phoneNumber,
            emailAddress,
            country,
            dateOfBirth,
            address,
            department,
            designation
        );

        if (validationError != null)
            return validationError;

        FirstName = firstName.Trim();
        MiddleName = NormalizeOptional(middleName);
        LastName = lastName.Trim();
        PhoneNumber = phoneNumber.Trim();
        EmailAddress = emailAddress.Trim();
        Country = country.Trim();
        DateOfBirth = dateOfBirth.Date;
        Address = address.Trim();
        Department = department.Trim();
        Designation = designation.Trim();

        return Result.Updated;
    }

    private static Error? Validate(
        string firstName,
        string? middleName,
        string lastName,
        string phoneNumber,
        string emailAddress,
        string country,
        DateTime dateOfBirth,
        string address,
        string department,
        string designation
    )
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return EmployeeErrors.FirstNameRequired;
        else if (firstName.Trim().Length > EmployeeConstants.FirstNameMaxLength)
            return EmployeeErrors.FirstNameTooLong;

        if (
            !string.IsNullOrWhiteSpace(middleName)
            && middleName.Trim().Length > EmployeeConstants.MiddleNameMaxLength
        )
            return EmployeeErrors.MiddleNameTooLong;

        if (string.IsNullOrWhiteSpace(lastName))
            return EmployeeErrors.LastNameRequired;
        else if (lastName.Trim().Length > EmployeeConstants.LastNameMaxLength)
            return EmployeeErrors.LastNameTooLong;

        if (string.IsNullOrWhiteSpace(phoneNumber))
            return EmployeeErrors.PhoneNumberRequired;
        else if (phoneNumber.Trim().Length > EmployeeConstants.PhoneNumberMaxLength)
            return EmployeeErrors.PhoneNumberInvalid;

        if (string.IsNullOrWhiteSpace(emailAddress))
            return EmployeeErrors.EmailAddressRequired;
        else if (emailAddress.Trim().Length > EmployeeConstants.EmailAddressMaxLength)
            return EmployeeErrors.EmailAddressTooLong;
        else if (!IsValidEmail(emailAddress.Trim()))
            return EmployeeErrors.EmailAddressInvalid;

        if (string.IsNullOrWhiteSpace(country))
            return EmployeeErrors.CountryRequired;
        else if (country.Trim().Length > EmployeeConstants.CountryMaxLength)
            return EmployeeErrors.CountryTooLong;

        if (
            dateOfBirth == default
            || dateOfBirth.Date > DateTime.UtcNow.AddYears(-EmployeeConstants.MinAge).Date
        )
            return EmployeeErrors.DateOfBirthInvalid;

        if (string.IsNullOrWhiteSpace(address))
            return EmployeeErrors.AddressRequired;
        else if (address.Trim().Length > EmployeeConstants.AddressMaxLength)
            return EmployeeErrors.AddressTooLong;

        if (string.IsNullOrWhiteSpace(department))
            return EmployeeErrors.DepartmentRequired;
        else if (department.Trim().Length > EmployeeConstants.DepartmentMaxLength)
            return EmployeeErrors.DepartmentTooLong;

        if (string.IsNullOrWhiteSpace(designation))
            return EmployeeErrors.DesignationRequired;
        else if (designation.Trim().Length > EmployeeConstants.DesignationMaxLength)
            return EmployeeErrors.DesignationTooLong;

        return null;
    }

    private static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static bool IsValidEmail(string value) =>
        Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$");
}
