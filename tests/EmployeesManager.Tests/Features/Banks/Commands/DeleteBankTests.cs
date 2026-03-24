using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Banks.Commands.DeleteBank;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Banks.Commands;

public sealed class DeleteBankTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly DeleteBankCommandHandler _handler;

    public DeleteBankTests()
        => _handler = new DeleteBankCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.Banks to return null
        var result = await _handler.Handle(new DeleteBankCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_CallsRemoveAndSave()
    {
        // TODO: setup _context.Banks to return a valid entity
        var result = await _handler.Handle(new DeleteBankCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
