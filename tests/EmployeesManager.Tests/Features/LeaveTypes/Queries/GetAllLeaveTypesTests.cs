using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveTypes.Queries.GetAllLeaveTypes;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.LeaveTypes.Queries;

public sealed class GetAllLeaveTypesTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly GetAllLeaveTypesQueryHandler _handler;

    public GetAllLeaveTypesTests()
        => _handler = new GetAllLeaveTypesQueryHandler(_context);

    [Fact]
    public async Task Handle_WhenDataExists_ReturnsAllDtos()
    {
        // TODO: setup _context.LeaveTypes to return 2 entities
        var result = await _handler.Handle(new GetAllLeaveTypesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoData_ReturnsEmptyList()
    {
        // TODO: setup _context.LeaveTypes to return empty list
        var result = await _handler.Handle(new GetAllLeaveTypesQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
