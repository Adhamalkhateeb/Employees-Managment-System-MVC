using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Designations.Commands.CreateDesignation;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Designations.Commands;

public sealed class CreateDesignationTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly CreateDesignationCommandHandler _handler;

    public CreateDesignationTests() => _handler = new CreateDesignationCommandHandler(_context);

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        var command = new CreateDesignationCommand("Senior Developer");
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_InvalidCommand_ValidatorFails()
    {
        var validator = new CreateDesignationCommandValidator();
        var command = new CreateDesignationCommand(string.Empty);
        var validation = await validator.ValidateAsync(command);

        validation.IsValid.Should().BeFalse();
        validation.Errors.Should().NotBeEmpty();
    }
}
