using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Countries.Queries.GetCountryById;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Countrys.Queries;

public sealed class GetCountryByIdTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly GetCountryByIdQueryHandler _handler;

    public GetCountryByIdTests() => _handler = new GetCountryByIdQueryHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.Countrys to return null
        var result = await _handler.Handle(
            new GetCountryByIdQuery(Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_ReturnsDtoWithMatchingId()
    {
        var id = Guid.NewGuid();
        // TODO: setup _context.Countrys to return entity with this id

        var result = await _handler.Handle(new GetCountryByIdQuery(id), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(id);
    }
}
