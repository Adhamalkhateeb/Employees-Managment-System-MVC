using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Branches.Queries.GetAllBranchs;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Branchs.Queries;

public sealed class GetAllBranchsTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly GetAllBranchsQueryHandler _handler;

    public GetAllBranchsTests() => _handler = new GetAllBranchsQueryHandler(_context);

    [Fact]
    public async Task Handle_WhenDataExists_ReturnsAllDtos()
    {
        // TODO: setup _context.Branchs to return 2 entities
        var result = await _handler.Handle(new GetAllBranchsQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoData_ReturnsEmptyList()
    {
        // TODO: setup _context.Branchs to return empty list
        var result = await _handler.Handle(new GetAllBranchsQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
