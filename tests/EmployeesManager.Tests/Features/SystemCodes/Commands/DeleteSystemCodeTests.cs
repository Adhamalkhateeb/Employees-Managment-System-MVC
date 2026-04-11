using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.SystemCodes.Commands.DeleteSystemCode;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.SystemCodes.Commands;

public sealed class DeleteSystemCodeTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly DeleteSystemCodeCommandHandler _handler;

    public DeleteSystemCodeTests()
        => _handler = new DeleteSystemCodeCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.SystemCodes to return null
        var result = await _handler.Handle(new DeleteSystemCodeCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_CallsRemoveAndSave()
    {
        // TODO: setup _context.SystemCodes to return a valid entity
        var result = await _handler.Handle(new DeleteSystemCodeCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
