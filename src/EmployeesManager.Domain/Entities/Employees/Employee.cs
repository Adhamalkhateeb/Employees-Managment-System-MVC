using System.Text.RegularExpressions;
using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Countries;
using EmployeesManager.Domain.Entities.Departments;
using EmployeesManager.Domain.Entities.Designations;

namespace EmployeesManager.Domain.Entities.Employees;

public sealed class Employee : AuditableEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string? MiddleName { get; private set; }
    public string LastName { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string EmailAddress { get; private set; } = string.Empty;
    public DateTime DateOfBirth { get; private set; }
    public string Address { get; private set; } = string.Empty;
    public Guid CountryId { get; private set; }
    public Country Country { get; private set; } = null!;
    public Guid DepartmentId { get; private set; }
    public Department Department { get; private set; } = null!;
    public Guid DesignationId { get; private set; }
    public Designation Designation { get; private set; } = null!;

    private Employee() { }

    private Employee(
        Guid id,
        string firstName,
        string? middleName,
        string lastName,
        string phoneNumber,
        string emailAddress,
        DateTime dateOfBirth,
        string address,
        Guid countryId,
        Guid departmentId,
        Guid designationId
    )
        : base(id)
    {
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        PhoneNumber = phoneNumber;
        EmailAddress = emailAddress;
        DateOfBirth = dateOfBirth;
        Address = address;
        CountryId = countryId;
        DepartmentId = departmentId;
        DesignationId = designationId;
    }

    public static Result<Employee> Create(
        string firstName,
        string? middleName,
        string lastName,
        string phoneNumber,
        string emailAddress,
        DateTime dateOfBirth,
        string address,
        Guid countryId,
        Guid departmentId,
        Guid designationId
    )
    {
        var validationError = Validate(
            firstName,
            middleName,
            lastName,
            phoneNumber,
            emailAddress,
            dateOfBirth,
            address,
            countryId,
            departmentId,
            designationId
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
            dateOfBirth,
            address,
            countryId,
            departmentId,
            designationId
        );
    }

    public Result<Updated> Update(
        string firstName,
        string? middleName,
        string lastName,
        string phoneNumber,
        string emailAddress,
        DateTime dateOfBirth,
        string address,
        Guid countryId,
        Guid departmentId,
        Guid designationId
    )
    {
        var validationError = Validate(
            firstName,
            middleName,
            lastName,
            phoneNumber,
            emailAddress,
            dateOfBirth,
            address,
            countryId,
            departmentId,
            designationId
        );

        if (validationError != null)
            return validationError;

        FirstName = firstName.Trim();
        MiddleName = NormalizeOptional(middleName);
        LastName = lastName.Trim();
        PhoneNumber = phoneNumber.Trim();
        EmailAddress = emailAddress.Trim();
        DateOfBirth = dateOfBirth.Date;
        Address = address.Trim();
        CountryId = countryId;
        DepartmentId = departmentId;
        DesignationId = designationId;

        return Result.Updated;
    }

    private static Error? Validate(
        string firstName,
        string? middleName,
        string lastName,
        string phoneNumber,
        string emailAddress,
        DateTime dateOfBirth,
        string address,
        Guid countryId,
        Guid departmentId,
        Guid designationId
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

        if (countryId == Guid.Empty)
            return EmployeeErrors.CountryIdRequired;

        if (
            dateOfBirth == default
            || dateOfBirth.Date > DateTime.UtcNow.AddYears(-EmployeeConstants.MinAge).Date
        )
            return EmployeeErrors.DateOfBirthInvalid;

        if (string.IsNullOrWhiteSpace(address))
            return EmployeeErrors.AddressRequired;
        else if (address.Trim().Length > EmployeeConstants.AddressMaxLength)
            return EmployeeErrors.AddressTooLong;

        if (departmentId == Guid.Empty)
            return EmployeeErrors.DepartmentIdRequired;

        if (designationId == Guid.Empty)
            return EmployeeErrors.DesignationIdRequired;

        return null;
    }

    private static string? NormalizeOptional(string? value) =>
        string.IsNullOrWhiteSpace(value) ? null : value.Trim();

    private static bool IsValidEmail(string value) =>
        Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$");
}
