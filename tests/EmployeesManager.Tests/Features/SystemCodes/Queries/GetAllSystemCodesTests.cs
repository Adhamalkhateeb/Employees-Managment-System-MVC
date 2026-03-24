using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.SystemCodes.Queries.GetAllSystemCodes;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.SystemCodes.Queries;

public sealed class GetAllSystemCodesTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly GetAllSystemCodesQueryHandler _handler;

    public GetAllSystemCodesTests()
        => _handler = new GetAllSystemCodesQueryHandler(_context);

    [Fact]
    public async Task Handle_WhenDataExists_ReturnsAllDtos()
    {
        // TODO: setup _context.SystemCodes to return 2 entities
        var result = await _handler.Handle(new GetAllSystemCodesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoData_ReturnsEmptyList()
    {
        // TODO: setup _context.SystemCodes to return empty list
        var result = await _handler.Handle(new GetAllSystemCodesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
