using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveTypes.Commands.DeleteLeaveType;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.LeaveTypes.Commands;

public sealed class DeleteLeaveTypeTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly DeleteLeaveTypeCommandHandler _handler;

    public DeleteLeaveTypeTests()
        => _handler = new DeleteLeaveTypeCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.LeaveTypes to return null
        var result = await _handler.Handle(new DeleteLeaveTypeCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_CallsRemoveAndSave()
    {
        // TODO: setup _context.LeaveTypes to return a valid entity
        var result = await _handler.Handle(new DeleteLeaveTypeCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
