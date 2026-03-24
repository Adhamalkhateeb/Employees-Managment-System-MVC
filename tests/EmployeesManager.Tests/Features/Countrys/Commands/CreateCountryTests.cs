using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Countries.Commands.CreateCountry;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Countrys.Commands;

public sealed class CreateCountryTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly CreateCountryCommandHandler _handler;

    public CreateCountryTests() => _handler = new CreateCountryCommandHandler(_context);

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        var command = new CreateCountryCommand("EG", "Egypt");
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();
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
}
