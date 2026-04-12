using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Branches.Commands.UpdateBranch;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Branchs.Commands;

public sealed class UpdateBranchTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly UpdateBranchCommandHandler _handler;

    public UpdateBranchTests() => _handler = new UpdateBranchCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.Branchs to return null
        var result = await _handler.Handle(
            new UpdateBranchCommand(Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }
}
