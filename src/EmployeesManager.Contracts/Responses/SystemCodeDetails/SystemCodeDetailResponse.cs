namespace EmployeesManager.Contracts.Responses.SystemCodeDetails;

public sealed record SystemCodeDetailResponse(
    Guid Id,
    Guid SystemCodeId,
    string SystemCodeName,
    string Code,
    string Description,
    int? OrderNo
);
