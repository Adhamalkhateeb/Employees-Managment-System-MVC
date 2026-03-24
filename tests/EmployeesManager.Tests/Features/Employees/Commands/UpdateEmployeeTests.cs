using EmployeesManager.Application.Features.Employees.Commands.UpdateEmployee;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Employees;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.Employees.Commands;

public sealed class UpdateEmployeeTests
{
    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        await using var context = CreateContext();
        var handler = new UpdateEmployeeCommandHandler(context);

        var result = await handler.Handle(CreateCommand(Guid.NewGuid()), CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_UpdatesEntity()
    {
        await using var context = CreateContext();
        var entityResult = Employee.Create(
            "Adham",
            null,
            "Yasser",
            "01000000000",
            "adham@example.com",
            "Egypt",
            new DateTime(1995, 5, 10),
            "Old Address",
            "Engineering",
            "Developer"
        );

        var entity = entityResult.Value;
        context.Employees.Add(entity);
        await context.SaveChangesAsync();

        var handler = new UpdateEmployeeCommandHandler(context);
        var result = await handler.Handle(
            CreateCommand(entity.Id, "Senior Developer", "New Address"),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeTrue();

        var updated = await context.Employees.FirstAsync(x => x.Id == entity.Id);
        updated.Designation.Should().Be("Senior Developer");
        updated.Address.Should().Be("New Address");
    }

    private static UpdateEmployeeCommand CreateCommand(
        Guid id,
        string designation = "Developer",
        string address = "Cairo"
    ) =>
        new(
            Id: id,
            FirstName: "Adham",
            MiddleName: "M",
            LastName: "Yasser",
            PhoneNumber: "01000000000",
            EmailAddress: "adham@example.com",
            Country: "Egypt",
            DateOfBirth: new DateTime(1995, 5, 10),
            Address: address,
            Department: "Engineering",
            Designation: designation
        );

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
