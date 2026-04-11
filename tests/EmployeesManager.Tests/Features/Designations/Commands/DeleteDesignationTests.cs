using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Designations.Commands.DeleteDesignation;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Designations.Commands;

public sealed class DeleteDesignationTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly DeleteDesignationCommandHandler _handler;

    public DeleteDesignationTests()
        => _handler = new DeleteDesignationCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.Designations to return null
        var result = await _handler.Handle(new DeleteDesignationCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_CallsRemoveAndSave()
    {
        // TODO: setup _context.Designations to return a valid entity
        var result = await _handler.Handle(new DeleteDesignationCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        await _context.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
