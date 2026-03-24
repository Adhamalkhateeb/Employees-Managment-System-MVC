using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Countries.Commands.DeleteCountry;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Countrys.Commands;

public sealed class DeleteCountryTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly DeleteCountryCommandHandler _handler;

    public DeleteCountryTests() => _handler = new DeleteCountryCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.Countrys to return null
        var result = await _handler.Handle(
            new DeleteCountryCommand(Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_CallsRemoveAndSave()
    {
        // TODO: setup _context.Countrys to return a valid entity
        var result = await _handler.Handle(
            new DeleteCountryCommand(Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeTrue();
        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
