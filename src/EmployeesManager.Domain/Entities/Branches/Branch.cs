using System.Net.Mail;
using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Employees;
using EmployeesManager.Domain.ValueObjects;

namespace EmployeesManager.Domain.Entities.Branches;

public sealed class Branch : AuditableEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Address { get; private set; } = string.Empty;
    public Phone Phone { get; private set; } = null!;
    public Email Email { get; private set; } = null!;
    public Guid? ManagerId { get; private set; }
    private readonly List<Employee> _employees = [];
    public IReadOnlyCollection<Employee> Employees => _employees.AsReadOnly();

    private Branch() { }

    private Branch(Guid id, string name, string address, Phone phone, Email email, Guid? managerId)
        : base(id)
    {
        Name = name;
        Address = address;
        Phone = phone;
        Email = email;
        ManagerId = managerId;
    }

    public static Result<Branch> Create(
        string name,
        string address,
        string phone,
        string email,
        Guid? managerId
    )
    {
        var error = Validate(name, address);
        if (error is not null)
            return error;

        var phoneResult = Phone.Create(phone);
        if (phoneResult.IsError)
            return phoneResult.Errors;

        var emailResult = Email.Create(email);
        if (emailResult.IsError)
            return emailResult.Errors;

        return new Branch(
            Guid.NewGuid(),
            name.Trim(),
            address.Trim(),
            phoneResult.Value,
            emailResult.Value,
            managerId
        );
    }

    public Result<Updated> Update(
        string name,
        string address,
        string phone,
        string email,
        Guid? managerId
    )
    {
        var error = Validate(name, address);

        if (error is not null)
            return error;

        var phoneResult = Phone.Create(phone);
        if (phoneResult.IsError)
            return phoneResult.Errors;

        var emailResult = Email.Create(email);
        if (emailResult.IsError)
            return emailResult.Errors;

        Name = name.Trim();
        Address = address.Trim();
        Phone = phoneResult.Value;
        Email = emailResult.Value;
        ManagerId = managerId;

        return Result.Updated;
    }

    public Result<Success> AssignManager(Guid managerId)
    {
        ManagerId = managerId;
        return Result.Success;
    }

    public Result<Success> RemoveManager()
    {
        ManagerId = null;
        return Result.Success;
    }

    private static Error? Validate(string name, string address)
    {
        name = name?.Trim() ?? string.Empty;
        address = address?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(name))
            return BranchErrors.NameRequired;

        if (name.Length > BranchConstants.NameMaxLength)
            return BranchErrors.NameTooLong;

        if (string.IsNullOrWhiteSpace(address))
            return BranchErrors.AddressRequired;

        if (address.Length > BranchConstants.AddressMaxLength)
            return BranchErrors.AddressTooLong;

        return null;
    }
}
