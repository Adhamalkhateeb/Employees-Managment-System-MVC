using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.SystemCodeDetails.Queries.GetAllSystemCodeDetails;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.SystemCodeDetails.Queries;

public sealed class GetAllSystemCodeDetailsTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly GetAllSystemCodeDetailsQueryHandler _handler;

    public GetAllSystemCodeDetailsTests()
        => _handler = new GetAllSystemCodeDetailsQueryHandler(_context);

    [Fact]
    public async Task Handle_WhenDataExists_ReturnsAllDtos()
    {
        // TODO: setup _context.SystemCodeDetails to return 2 entities
        var result = await _handler.Handle(new GetAllSystemCodeDetailsQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoData_ReturnsEmptyList()
    {
        // TODO: setup _context.SystemCodeDetails to return empty list
        var result = await _handler.Handle(new GetAllSystemCodeDetailsQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
