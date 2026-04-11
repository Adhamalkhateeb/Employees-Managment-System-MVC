using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Cities.Commands.UpdateCity;
using EmployeesManager.Domain.Common.Results;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace EmployeesManager.Tests.Features.Citys.Commands;

public sealed class UpdateCityTests
{
    private readonly IAppDbContext _context = Substitute.For<IAppDbContext>();
    private readonly UpdateCityCommandHandler _handler;

    public UpdateCityTests() => _handler = new UpdateCityCommandHandler(_context);

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        // TODO: setup _context.Citys to return null
        var result = await _handler.Handle(
            new UpdateCityCommand(Guid.NewGuid(), "C001", "Cairo", Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }
}
