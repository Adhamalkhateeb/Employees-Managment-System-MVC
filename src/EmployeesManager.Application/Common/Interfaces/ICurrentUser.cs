namespace EmployeesManager.Application.Common.Interfaces;

public interface ICurrentUser
{
    Guid? Id { get; }
    string? UserName { get; }
    string? Email { get; }
    bool IsAuthenticated { get; }
    bool IsInRole(string role);
    IEnumerable<string> Roles { get; }
}
