namespace EmployeesManager.Application.Features.SystemCodeDetails.Common;

public interface ISystemCodeDetailCommand
{
    Guid SystemCodeId { get; }
    string Code { get; }
    string? Description { get; }
    int? OrderNo { get; }
}
