using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.SystemCodeDetails.Commands.CreateSystemCodeDetail;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.SystemCodeDetails.Commands;

public sealed class CreateSystemCodeDetailTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly CreateSystemCodeDetailCommandHandler _handler;

    public CreateSystemCodeDetailTests() =>
        _handler = new CreateSystemCodeDetailCommandHandler(_context);

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        var command = new CreateSystemCodeDetailCommand(Guid.NewGuid(), "M", "Male", 1);
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();
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
}
