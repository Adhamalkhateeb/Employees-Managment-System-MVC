using EmployeesManager.Domain.Entities.LeaveApplications;
using EmployeesManager.Domain.Entities.Banks;
using EmployeesManager.Domain.Entities.Cities;
using EmployeesManager.Domain.Entities.Countries;
using EmployeesManager.Domain.Entities.Departments;
using EmployeesManager.Domain.Entities.Designations;
using EmployeesManager.Domain.Entities.Employees;
using EmployeesManager.Domain.Entities.LeaveTypes;
using EmployeesManager.Domain.Entities.SystemCodeDetails;
using EmployeesManager.Domain.Entities.SystemCodes;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Common.Interfaces;

public interface IAppDbContext
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    DbSet<Employee> Employees { get; }
    DbSet<Department> Departments { get; }
    DbSet<Designation> Designations { get; }
    DbSet<SystemCode> SystemCodes { get; }
    DbSet<SystemCodeDetail> SystemCodeDetails { get; }
    DbSet<Bank> Banks { get; }
    DbSet<LeaveType> LeaveTypes { get; }
    DbSet<Country> Countries { get; }
    DbSet<City> Cities { get; }
    DbSet<LeaveApplication> LeaveApplications { get; }
}
