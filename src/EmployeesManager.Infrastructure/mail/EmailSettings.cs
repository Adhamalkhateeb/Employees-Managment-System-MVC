namespace EmployeesManager.Infrastructure.Mail;

public sealed class EmailSettings
{
    public const string SectionName = "EmailSettings";
    public string Host { get; init; } = string.Empty;

    public int Port { get; init; } = 587;

    public bool UseSsl { get; init; } = false;

    public string UserName { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;

    public string FromEmail { get; init; } = string.Empty;

    public string FromName { get; init; } = string.Empty;
    public int TimeoutSeconds { get; init; } = 30;
}
