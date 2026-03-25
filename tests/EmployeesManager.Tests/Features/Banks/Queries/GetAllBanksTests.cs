using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Banks.Queries.GetAllBanks;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Banks.Queries;

public sealed class GetAllBanksTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly GetAllBanksQueryHandler _handler;

    public GetAllBanksTests() => _handler = new GetAllBanksQueryHandler(_context);

    [Fact]
    public async Task Handle_WhenDataExists_ReturnsAllDtos()
    {
        // TODO: setup _context.Banks to return 2 entities
        var result = await _handler.Handle(new GetAllBanksQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoData_ReturnsEmptyList()
    {
        // TODO: setup _context.Banks to return empty list
        var result = await _handler.Handle(new GetAllBanksQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
