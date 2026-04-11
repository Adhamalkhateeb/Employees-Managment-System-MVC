using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.SystemCodeDetails.Commands.UpdateSystemCodeDetail;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.SystemCodeDetails.Commands;

public sealed class UpdateSystemCodeDetailTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly UpdateSystemCodeDetailCommandHandler _handler;

    public UpdateSystemCodeDetailTests() =>
        _handler = new UpdateSystemCodeDetailCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.SystemCodeDetails to return null
        var result = await _handler.Handle(
            new UpdateSystemCodeDetailCommand(Guid.NewGuid(), Guid.NewGuid(), "M", "Male", 1),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }
}
