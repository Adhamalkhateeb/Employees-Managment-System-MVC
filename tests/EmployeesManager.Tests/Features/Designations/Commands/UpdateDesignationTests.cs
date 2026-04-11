using EmployeesManager.Application.Features.Designations.Commands.UpdateDesignation;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Designations;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.Designations.Commands;

public sealed class UpdateDesignationTests
{
    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        await using var context = CreateContext();
        var handler = new UpdateDesignationCommandHandler(context);
        var result = await handler.Handle(
            new UpdateDesignationCommand(Guid.NewGuid(), "Senior Developer", "SD"),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_UpdatesEntity()
    {
        await using var context = CreateContext();
        var entity = Designation.Create("Senior Developer", "SD").Value;
        context.Designations.Add(entity);
        await context.SaveChangesAsync();

        var handler = new UpdateDesignationCommandHandler(context);
        var result = await handler.Handle(
            new UpdateDesignationCommand(entity.Id, "Tech Lead", "TL"),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeTrue();

        var updated = await context.Designations.FirstAsync(x => x.Id == entity.Id);
        updated.Name.Should().Be("Tech Lead");
        updated.Code.Should().Be("TL");
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
