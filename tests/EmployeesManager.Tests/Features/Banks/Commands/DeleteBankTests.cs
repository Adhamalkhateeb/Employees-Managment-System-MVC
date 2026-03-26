using EmployeesManager.Application.Features.Banks.Commands.DeleteBank;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Banks;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.Banks.Commands;

public sealed class DeleteBankTests
{
    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        await using var context = CreateContext();
        var handler = new DeleteBankCommandHandler(context);

        var result = await handler.Handle(
            new DeleteBankCommand(Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_CallsRemoveAndSave()
    {
        await using var context = CreateContext();
        var handler = new DeleteBankCommandHandler(context);
        var bank = Bank.Create("B001", "Main Bank", "1234567890").Value;
        context.Banks.Add(bank);
        await context.SaveChangesAsync();

        var result = await handler.Handle(new DeleteBankCommand(bank.Id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        context.Banks.Count().Should().Be(0);
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
