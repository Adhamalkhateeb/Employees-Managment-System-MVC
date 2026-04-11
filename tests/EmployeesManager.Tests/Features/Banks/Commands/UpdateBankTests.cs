using EmployeesManager.Application.Features.Banks.Commands.UpdateBank;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Banks;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.Banks.Commands;

public sealed class UpdateBankTests
{
    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        await using var context = CreateContext();
        var handler = new UpdateBankCommandHandler(context);
        var result = await handler.Handle(
            new UpdateBankCommand(Guid.NewGuid(), "B001", "Main Bank", "1234567890"),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_UpdatesEntity()
    {
        await using var context = CreateContext();
        var handler = new UpdateBankCommandHandler(context);
        var bank = Bank.Create("B001", "Main Bank", "1234567890").Value;
        context.Banks.Add(bank);
        await context.SaveChangesAsync();

        var result = await handler.Handle(
            new UpdateBankCommand(bank.Id, "B009", "Updated Bank", "5555555555"),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeTrue();
        var updated = await context.Banks.FirstAsync(x => x.Id == bank.Id);
        updated.Code.Should().Be("B009");
        updated.Name.Should().Be("Updated Bank");
        updated.AccountNo.Should().Be("5555555555");
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
