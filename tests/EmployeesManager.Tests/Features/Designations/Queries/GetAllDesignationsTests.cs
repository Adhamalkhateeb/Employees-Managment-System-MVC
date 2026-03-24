using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Designations.Queries.GetAllDesignations;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Designations.Queries;

public sealed class GetAllDesignationsTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly GetAllDesignationsQueryHandler _handler;

    public GetAllDesignationsTests()
        => _handler = new GetAllDesignationsQueryHandler(_context);

    [Fact]
    public async Task Handle_WhenDataExists_ReturnsAllDtos()
    {
        // TODO: setup _context.Designations to return 2 entities
        var result = await _handler.Handle(new GetAllDesignationsQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoData_ReturnsEmptyList()
    {
        // TODO: setup _context.Designations to return empty list
        var result = await _handler.Handle(new GetAllDesignationsQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
