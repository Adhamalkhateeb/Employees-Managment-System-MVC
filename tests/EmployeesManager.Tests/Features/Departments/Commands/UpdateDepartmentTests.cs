using EmployeesManager.Application.Features.Departments.Commands.UpdateDepartment;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Departments;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.Departments.Commands;

public sealed class UpdateDepartmentTests
{
    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        await using var context = CreateContext();
        var handler = new UpdateDepartmentCommandHandler(context);
        var result = await handler.Handle(
            new UpdateDepartmentCommand(Guid.NewGuid(), "Engineering", "ENG"),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_UpdatesEntity()
    {
        await using var context = CreateContext();
        var entity = Department.Create("Engineering", "ENG").Value;
        context.Departments.Add(entity);
        await context.SaveChangesAsync();

        var handler = new UpdateDepartmentCommandHandler(context);
        var result = await handler.Handle(
            new UpdateDepartmentCommand(entity.Id, "Human Resources", "HR"),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeTrue();

        var updated = await context.Departments.FirstAsync(x => x.Id == entity.Id);
        updated.Name.Should().Be("Human Resources");
        updated.Code.Should().Be("HR");
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
