using EmployeesManager.Application.Features.Employees.Commands.UpdateEmployee;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Countries;
using EmployeesManager.Domain.Entities.Departments;
using EmployeesManager.Domain.Entities.Designations;
using EmployeesManager.Domain.Entities.Employees;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace EmployeesManager.Tests.Features.Employees.Commands;

public sealed class UpdateEmployeeTests
{
    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        await using var context = CreateContext();
        var references = await SeedReferencesAsync(context);
        var handler = new UpdateEmployeeCommandHandler(
            context,
            NullLogger<UpdateEmployeeCommandHandler>.Instance
        );

        var result = await handler.Handle(
            CreateCommand(
                Guid.NewGuid(),
                references.CountryId,
                references.DepartmentId,
                references.DesignationId
            ),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_UpdatesEntity()
    {
        await using var context = CreateContext();
        var references = await SeedReferencesAsync(context);
        var newDesignation = Designation.Create("Senior Developer", "SD").Value;
        context.Designations.Add(newDesignation);
        await context.SaveChangesAsync();

        var entityResult = Employee.Create(
            "Adham",
            null,
            "Yasser",
            "01000000000",
            "adham@example.com",
            new DateTime(1995, 5, 10),
            "Old Address",
            references.CountryId,
            references.DepartmentId,
            references.DesignationId
        );

        var entity = entityResult.Value;
        context.Employees.Add(entity);
        await context.SaveChangesAsync();

        var handler = new UpdateEmployeeCommandHandler(
            context,
            NullLogger<UpdateEmployeeCommandHandler>.Instance
        );
        var result = await handler.Handle(
            CreateCommand(
                entity.Id,
                references.CountryId,
                references.DepartmentId,
                newDesignation.Id,
                "New Address"
            ),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeTrue();

        var updated = await context.Employees.FirstAsync(x => x.Id == entity.Id);
        updated.DesignationId.Should().Be(newDesignation.Id);
        updated.Address.Should().Be("New Address");
    }

    private static UpdateEmployeeCommand CreateCommand(
        Guid id,
        Guid countryId,
        Guid departmentId,
        Guid designationId,
        string address = "Cairo"
    ) =>
        new(
            Id: id,
            FirstName: "Adham",
            MiddleName: "M",
            LastName: "Yasser",
            PhoneNumber: "01000000000",
            EmailAddress: "adham@example.com",
            DateOfBirth: new DateTime(1995, 5, 10),
            Address: address,
            CountryId: countryId,
            DepartmentId: departmentId,
            DesignationId: designationId
        );

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
