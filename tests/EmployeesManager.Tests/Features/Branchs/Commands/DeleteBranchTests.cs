using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Branches.Commands.DeleteBranch;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Branchs.Commands;

public sealed class DeleteBranchTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly DeleteBranchCommandHandler _handler;

    public DeleteBranchTests() => _handler = new DeleteBranchCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.Branchs to return null
        var result = await _handler.Handle(
            new DeleteBranchCommand(Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_CallsRemoveAndSave()
    {
        // TODO: setup _context.Branchs to return a valid entity
        var result = await _handler.Handle(
            new DeleteBranchCommand(Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeTrue();
        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
