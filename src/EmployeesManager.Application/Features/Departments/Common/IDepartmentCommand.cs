namespace EmployeesManager.Application.Features.Departments.Common;

public interface IDepartmentCommand
{
    string Name { get; }
    string Code { get; }
}
