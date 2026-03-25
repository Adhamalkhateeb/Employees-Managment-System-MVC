using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveApplications.Queries.GetLeaveApplicationById;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.LeaveApplications.Queries;

public sealed class GetLeaveApplicationByIdTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly GetLeaveApplicationByIdQueryHandler _handler;

    public GetLeaveApplicationByIdTests()
        => _handler = new GetLeaveApplicationByIdQueryHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.LeaveApplications to return null
        var result = await _handler.Handle(new GetLeaveApplicationByIdQuery(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_ReturnsDtoWithMatchingId()
    {
        var id = Guid.NewGuid();
        // TODO: setup _context.LeaveApplications to return entity with this id

        var result = await _handler.Handle(new GetLeaveApplicationByIdQuery(id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(id);
    }
}
