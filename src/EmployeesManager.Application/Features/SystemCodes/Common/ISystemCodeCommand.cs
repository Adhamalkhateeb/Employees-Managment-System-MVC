namespace EmployeesManager.Application.Features.SystemCodes.Common;

public interface ISystemCodeCommand
{
    string Code { get; }
    string? Description { get; }
}
