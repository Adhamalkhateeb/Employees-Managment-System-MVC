using AutoFixture;
using AutoFixture.AutoNSubstitute;
using EmployeesManager.Application.Features.LeaveApplications.Queries.GetAllLeaveApplications;
using EmployeesManager.Domain.Entities.Countries;
using EmployeesManager.Domain.Entities.Departments;
using EmployeesManager.Domain.Entities.Designations;
using EmployeesManager.Domain.Entities.Employees;
using EmployeesManager.Domain.Entities.LeaveApplications;
using EmployeesManager.Domain.Entities.LeaveApplications.Enums;
using EmployeesManager.Domain.Entities.LeaveTypes;
using EmployeesManager.Infrastructure.Data;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EmployeesManager.Tests.Features.LeaveApplications.Queries;

public sealed class GetAllLeaveApplicationsTests
{
    private readonly Fixture _fixture;

    public GetAllLeaveApplicationsTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }

    [Fact]
    public async Task Handle_WhenDataExists_ReturnsAllDtos()
    {
        await using var context = CreateContext();
        var handler = new GetAllLeaveApplicationsQueryHandler(context);
        var refs = await SeedEmployeeAndLeaveTypeAsync(context);

        await SeedLeaveAsync(
            context,
            refs.EmployeeId,
            refs.LeaveTypeId,
            DateTimeOffset.UtcNow.Date.AddDays(2)
        );
        await SeedLeaveAsync(
            context,
            refs.EmployeeId,
            refs.LeaveTypeId,
            DateTimeOffset.UtcNow.Date.AddDays(6)
        );

        var result = await handler.Handle(
            new GetAllLeaveApplicationsQuery(),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().HaveCount(2);
        result.Value.Should().OnlyContain(x => x.EmployeeId == refs.EmployeeId);
    }

    [Fact]
    public async Task Handle_WhenNoData_ReturnsEmptyList()
    {
        await using var context = CreateContext();
        var handler = new GetAllLeaveApplicationsQueryHandler(context);

        var result = await handler.Handle(
            new GetAllLeaveApplicationsQuery(),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEmpty();
    }

    private async Task SeedLeaveAsync(
        AppDbContext context,
        Guid employeeId,
        Guid leaveTypeId,
        DateTimeOffset start
    )
    {
        var leave = LeaveApplication
            .Create(
                employeeId,
                leaveTypeId,
                LeaveApplicationDurations.FullDay,
                start,
                start.AddDays(1),
                $"Query-{Math.Abs(_fixture.Create<int>())}",
                null
            )
            .Value;

        context.LeaveApplications.Add(leave);
        await context.SaveChangesAsync();
    }

    private async Task<(Guid EmployeeId, Guid LeaveTypeId)> SeedEmployeeAndLeaveTypeAsync(
        AppDbContext context
    )
    {
        var country = Country.Create("EG", "Egypt").Value;
        var department = Department.Create("Engineering", "ENG").Value;
        var designation = Designation.Create("Developer", "DEV").Value;
        var leaveType = LeaveType.Create("Annual", "ANL").Value;

        context.Countries.Add(country);
        context.Departments.Add(department);
        context.Designations.Add(designation);
        context.LeaveTypes.Add(leaveType);
        await context.SaveChangesAsync();

        var employee = Employee
            .Create(
                "John",
                null,
                "Doe",
                "01000000000",
                $"john{Math.Abs(_fixture.Create<int>())}@example.com",
                new DateTime(1995, 1, 1),
                "Cairo",
                country.Id,
                department.Id,
                designation.Id
            )
            .Value;

        context.Employees.Add(employee);
        await context.SaveChangesAsync();

        return (employee.Id, leaveType.Id);
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
