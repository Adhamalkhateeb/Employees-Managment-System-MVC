using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.Employees;

public static class EmployeeErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Employee.NotFound", $"Employee '{id}' was not found.");

    public static readonly Error FirstNameRequired = Error.Validation(
        "Employee.FirstName.Required",
        "First name is required."
    );

    public static readonly Error FirstNameTooLong = Error.Validation(
        "Employee.FirstName.TooLong",
        "First name is too long."
    );

    public static readonly Error MiddleNameTooLong = Error.Validation(
        "Employee.MiddleName.TooLong",
        "Middle name is too long."
    );

    public static readonly Error LastNameRequired = Error.Validation(
        "Employee.LastName.Required",
        "Last name is required."
    );

    public static readonly Error LastNameTooLong = Error.Validation(
        "Employee.LastName.TooLong",
        "Last name is too long."
    );

    public static readonly Error PhoneNumberRequired = Error.Validation(
        "Employee.PhoneNumber.Required",
        "Phone number is required."
    );

    public static readonly Error PhoneNumberInvalid = Error.Validation(
        "Employee.PhoneNumber.Invalid",
        "Phone number is invalid."
    );

    public static readonly Error PhoneNumberAlreadyExists = Error.Conflict(
        "Employee.PhoneNumber.AlreadyExists",
        "Phone number already exists."
    );

    public static readonly Error EmailAddressRequired = Error.Validation(
        "Employee.EmailAddress.Required",
        "Email address is required."
    );

    public static readonly Error EmailAddressTooLong = Error.Validation(
        "Employee.EmailAddress.TooLong",
        "Email address is too long."
    );

    public static readonly Error EmailAddressInvalid = Error.Validation(
        "Employee.EmailAddress.Invalid",
        "Email address format is invalid."
    );

    public static readonly Error EmailAddressAlreadyExists = Error.Conflict(
        "Employee.EmailAddress.AlreadyExists",
        "Email address already exists."
    );

    public static readonly Error CountryIdRequired = Error.Validation(
        "Employee.CountryId.Required",
        "Country is required."
    );

    public static readonly Error CountryNotFound = Error.NotFound(
        "Employee.Country.NotFound",
        "Selected country was not found."
    );

    public static readonly Error DateOfBirthInvalid = Error.Validation(
        "Employee.DateOfBirth.Invalid",
        "Date of birth is invalid."
    );

    public static readonly Error AddressRequired = Error.Validation(
        "Employee.Address.Required",
        "Address is required."
    );

    public static readonly Error AddressTooLong = Error.Validation(
        "Employee.Address.TooLong",
        "Address is too long."
    );

    public static readonly Error DepartmentIdRequired = Error.Validation(
        "Employee.DepartmentId.Required",
        "Department is required."
    );

    public static readonly Error DepartmentNotFound = Error.NotFound(
        "Employee.Department.NotFound",
        "Selected department was not found."
    );

    public static readonly Error DesignationIdRequired = Error.Validation(
        "Employee.DesignationId.Required",
        "Designation is required."
    );

    public static readonly Error DesignationNotFound = Error.NotFound(
        "Employee.Designation.NotFound",
        "Selected designation was not found."
    );
}
