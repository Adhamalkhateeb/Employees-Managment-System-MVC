namespace EmployeesManager.Application.Features.SystemCodes.Common;

public interface ISystemCodeCommand
{
    string Name { get; }
    string Code { get; }
}
