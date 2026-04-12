using EmployeesManager.Domain.Entities.Branches;
using EmployeesManager.Domain.Entities.Departments;
using EmployeesManager.Domain.Entities.Employees;
using EmployeesManager.Domain.Entities.LeaveTypes;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Common.Interfaces;

public interface IAppDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<Employee> Employees { get; }
    DbSet<Department> Departments { get; }
    DbSet<Branch> Branches { get; }
    DbSet<LeaveType> LeaveTypes { get; }
    DbSet<LeaveApplication> LeaveApplications { get; }
}
