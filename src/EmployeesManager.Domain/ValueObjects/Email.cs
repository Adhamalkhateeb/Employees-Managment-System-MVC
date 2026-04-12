using System.Net.Mail;
using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    public string Value { get; }

    private Email(string value)
    {
        Value = value;
    }

    public static Result<Email> Create(string? email)
    {
        email = email?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(email))
            return EmailErrors.Required;

        if (email.Length > EmailConstants.MaxLength)
            return EmailErrors.TooLong;

        if (!IsValidEmail(email))
            return EmailErrors.Invalid;

        return new Email(email);
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var _ = new MailAddress(email);
            return true;
        }
        catch
        {
            return false;
        }
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value.ToLower();
    }
}

public static class EmailConstants
{
    public const int MaxLength = 256;
}

public static class EmailErrors
{
    public static readonly Error Required = Error.Validation(
        "Email.Required",
        "Email is required.",
        "Email"
    );
    public static readonly Error Invalid = Error.Validation(
        "Email.Invalid",
        "Email format is invalid.",
        "Email"
    );
    public static readonly Error TooLong = Error.Validation(
        "Email.TooLong",
        "Email is too long.",
        "Email"
    );
}
