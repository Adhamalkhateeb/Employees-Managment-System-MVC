using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveTypes.Commands.UpdateLeaveType;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.LeaveTypes.Commands;

public sealed class UpdateLeaveTypeTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly UpdateLeaveTypeCommandHandler _handler;

    public UpdateLeaveTypeTests() => _handler = new UpdateLeaveTypeCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.LeaveTypes to return null
        var result = await _handler.Handle(
            new UpdateLeaveTypeCommand(Guid.NewGuid(), "Annual Leave"),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }
}
