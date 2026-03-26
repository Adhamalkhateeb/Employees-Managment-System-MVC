using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Banks.Commands.CreateBank;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Banks.Commands;

public sealed class CreateBankTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly CreateBankCommandHandler _handler;

    public CreateBankTests() => _handler = new CreateBankCommandHandler(_context);

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        var command = new CreateBankCommand("B001", "Main Bank", "1234567890");
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Should().Be(Result.Created);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ValidatorFails()
    {
        var validator = new CreateBankCommandValidator();
        var command = new CreateBankCommand(string.Empty, string.Empty, string.Empty);
        var validation = await validator.ValidateAsync(command);

        validation.IsValid.Should().BeFalse();
        validation.Errors.Should().NotBeEmpty();
    }
}
