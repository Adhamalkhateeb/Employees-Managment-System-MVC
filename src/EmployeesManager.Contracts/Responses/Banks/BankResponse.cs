namespace EmployeesManager.Contracts.Responses.Banks;

public sealed record BankResponse(Guid Id, string Code, string Name, string AccountNo);
