using EmployeesManager.Application.Features.Departments.Commands.DeleteDepartment;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Departments;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.Departments.Commands;

public sealed class DeleteDepartmentTests
{
    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        await using var context = CreateContext();
        var handler = new DeleteDepartmentCommandHandler(context);

        var result = await handler.Handle(
            new DeleteDepartmentCommand(Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_CallsRemoveAndSave()
    {
        await using var context = CreateContext();
        var entity = Department.Create("Engineering", "ENG").Value;
        context.Departments.Add(entity);
        await context.SaveChangesAsync();

        var handler = new DeleteDepartmentCommandHandler(context);
        var result = await handler.Handle(
            new DeleteDepartmentCommand(entity.Id),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeTrue();
        context.Departments.Count().Should().Be(0);
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
