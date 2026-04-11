namespace EmployeesManager.Application.Features.SystemCodeDetails.Dtos;

public sealed record SystemCodeDetailDto(
    Guid Id,
    Guid SystemCodeId,
    string SystemCode,
    string Code,
    string? Description,
    int? OrderNo
);
