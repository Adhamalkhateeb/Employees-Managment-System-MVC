using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Designations.Commands.UpdateDesignation;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Designations.Commands;

public sealed class UpdateDesignationTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly UpdateDesignationCommandHandler _handler;

    public UpdateDesignationTests() => _handler = new UpdateDesignationCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.Designations to return null
        var result = await _handler.Handle(
            new UpdateDesignationCommand(Guid.NewGuid(), "Senior Developer"),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }
}
