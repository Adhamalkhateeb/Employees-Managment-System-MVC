using EmployeesManager.Application.Features.Employees.Commands.CreateEmployee;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.Employees.Commands;

public sealed class CreateEmployeeTests
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        await using var context = CreateContext();
        var handler = new CreateEmployeeCommandHandler(context);

        var command = new CreateEmployeeCommand(
            FirstName: "Adham",
            MiddleName: "M",
            LastName: "Yasser",
            PhoneNumber: "01000000000",
            EmailAddress: "adham@example.com",
            Country: "Egypt",
            DateOfBirth: new DateTime(1995, 5, 10),
            Address: "Cairo",
            Department: "Engineering",
            Designation: "Developer"
        );

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();
        context.Employees.Count().Should().Be(1);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ValidatorFails()
    {
        var validator = new CreateEmployeeCommandValidator();
        var command = new CreateEmployeeCommand(
            FirstName: string.Empty,
            MiddleName: null,
            LastName: string.Empty,
            PhoneNumber: string.Empty,
            EmailAddress: "invalid-email",
            Country: string.Empty,
            DateOfBirth: DateTime.UtcNow.AddDays(1),
            Address: string.Empty,
            Department: string.Empty,
            Designation: string.Empty
        );

        var validation = await validator.ValidateAsync(command);

        validation.IsValid.Should().BeFalse();
        validation.Errors.Should().NotBeEmpty();
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
