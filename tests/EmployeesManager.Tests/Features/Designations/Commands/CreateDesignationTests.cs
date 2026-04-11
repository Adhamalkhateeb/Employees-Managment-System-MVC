using EmployeesManager.Application.Features.Designations.Commands.CreateDesignation;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.Designations.Commands;

public sealed class CreateDesignationTests
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        await using var context = CreateContext();
        var handler = new CreateDesignationCommandHandler(context);
        var command = new CreateDesignationCommand("Senior Developer", "SD");

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        context.Designations.Count().Should().Be(1);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ValidatorFails()
    {
        var validator = new CreateDesignationCommandValidator();
        var command = new CreateDesignationCommand(string.Empty, string.Empty);
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
