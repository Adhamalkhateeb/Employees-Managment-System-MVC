using EmployeesManager.Application.Features.Banks.Queries.GetAllBanks;
using EmployeesManager.Domain.Entities.Banks;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.Banks.Queries;

public sealed class GetAllBanksTests
{
    [Fact]
    public async Task Handle_WhenDataExists_ReturnsAllDtos()
    {
        await using var context = CreateContext();
        var handler = new GetAllBanksQueryHandler(context);
        context.Banks.Add(Bank.Create("B001", "Main Bank", "1234567890").Value);
        context.Banks.Add(Bank.Create("B002", "Second Bank", "1234567891").Value);
        await context.SaveChangesAsync();

        var result = await handler.Handle(new GetAllBanksQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoData_ReturnsEmptyList()
    {
        await using var context = CreateContext();
        var handler = new GetAllBanksQueryHandler(context);

        var result = await handler.Handle(new GetAllBanksQuery(), CancellationToken.None);

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
