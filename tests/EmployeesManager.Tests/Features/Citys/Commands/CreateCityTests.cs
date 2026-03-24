using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Cities.Commands.CreateCity;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Citys.Commands;

public sealed class CreateCityTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly CreateCityCommandHandler _handler;

    public CreateCityTests() => _handler = new CreateCityCommandHandler(_context);

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        var command = new CreateCityCommand("C001", "Cairo", Guid.NewGuid());
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_InvalidCommand_ValidatorFails()
    {
        var validator = new CreateCityCommandValidator();
        var command = new CreateCityCommand(string.Empty, string.Empty, Guid.Empty);
        var validation = await validator.ValidateAsync(command);

        validation.IsValid.Should().BeFalse();
        validation.Errors.Should().NotBeEmpty();
    }
}
