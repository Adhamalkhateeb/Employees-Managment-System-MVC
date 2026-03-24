using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Cities.Commands.DeleteCity;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Citys.Commands;

public sealed class DeleteCityTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly DeleteCityCommandHandler _handler;

    public DeleteCityTests() => _handler = new DeleteCityCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.Citys to return null
        var result = await _handler.Handle(
            new DeleteCityCommand(Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_CallsRemoveAndSave()
    {
        // TODO: setup _context.Citys to return a valid entity
        var result = await _handler.Handle(
            new DeleteCityCommand(Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeTrue();
        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
