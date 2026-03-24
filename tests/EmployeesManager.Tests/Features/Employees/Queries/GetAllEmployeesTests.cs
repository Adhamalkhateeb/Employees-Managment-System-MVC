using EmployeesManager.Application.Features.Employees.Queries.GetAllEmployees;
using EmployeesManager.Domain.Entities.Countries;
using EmployeesManager.Domain.Entities.Departments;
using EmployeesManager.Domain.Entities.Designations;
using EmployeesManager.Domain.Entities.Employees;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.Employees.Queries;

public sealed class GetAllEmployeesTests
{
    [Fact]
    public async Task Handle_WhenDataExists_ReturnsAllDtos()
    {
        await using var context = CreateContext();
        var references = await SeedReferencesAsync(context);
        context.Employees.Add(
            Employee
                .Create(
                    "Adham",
                    null,
                    "Yasser",
                    "01000000000",
                    "adham@example.com",
                    new DateTime(1995, 5, 10),
                    "Cairo",
                    references.CountryId,
                    references.DepartmentId,
                    references.DesignationId
                )
                .Value
        );

        var hrDepartment = Department.Create("HR", "HR").Value;
        var specialistDesignation = Designation.Create("Specialist").Value;
        context.Departments.Add(hrDepartment);
        context.Designations.Add(specialistDesignation);
        await context.SaveChangesAsync();

        context.Employees.Add(
            Employee
                .Create(
                    "Ali",
                    null,
                    "Mahmoud",
                    "01100000000",
                    "ali@example.com",
                    new DateTime(1994, 4, 2),
                    "Giza",
                    references.CountryId,
                    hrDepartment.Id,
                    specialistDesignation.Id
                )
                .Value
        );
        await context.SaveChangesAsync();

        var handler = new GetAllEmployeesQueryHandler(context);
        var result = await handler.Handle(new GetAllEmployeesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoData_ReturnsEmptyList()
    {
        await using var context = CreateContext();
        var handler = new GetAllEmployeesQueryHandler(context);

        var result = await handler.Handle(new GetAllEmployeesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static async Task<(
        Guid CountryId,
        Guid DepartmentId,
        Guid DesignationId
    )> SeedReferencesAsync(AppDbContext context)
    {
        var country = Country.Create("EG", "Egypt").Value;
        var department = Department.Create("Engineering", "ENG").Value;
        var designation = Designation.Create("Developer").Value;

        context.Countries.Add(country);
        context.Departments.Add(department);
        context.Designations.Add(designation);
        await context.SaveChangesAsync();

        return (country.Id, department.Id, designation.Id);
    }
}
