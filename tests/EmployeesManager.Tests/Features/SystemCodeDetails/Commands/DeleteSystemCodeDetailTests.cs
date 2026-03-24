using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.SystemCodeDetails.Commands.DeleteSystemCodeDetail;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.SystemCodeDetails.Commands;

public sealed class DeleteSystemCodeDetailTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly DeleteSystemCodeDetailCommandHandler _handler;

    public DeleteSystemCodeDetailTests()
        => _handler = new DeleteSystemCodeDetailCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.SystemCodeDetails to return null
        var result = await _handler.Handle(new DeleteSystemCodeDetailCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_CallsRemoveAndSave()
    {
        // TODO: setup _context.SystemCodeDetails to return a valid entity
        var result = await _handler.Handle(new DeleteSystemCodeDetailCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
