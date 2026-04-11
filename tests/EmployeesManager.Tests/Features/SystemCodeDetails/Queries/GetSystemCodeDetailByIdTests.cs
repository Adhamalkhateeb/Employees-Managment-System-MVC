using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.SystemCodeDetails.Queries.GetSystemCodeDetailById;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.SystemCodeDetails.Queries;

public sealed class GetSystemCodeDetailByIdTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly GetSystemCodeDetailByIdQueryHandler _handler;

    public GetSystemCodeDetailByIdTests()
        => _handler = new GetSystemCodeDetailByIdQueryHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.SystemCodeDetails to return null
        var result = await _handler.Handle(new GetSystemCodeDetailByIdQuery(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_ReturnsDtoWithMatchingId()
    {
        var id = Guid.NewGuid();
        // TODO: setup _context.SystemCodeDetails to return entity with this id

        var result = await _handler.Handle(new GetSystemCodeDetailByIdQuery(id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(id);
    }
}
