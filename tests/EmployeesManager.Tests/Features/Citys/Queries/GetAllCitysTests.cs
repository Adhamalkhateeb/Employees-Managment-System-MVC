using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Cities.Queries.GetAllCities;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Citys.Queries;

public sealed class GetAllCitysTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly GetAllCitiesQueryHandler _handler;

    public GetAllCitysTests() => _handler = new GetAllCitiesQueryHandler(_context);

    [Fact]
    public async Task Handle_WhenDataExists_ReturnsAllDtos()
    {
        // TODO: setup _context.Citys to return 2 entities
        var result = await _handler.Handle(new GetAllCitiesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoData_ReturnsEmptyList()
    {
        // TODO: setup _context.Citys to return empty list
        var result = await _handler.Handle(new GetAllCitiesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
