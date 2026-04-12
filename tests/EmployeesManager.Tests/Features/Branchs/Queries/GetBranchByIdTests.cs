using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Branches.Queries.GetBranchById;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Branchs.Queries;

public sealed class GetBranchByIdTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly GetBranchByIdQueryHandler _handler;

    public GetBranchByIdTests() => _handler = new GetBranchByIdQueryHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.Branchs to return null
        var result = await _handler.Handle(
            new GetBranchByIdQuery(Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_ReturnsDtoWithMatchingId()
    {
        var id = Guid.NewGuid();
        // TODO: setup _context.Branchs to return entity with this id

        var result = await _handler.Handle(new GetBranchByIdQuery(id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(id);
    }
}
