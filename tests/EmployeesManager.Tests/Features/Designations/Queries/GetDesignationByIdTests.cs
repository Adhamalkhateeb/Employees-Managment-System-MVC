using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Designations.Queries.GetDesignationById;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Designations.Queries;

public sealed class GetDesignationByIdTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly GetDesignationByIdQueryHandler _handler;

    public GetDesignationByIdTests()
        => _handler = new GetDesignationByIdQueryHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.Designations to return null
        var result = await _handler.Handle(new GetDesignationByIdQuery(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_ReturnsDtoWithMatchingId()
    {
        var id = Guid.NewGuid();
        // TODO: setup _context.Designations to return entity with this id

        var result = await _handler.Handle(new GetDesignationByIdQuery(id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(id);
    }
}
