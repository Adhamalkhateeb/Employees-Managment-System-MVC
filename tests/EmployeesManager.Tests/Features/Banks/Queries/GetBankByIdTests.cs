using EmployeesManager.Application.Features.Banks.Queries.GetBankById;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Banks;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.Banks.Queries;

public sealed class GetBankByIdTests
{
    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        await using var context = CreateContext();
        var handler = new GetBankByIdQueryHandler(context);
        var result = await handler.Handle(
            new GetBankByIdQuery(Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_ReturnsDtoWithMatchingId()
    {
        await using var context = CreateContext();
        var handler = new GetBankByIdQueryHandler(context);
        var bank = Bank.Create("B001", "Main Bank", "1234567890").Value;
        context.Banks.Add(bank);
        await context.SaveChangesAsync();

        var result = await handler.Handle(new GetBankByIdQuery(bank.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(bank.Id);
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
