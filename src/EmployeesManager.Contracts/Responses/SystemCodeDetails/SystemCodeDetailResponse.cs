namespace EmployeesManager.Contracts.Responses.SystemCodeDetails;

public sealed record SystemCodeDetailResponse(
    Guid Id,
    Guid SystemCodeId,
    string SystemCode,
    string Code,
    string? Description,
    int? OrderNo
);
