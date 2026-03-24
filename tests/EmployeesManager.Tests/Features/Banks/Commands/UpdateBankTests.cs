using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Banks.Commands.UpdateBank;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Banks.Commands;

public sealed class UpdateBankTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly UpdateBankCommandHandler _handler;

    public UpdateBankTests() => _handler = new UpdateBankCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.Banks to return null
        var result = await _handler.Handle(
            new UpdateBankCommand(Guid.NewGuid(), "B001", "Main Bank", "1234567890"),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }
}
