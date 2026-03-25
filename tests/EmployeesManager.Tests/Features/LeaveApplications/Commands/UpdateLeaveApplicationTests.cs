using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveApplications.Commands.UpdateLeaveApplication;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.LeaveApplications.Commands;

public sealed class UpdateLeaveApplicationTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly UpdateLeaveApplicationCommandHandler _handler;

    public UpdateLeaveApplicationTests() =>
        _handler = new UpdateLeaveApplicationCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.LeaveApplications to return null
        var result = await _handler.Handle(
            new UpdateLeaveApplicationCommand(
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                Guid.NewGuid(),
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow.AddDays(1),
                "Annual leave",
                null
            ),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }
}
