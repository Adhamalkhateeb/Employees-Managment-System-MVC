using EmployeesManager.Application.Features.LeaveTypes.Commands.UpdateLeaveType;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.LeaveTypes;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.LeaveTypes.Commands;

public sealed class UpdateLeaveTypeTests
{
    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        await using var context = CreateContext();
        var handler = new UpdateLeaveTypeCommandHandler(context);
        var result = await handler.Handle(
            new UpdateLeaveTypeCommand(Guid.NewGuid(), "Annual Leave", "AL"),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_UpdatesEntity()
    {
        await using var context = CreateContext();
        var entity = LeaveType.Create("Annual Leave", "AL").Value;
        context.LeaveTypes.Add(entity);
        await context.SaveChangesAsync();

        var handler = new UpdateLeaveTypeCommandHandler(context);
        var result = await handler.Handle(
            new UpdateLeaveTypeCommand(entity.Id, "Sick Leave", "SL"),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeTrue();

        var updated = await context.LeaveTypes.FirstAsync(x => x.Id == entity.Id);
        updated.Name.Should().Be("Sick Leave");
        updated.Code.Should().Be("SL");
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
