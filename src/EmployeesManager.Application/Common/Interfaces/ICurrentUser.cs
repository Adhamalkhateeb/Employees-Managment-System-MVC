namespace EmployeesManager.Application.Common.Interfaces;

public interface ICurrentUser
{
    Guid Id { get; }
    string UserName { get; }
    bool IsAuthenticated { get; }

    bool IsInRole(string role);
}
