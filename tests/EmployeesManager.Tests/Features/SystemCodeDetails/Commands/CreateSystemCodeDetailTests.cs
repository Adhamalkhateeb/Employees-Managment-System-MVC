using EmployeesManager.Application.Features.SystemCodeDetails.Commands.CreateSystemCodeDetail;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.SystemCodeDetails.Commands;

public sealed class CreateSystemCodeDetailTests
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        await using var context = CreateContext();
        var handler = new CreateSystemCodeDetailCommandHandler(context);
        var command = new CreateSystemCodeDetailCommand(Guid.NewGuid(), "M", "Male", 1);
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        context.SystemCodeDetails.Count().Should().Be(1);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ValidatorFails()
    {
        var validator = new CreateSystemCodeDetailCommandValidator();
        var command = new CreateSystemCodeDetailCommand(Guid.Empty, string.Empty, string.Empty, -1);
        var validation = await validator.ValidateAsync(command);

        validation.IsValid.Should().BeFalse();
        validation.Errors.Should().NotBeEmpty();
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
