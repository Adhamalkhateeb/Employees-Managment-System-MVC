using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Branches.Commands.CreateBranch;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Branchs.Commands;

public sealed class CreateBranchTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly CreateBranchCommandHandler _handler;

    public CreateBranchTests() => _handler = new CreateBranchCommandHandler(_context);

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        var command =
            new CreateBranchCommand( /* TODO: valid properties */
            );
        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Handle_InvalidCommand_ValidatorFails()
    {
        var validator = new CreateBranchCommandValidator();
        var command =
            new CreateBranchCommand( /* TODO: invalid properties */
            );
        var validation = await validator.ValidateAsync(command);

        validation.IsValid.Should().BeFalse();
        validation.Errors.Should().NotBeEmpty();
    }
}
