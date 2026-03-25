using EmployeesManager.Application.Features.Employees.Commands.CreateEmployee;
using EmployeesManager.Domain.Entities.Countries;
using EmployeesManager.Domain.Entities.Departments;
using EmployeesManager.Domain.Entities.Designations;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace EmployeesManager.Tests.Features.Employees.Commands;

public sealed class CreateEmployeeTests
{
    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        await using var context = CreateContext();
        var references = await SeedReferencesAsync(context);
        var handler = new CreateEmployeeCommandHandler(
            context,
            NullLogger<CreateEmployeeCommandHandler>.Instance
        );

        var command = new CreateEmployeeCommand(
            FirstName: "Adham",
            MiddleName: "M",
            LastName: "Yasser",
            PhoneNumber: "01000000000",
            EmailAddress: "adham@example.com",
            DateOfBirth: new DateTime(1995, 5, 10),
            Address: "Cairo",
            CountryId: references.CountryId,
            DepartmentId: references.DepartmentId,
            DesignationId: references.DesignationId
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
            DateOfBirth: DateTime.UtcNow.AddDays(1),
            Address: string.Empty,
            CountryId: Guid.Empty,
            DepartmentId: Guid.Empty,
            DesignationId: Guid.Empty
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

    private static async Task<(
        Guid CountryId,
        Guid DepartmentId,
        Guid DesignationId
    )> SeedReferencesAsync(AppDbContext context)
    {
        var country = Country.Create("EG", "Egypt").Value;
        var department = Department.Create("Engineering", "ENG").Value;
        var designation = Designation.Create("Developer", "DEV").Value;

        context.Countries.Add(country);
        context.Departments.Add(department);
        context.Designations.Add(designation);
        await context.SaveChangesAsync();

        return (country.Id, department.Id, designation.Id);
    }
}
