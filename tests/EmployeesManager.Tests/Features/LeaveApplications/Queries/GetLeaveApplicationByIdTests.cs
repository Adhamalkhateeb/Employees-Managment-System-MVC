using AutoFixture;
using AutoFixture.AutoNSubstitute;
using EmployeesManager.Application.Features.LeaveApplications.Queries.GetLeaveApplicationById;
using EmployeesManager.Domain.Common.Results;
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

public sealed class GetLeaveApplicationByIdTests
{
    private readonly Fixture _fixture;

    public GetLeaveApplicationByIdTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        await using var context = CreateContext();
        var handler = new GetLeaveApplicationByIdQueryHandler(context);

        var result = await handler.Handle(
            new GetLeaveApplicationByIdQuery(Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_ExistingId_ReturnsDtoWithMatchingId()
    {
        await using var context = CreateContext();
        var handler = new GetLeaveApplicationByIdQueryHandler(context);
        var refs = await SeedEmployeeAndLeaveTypeAsync(context);

        var leave = LeaveApplication
            .Create(
                refs.EmployeeId,
                refs.LeaveTypeId,
                LeaveApplicationDurations.FullDay,
                DateTimeOffset.UtcNow.Date.AddDays(3),
                DateTimeOffset.UtcNow.Date.AddDays(4),
                $"ById-{Math.Abs(_fixture.Create<int>())}",
                null
            )
            .Value;

        context.LeaveApplications.Add(leave);
        await context.SaveChangesAsync();

        var result = await handler.Handle(
            new GetLeaveApplicationByIdQuery(leave.Id),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(leave.Id);
        result.Value.EmployeeId.Should().Be(refs.EmployeeId);
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
