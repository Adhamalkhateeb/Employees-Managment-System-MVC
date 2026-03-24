using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.SystemCodes.Commands.UpdateSystemCode;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.SystemCodes.Commands;

public sealed class UpdateSystemCodeTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly UpdateSystemCodeCommandHandler _handler;

    public UpdateSystemCodeTests() => _handler = new UpdateSystemCodeCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.SystemCodes to return null
        var result = await _handler.Handle(
            new UpdateSystemCodeCommand(Guid.NewGuid(), "GENDER", "GEN"),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }
}
