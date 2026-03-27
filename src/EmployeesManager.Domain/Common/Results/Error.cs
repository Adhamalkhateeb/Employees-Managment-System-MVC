namespace EmployeesManager.Domain.Common.Results;

public readonly record struct Error
{
    private Error(string code, string description, ErrorKind type, string? propertyName = null)
    {
        Code = code;
        Description = description;
        Type = type;
        PropertyName = propertyName;
    }

    public string Code { get; }
    public string Description { get; }
    public ErrorKind Type { get; }

    public string? PropertyName { get; }

    public static Error Validation(
        string code = nameof(Validation),
        string description = "Validation error.",
        string? propertyName = null
    ) => new(code, description, ErrorKind.Validation, propertyName);

    public static Error Conflict(
        string code = nameof(Conflict),
        string description = "Conflict error.",
        string? propertyName = null
    ) => new(code, description, ErrorKind.Conflict, propertyName);

    public static Error NotFound(
        string code = nameof(NotFound),
        string description = "Not found.",
        string? propertyName = null
    ) => new(code, description, ErrorKind.NotFound, propertyName);

    public static Error Unauthorized(
        string code = nameof(Unauthorized),
        string description = "Unauthorized.",
        string? propertyName = null
    ) => new(code, description, ErrorKind.Unauthorized, propertyName);

    public static Error Forbidden(
        string code = nameof(Forbidden),
        string description = "Forbidden.",
        string? propertyName = null
    ) => new(code, description, ErrorKind.Forbidden, propertyName);

    public static Error Failure(
        string code = nameof(Failure),
        string description = "General failure.",
        string? propertyName = null
    ) => new(code, description, ErrorKind.Failure, propertyName);

    public static Error Unexpected(
        string code = nameof(Unexpected),
        string description = "Unexpected error.",
        string? propertyName = null
    ) => new(code, description, ErrorKind.Unexpected, propertyName);

    public static Error Create(
        ErrorKind type,
        string code,
        string description,
        string? propertyName = null
    ) => new(code, description, type, propertyName);
}
