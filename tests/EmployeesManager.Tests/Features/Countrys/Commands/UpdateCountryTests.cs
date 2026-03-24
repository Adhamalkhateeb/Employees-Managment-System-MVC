using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Countries.Commands.UpdateCountry;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Countrys.Commands;

public sealed class UpdateCountryTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly UpdateCountryCommandHandler _handler;

    public UpdateCountryTests() => _handler = new UpdateCountryCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.Countrys to return null
        var result = await _handler.Handle(
            new UpdateCountryCommand(Guid.NewGuid(), "EG", "Egypt"),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }
}
