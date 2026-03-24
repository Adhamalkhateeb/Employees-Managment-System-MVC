using EmployeesManager.Domain.Entities.Employees;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Common.Interfaces;

public interface IAppDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<Employee> Employees { get; }
}
