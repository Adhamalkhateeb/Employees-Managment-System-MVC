using EmployeesManager.Application.Features.Employees.Queries.GetEmployeeById;
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

namespace EmployeesManager.Tests.Features.Employees.Queries;

public sealed class GetEmployeeByIdTests
{
    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        await using var context = CreateContext();
        var handler = new GetEmployeeByIdQueryHandler(
            context,
            NullLogger<GetEmployeeByIdQueryHandler>.Instance
        );

        var result = await handler.Handle(
            new GetEmployeeByIdQuery(Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_ReturnsDtoWithMatchingId()
    {
        await using var context = CreateContext();
        var references = await SeedReferencesAsync(context);
        var entity = Employee
            .Create(
                "Adham",
                null,
                "Yasser",
                "01000000000",
                "adham@example.com",
                new DateTime(1995, 5, 10),
                "Cairo",
                references.CountryId,
                references.DepartmentId,
                references.DesignationId
            )
            .Value;

        context.Employees.Add(entity);
        await context.SaveChangesAsync();

        var handler = new GetEmployeeByIdQueryHandler(
            context,
            NullLogger<GetEmployeeByIdQueryHandler>.Instance
        );

        var result = await handler.Handle(
            new GetEmployeeByIdQuery(entity.Id),
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

    private static async Task<(
        Guid CountryId,
        Guid DepartmentId,
        Guid DesignationId
    )> SeedReferencesAsync(AppDbContext context)
    {
        var country = Country.Create("EG", "Egypt").Value;
        var department = Department.Create("Engineering", "ENG").Value;
        var designation = Designation.Create("Developer").Value;

        context.Countries.Add(country);
        context.Departments.Add(department);
        context.Designations.Add(designation);
        await context.SaveChangesAsync();

        return (country.Id, department.Id, designation.Id);
    }
}
