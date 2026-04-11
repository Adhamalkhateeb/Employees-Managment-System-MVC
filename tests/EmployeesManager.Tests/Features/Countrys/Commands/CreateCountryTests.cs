using EmployeesManager.Application.Features.Countries.Commands.CreateCountry;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.Countrys.Commands;

public sealed class CreateCountryTests
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        await using var context = CreateContext();
        var handler = new CreateCountryCommandHandler(context);
        var command = new CreateCountryCommand("EG", "Egypt");
        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        context.Countries.Count().Should().Be(1);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ValidatorFails()
    {
        var validator = new CreateCountryCommandValidator();
        var command = new CreateCountryCommand(string.Empty, string.Empty);
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
