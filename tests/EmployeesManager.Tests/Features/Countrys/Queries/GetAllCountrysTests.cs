using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Countries.Queries.GetAllCountries;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Countrys.Queries;

public sealed class GetAllCountrysTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly GetAllCountriesQueryHandler _handler;

    public GetAllCountrysTests() => _handler = new GetAllCountriesQueryHandler(_context);

    [Fact]
    public async Task Handle_WhenDataExists_ReturnsAllDtos()
    {
        // TODO: setup _context.Countrys to return 2 entities
        var result = await _handler.Handle(new GetAllCountriesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoData_ReturnsEmptyList()
    {
        // TODO: setup _context.Countrys to return empty list
        var result = await _handler.Handle(new GetAllCountriesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
