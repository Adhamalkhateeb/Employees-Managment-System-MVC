using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.Employees;

public static class EmployeeErrors
{
    public static Error NotFound(Guid id) =>
        Error.NotFound("Employee.NotFound", $"Employee '{id}' was not found.");

    public static readonly Error FirstNameRequired = Error.Validation(
        "Employee.FirstName.Required",
        "First name is required.",
        "FirstName"
    );

    public static readonly Error FirstNameTooLong = Error.Validation(
        "Employee.FirstName.TooLong",
        "First name is too long.",
        "FirstName"
    );

    public static readonly Error NationalIdRequired = Error.Validation(
        "Employee.NationalId.Required",
        "National ID is required.",
        "NationalId"
    );

    public static readonly Error NationalIdTooLong = Error.Validation(
        "Employee.NationalId.TooLong",
        "National ID is too long.",
        "NationalId"
    );

    public static readonly Error NationalIdAlreadyExists = Error.Conflict(
        "Employee.NationalId.AlreadyExists",
        "National ID already exists.",
        "NationalId"
    );

    public static readonly Error LastNameRequired = Error.Validation(
        "Employee.LastName.Required",
        "Last name is required.",
        "LastName"
    );

    public static readonly Error LastNameTooLong = Error.Validation(
        "Employee.LastName.TooLong",
        "Last name is too long.",
        "LastName"
    );

    public static readonly Error PhoneNumberRequired = Error.Validation(
        "Employee.PhoneNumber.Required",
        "Phone number is required.",
        "PhoneNumber"
    );

    public static readonly Error PhoneNumberInvalid = Error.Validation(
        "Employee.PhoneNumber.Invalid",
        "Phone number is invalid.",
        "PhoneNumber"
    );

    public static readonly Error PhoneNumberAlreadyExists = Error.Conflict(
        "Employee.PhoneNumber.AlreadyExists",
        "Phone number already exists.",
        "PhoneNumber"
    );

    public static readonly Error EmailAddressRequired = Error.Validation(
        "Employee.EmailAddress.Required",
        "Email address is required.",
        "EmailAddress"
    );

    public static readonly Error EmailAddressTooLong = Error.Validation(
        "Employee.EmailAddress.TooLong",
        "Email address is too long.",
        "EmailAddress"
    );

    public static readonly Error EmailAddressInvalid = Error.Validation(
        "Employee.EmailAddress.Invalid",
        "Email address format is invalid.",
        "EmailAddress"
    );

    public static readonly Error EmailAddressAlreadyExists = Error.Conflict(
        "Employee.EmailAddress.AlreadyExists",
        "Email address already exists.",
        "EmailAddress"
    );

    public static readonly Error HireDateInvalid = Error.Validation(
        "Employee.HireDate.Invalid",
        "Hire date is invalid.",
        "HireDate"
    );

    public static readonly Error AddressRequired = Error.Validation(
        "Employee.Address.Required",
        "Address is required.",
        "Address"
    );

    public static readonly Error AddressTooLong = Error.Validation(
        "Employee.Address.TooLong",
        "Address is too long.",
        "Address"
    );

    public static readonly Error DepartmentIdRequired = Error.Validation(
        "Employee.DepartmentId.Required",
        "Department is required.",
        "DepartmentId"
    );

    public static readonly Error DepartmentNotFound = Error.NotFound(
        "Employee.Department.NotFound",
        "Selected department was not found."
    );

    public static readonly Error BranchNotFound = Error.NotFound(
        "Employee.Branch.NotFound",
        "Selected branch was not found."
    );
}
