using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveApplications.Commands.CreateLeaveApplication;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.LeaveApplications.Commands;

public sealed class CreateLeaveApplicationTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly CreateLeaveApplicationCommandHandler _handler;

    public CreateLeaveApplicationTests() =>
        _handler = new CreateLeaveApplicationCommandHandler(_context);

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        var command = new CreateLeaveApplicationCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            Guid.NewGuid(),
            DateTimeOffset.UtcNow,
            DateTimeOffset.UtcNow.AddDays(1),
            "Annual leave",
            null
        );
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_InvalidCommand_ValidatorFails()
    {
        var validator = new CreateLeaveApplicationCommandValidator();
        var command = new CreateLeaveApplicationCommand(
            Guid.Empty,
            Guid.Empty,
            Guid.Empty,
            Guid.Empty,
            DateTimeOffset.MinValue,
            DateTimeOffset.MinValue,
            string.Empty,
            null
        );
        var validation = await validator.ValidateAsync(command);

        validation.IsValid.Should().BeFalse();
        validation.Errors.Should().NotBeEmpty();
    }
}
