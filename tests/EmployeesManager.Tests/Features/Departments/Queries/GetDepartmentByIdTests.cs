using EmployeesManager.Application.Features.Departments.Queries.GetDepartmentById;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Departments;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.Departments.Queries;

public sealed class GetDepartmentByIdTests
{
    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        await using var context = CreateContext();
        var handler = new GetDepartmentByIdQueryHandler(context);

        var result = await handler.Handle(
            new GetDepartmentByIdQuery(Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_ReturnsDtoWithMatchingId()
    {
        await using var context = CreateContext();
        var entity = Department.Create("Engineering", "ENG").Value;
        context.Departments.Add(entity);
        await context.SaveChangesAsync();

        var handler = new GetDepartmentByIdQueryHandler(context);

        var result = await handler.Handle(
            new GetDepartmentByIdQuery(entity.Id),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(entity.Id);
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
