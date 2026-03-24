using EmployeesManager.Application.Features.Employees.Queries.GetAllEmployees;
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
        context.Employees.Add(
            Employee
                .Create(
                    "Adham",
                    null,
                    "Yasser",
                    "01000000000",
                    "adham@example.com",
                    "Egypt",
                    new DateTime(1995, 5, 10),
                    "Cairo",
                    "Engineering",
                    "Developer"
                )
                .Value
        );
        context.Employees.Add(
            Employee
                .Create(
                    "Ali",
                    null,
                    "Mahmoud",
                    "01100000000",
                    "ali@example.com",
                    "Egypt",
                    new DateTime(1994, 4, 2),
                    "Giza",
                    "HR",
                    "Specialist"
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
}
