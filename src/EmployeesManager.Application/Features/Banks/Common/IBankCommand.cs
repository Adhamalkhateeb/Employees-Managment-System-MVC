namespace EmployeesManager.Application.Features.Banks.Common;

public interface IBankCommand
{
    string Code { get; }
    string Name { get; }
    string AccountNo { get; }
}
