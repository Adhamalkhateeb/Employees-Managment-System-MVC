using EmployeesManager.Application.Features.LeaveTypes.Commands.CreateLeaveType;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.LeaveTypes.Commands;

public sealed class CreateLeaveTypeTests
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        await using var context = CreateContext();
        var handler = new CreateLeaveTypeCommandHandler(context);
        var command = new CreateLeaveTypeCommand("Annual Leave", "AL");

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        context.LeaveTypes.Count().Should().Be(1);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ValidatorFails()
    {
        var validator = new CreateLeaveTypeCommandValidator();
        var command = new CreateLeaveTypeCommand(string.Empty, string.Empty);
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
