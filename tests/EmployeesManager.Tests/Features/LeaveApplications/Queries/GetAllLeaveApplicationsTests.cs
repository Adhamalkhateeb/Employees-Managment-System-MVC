using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveApplications.Queries.GetAllLeaveApplications;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.LeaveApplications.Queries;

public sealed class GetAllLeaveApplicationsTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly GetAllLeaveApplicationsQueryHandler _handler;

    public GetAllLeaveApplicationsTests()
        => _handler = new GetAllLeaveApplicationsQueryHandler(_context);

    [Fact]
    public async Task Handle_WhenDataExists_ReturnsAllDtos()
    {
        // TODO: setup _context.LeaveApplications to return 2 entities
        var result = await _handler.Handle(new GetAllLeaveApplicationsQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
    }

    [Fact]
    public async Task Handle_WhenNoData_ReturnsEmptyList()
    {
        // TODO: setup _context.LeaveApplications to return empty list
        var result = await _handler.Handle(new GetAllLeaveApplicationsQuery(), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }
}
