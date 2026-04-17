namespace EmployeesManager.Application.Features.Designations.Common;

public interface IDesignationCommand
{
    string Name { get; }
    string Code { get; }
}
