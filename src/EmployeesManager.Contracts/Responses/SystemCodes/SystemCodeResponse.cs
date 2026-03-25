namespace EmployeesManager.Contracts.Responses.SystemCodes;

public sealed record SystemCodeResponse(Guid Id, string Code, string? Description);
