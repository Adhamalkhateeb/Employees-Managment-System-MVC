using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.SystemCodes.Queries.GetSystemCodeById;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.SystemCodes.Queries;

public sealed class GetSystemCodeByIdTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly GetSystemCodeByIdQueryHandler _handler;

    public GetSystemCodeByIdTests()
        => _handler = new GetSystemCodeByIdQueryHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.SystemCodes to return null
        var result = await _handler.Handle(new GetSystemCodeByIdQuery(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_ReturnsDtoWithMatchingId()
    {
        var id = Guid.NewGuid();
        // TODO: setup _context.SystemCodes to return entity with this id

        var result = await _handler.Handle(new GetSystemCodeByIdQuery(id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(id);
    }
}
