using EmployeesManager.Domain.Entities.LeaveApplications;
using System.Reflection;
using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Entities.Banks;
using EmployeesManager.Domain.Entities.Cities;
using EmployeesManager.Domain.Entities.Countries;
using EmployeesManager.Domain.Entities.Departments;
using EmployeesManager.Domain.Entities.Designations;
using EmployeesManager.Domain.Entities.Employees;
using EmployeesManager.Domain.Entities.LeaveTypes;
using EmployeesManager.Domain.Entities.SystemCodeDetails;
using EmployeesManager.Domain.Entities.SystemCodes;
using EmployeesManager.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Infrastructure.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<AppUser, AppRole, Guid>(options),
        IAppDbContext
{
    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<Department> Departments => Set<Department>();

    public DbSet<Designation> Designations => Set<Designation>();

    public DbSet<SystemCode> SystemCodes => Set<SystemCode>();

    public DbSet<SystemCodeDetail> SystemCodeDetails => Set<SystemCodeDetail>();

    public DbSet<Bank> Banks => Set<Bank>();

    public DbSet<LeaveType> LeaveTypes => Set<LeaveType>();

    public DbSet<Country> Countries => Set<Country>();

    public DbSet<City> Cities => Set<City>();

        public DbSet<LeaveApplication> LeaveApplications => Set<LeaveApplication>();

protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}

