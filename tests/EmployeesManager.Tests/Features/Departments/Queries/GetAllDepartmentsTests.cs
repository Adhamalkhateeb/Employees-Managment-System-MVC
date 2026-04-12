using EmployeesManager.Application.Features.Departments.Queries.GetAllDepartments;
using EmployeesManager.Domain.Entities.Departments;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.Departments.Queries;

public sealed class GetAllDepartmentsTests
{
    [Fact]
    public async Task Handle_WhenDataExists_ReturnsAllDtos()
    {
        await using var context = CreateContext();
        context.Departments.Add(Department.Create("Engineering", "ENG").Value);
        context.Departments.Add(Department.Create("Human Resources", "HR").Value);
        await context.SaveChangesAsync();

        var handler = new GetDepartmentsQueryHandler(context);
        var result = await handler.Handle(new GetAllDepartmentsQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoData_ReturnsEmptyList()
    {
        await using var context = CreateContext();
        var handler = new GetDepartmentsQueryHandler(context);
        var result = await handler.Handle(new GetAllDepartmentsQuery(), CancellationToken.None);

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
}
