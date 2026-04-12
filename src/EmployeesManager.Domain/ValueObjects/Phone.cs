using System.Text.RegularExpressions;
using EmployeesManager.Domain.Common;
using EmployeesManager.Domain.Common.Results;

namespace EmployeesManager.Domain.ValueObjects;

public sealed class Phone : ValueObject
{
    public string Value { get; }

    private Phone(string value)
    {
        Value = value;
    }

    public static Result<Phone> Create(string? phone)
    {
        phone = phone?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(phone))
            return PhoneErrors.Required;

        if (phone.Length > PhoneConstants.MaxLength)
            return PhoneErrors.TooLong;

        if (!IsValidPhone(phone))
            return PhoneErrors.Invalid;

        return new Phone(phone);
    }

    private static bool IsValidPhone(string phone)
    {
        const string pattern = @"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$";
        return Regex.IsMatch(phone, pattern);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}

public static class PhoneConstants
{
    public const int MaxLength = 20;
}

public static class PhoneErrors
{
    public static readonly Error Required = Error.Validation(
        "Phone.Required",
        "Phone is required.",
        "Phone"
    );
    public static readonly Error Invalid = Error.Validation(
        "Phone.Invalid",
        "Phone format is invalid.",
        "Phone"
    );
    public static readonly Error TooLong = Error.Validation(
        "Phone.TooLong",
        "Phone is too long.",
        "Phone"
    );
}
