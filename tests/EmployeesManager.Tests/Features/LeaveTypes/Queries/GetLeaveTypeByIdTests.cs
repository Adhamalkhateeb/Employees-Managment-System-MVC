using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveTypes.Queries.GetLeaveTypeById;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.LeaveTypes.Queries;

public sealed class GetLeaveTypeByIdTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly GetLeaveTypeByIdQueryHandler _handler;

    public GetLeaveTypeByIdTests()
        => _handler = new GetLeaveTypeByIdQueryHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.LeaveTypes to return null
        var result = await _handler.Handle(new GetLeaveTypeByIdQuery(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_ReturnsDtoWithMatchingId()
    {
        var id = Guid.NewGuid();
        // TODO: setup _context.LeaveTypes to return entity with this id

        var result = await _handler.Handle(new GetLeaveTypeByIdQuery(id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(id);
    }
}
