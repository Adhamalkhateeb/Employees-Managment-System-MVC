using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.Entities.Banks;

public sealed class Bank : AuditableEntity
{
    public string Code { get; private set; } = default!;
    public string Name { get; private set; } = default!;
    public string AccountNo { get; private set; } = default!;

    private Bank() { }

    private Bank(Guid id)
        : base(id) { }

    public static Result<Bank> Create(string code, string name, string accountNo)
    {
        var validationError = Validate(code, name, accountNo);

        if (validationError is not null)
            return validationError;

        return new Bank(Guid.NewGuid())
        {
            Code = code.Trim(),
            Name = name.Trim(),
            AccountNo = accountNo.Trim(),
        };
    }

    public Result<Updated> Update(string code, string name, string accountNo)
    {
        var validationError = Validate(code, name, accountNo);

        if (validationError is not null)
            return validationError;

        Code = code.Trim();
        Name = name.Trim();
        AccountNo = accountNo.Trim();

        return Result.Updated;
    }

    private static Error? Validate(string code, string name, string accountNo)
    {
        if (string.IsNullOrWhiteSpace(code))
            return BankErrors.CodeRequired;
        if (code.Trim().Length > BankConstants.CodeMaxLength)
            return BankErrors.CodeTooLong;

        if (string.IsNullOrWhiteSpace(name))
            return BankErrors.NameRequired;
        if (name.Trim().Length > BankConstants.NameMaxLength)
            return BankErrors.NameTooLong;

        if (string.IsNullOrWhiteSpace(accountNo))
            return BankErrors.AccountNoRequired;
        if (accountNo.Trim().Length > BankConstants.AccountNoMaxLength)
            return BankErrors.AccountNoTooLong;

        return null;
    }
}
