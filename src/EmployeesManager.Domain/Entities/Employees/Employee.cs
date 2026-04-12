using System.Text.RegularExpressions;
using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Branches;
using EmployeesManager.Domain.Entities.Departments;

namespace EmployeesManager.Domain.Entities.Employees;

public sealed class Employee : AuditableEntity
{
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string NationalId { get; private set; } = string.Empty;
    public string PhoneNumber { get; private set; } = string.Empty;
    public string EmailAddress { get; private set; } = string.Empty;
    public DateTime HireDate { get; private set; }
    public string Address { get; private set; } = string.Empty;
    public Guid DepartmentId { get; private set; }
    public Department Department { get; private set; } = null!;

    public Guid? BranchId { get; private set; }
    public Branch? Branch { get; private set; }

    private Employee() { }

    private Employee(
        Guid id,
        string firstName,
        string lastName,
        string nationalId,
        string phoneNumber,
        string emailAddress,
        DateTime hireDate,
        string address,
        Guid departmentId,
        Guid? branchId
    )
        : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        NationalId = nationalId;
        PhoneNumber = phoneNumber;
        EmailAddress = emailAddress;
        HireDate = hireDate == default ? DateTime.UtcNow : hireDate;
        Address = address;
        DepartmentId = departmentId;
        BranchId = branchId;
    }

    public static Result<Employee> Create(
        string firstName,
        string lastName,
        string nationalId,
        string phoneNumber,
        string emailAddress,
        DateTime hireDate,
        string address,
        Guid departmentId,
        Guid? branchId
    )
    {
        var validationError = Validate(
            firstName,
            lastName,
            nationalId,
            phoneNumber,
            emailAddress,
            hireDate,
            address,
            departmentId
        );

        if (validationError != null)
            return validationError;

        return new Employee(
            Guid.NewGuid(),
            firstName.Trim(),
            lastName.Trim(),
            nationalId.Trim(),
            phoneNumber.Trim(),
            emailAddress.Trim(),
            hireDate == default ? DateTime.UtcNow.Date : hireDate.Date,
            address.Trim(),
            departmentId,
            branchId
        );
    }

    public Result<Updated> Update(
        string firstName,
        string lastName,
        string nationalId,
        string phoneNumber,
        string emailAddress,
        DateTime hireDate,
        string address,
        Guid departmentId,
        Guid? branchId
    )
    {
        var validationError = Validate(
            firstName,
            lastName,
            nationalId,
            phoneNumber,
            emailAddress,
            hireDate,
            address,
            departmentId
        );

        if (validationError != null)
            return validationError;

        FirstName = firstName.Trim();
        LastName = lastName.Trim();
        NationalId = nationalId.Trim();
        PhoneNumber = phoneNumber.Trim();
        EmailAddress = emailAddress.Trim();
        HireDate = hireDate == default ? DateTime.UtcNow.Date : hireDate.Date;
        Address = address.Trim();
        DepartmentId = departmentId;
        BranchId = branchId;

        return Result.Updated;
    }

    private static Error? Validate(
        string firstName,
        string lastName,
        string nationalId,
        string phoneNumber,
        string emailAddress,
        DateTime hireDate,
        string address,
        Guid departmentId
    )
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return EmployeeErrors.FirstNameRequired;
        else if (firstName.Trim().Length > EmployeeConstants.FirstNameMaxLength)
            return EmployeeErrors.FirstNameTooLong;

        if (string.IsNullOrWhiteSpace(lastName))
            return EmployeeErrors.LastNameRequired;
        else if (lastName.Trim().Length > EmployeeConstants.LastNameMaxLength)
            return EmployeeErrors.LastNameTooLong;

        if (string.IsNullOrWhiteSpace(nationalId))
            return EmployeeErrors.NationalIdRequired;
        else if (nationalId.Trim().Length > EmployeeConstants.NationalIdMaxLength)
            return EmployeeErrors.NationalIdTooLong;

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

        if (hireDate == default || hireDate.Date > DateTime.UtcNow.Date)
            return EmployeeErrors.HireDateInvalid;

        if (string.IsNullOrWhiteSpace(address))
            return EmployeeErrors.AddressRequired;
        else if (address.Trim().Length > EmployeeConstants.AddressMaxLength)
            return EmployeeErrors.AddressTooLong;

        if (departmentId == Guid.Empty)
            return EmployeeErrors.DepartmentIdRequired;

        return null;
    }

    private static bool IsValidEmail(string value) =>
        Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$");
}
