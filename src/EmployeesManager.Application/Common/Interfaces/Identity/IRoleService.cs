using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Identity;

namespace EmployeesManager.Application.Common.Interfaces.Identity;

public interface IRoleService
{
    Task<Result<Updated>> AssignRoleAsync(Guid userId, Role role);
    Task<Result<Updated>> RemoveRoleAsync(Guid userId, Role role);
}
