using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveApplications.Commands.DeleteLeaveApplication;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.LeaveApplications.Commands;

public sealed class DeleteLeaveApplicationTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly DeleteLeaveApplicationCommandHandler _handler;

    public DeleteLeaveApplicationTests()
        => _handler = new DeleteLeaveApplicationCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.LeaveApplications to return null
        var result = await _handler.Handle(new DeleteLeaveApplicationCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_CallsRemoveAndSave()
    {
        // TODO: setup _context.LeaveApplications to return a valid entity
        var result = await _handler.Handle(new DeleteLeaveApplicationCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
