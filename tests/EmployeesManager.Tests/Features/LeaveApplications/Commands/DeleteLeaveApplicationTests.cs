using AutoFixture;
using AutoFixture.AutoNSubstitute;
using EmployeesManager.Application.Features.LeaveApplications.Commands.DeleteLeaveApplication;
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

namespace EmployeesManager.Tests.Features.LeaveApplications.Commands;

public sealed class DeleteLeaveApplicationTests
{
    private readonly Fixture _fixture;

    public DeleteLeaveApplicationTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        await using var context = CreateContext();
        var handler = new DeleteLeaveApplicationCommandHandler(context);

        var result = await handler.Handle(
            new DeleteLeaveApplicationCommand(Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_NonPendingEntity_ReturnsNotEditable()
    {
        await using var context = CreateContext();
        var handler = new DeleteLeaveApplicationCommandHandler(context);
        var refs = await SeedEmployeeAndLeaveTypeAsync(context);
        var leave = await SeedPendingLeaveAsync(context, refs.EmployeeId, refs.LeaveTypeId);

        leave.Approve();
        await context.SaveChangesAsync();

        var result = await handler.Handle(
            new DeleteLeaveApplicationCommand(leave.Id),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Code.Should().Be(LeaveApplicationErrors.NotEditable.Code);
    }

    [Fact]
    public async Task Handle_PendingEntity_RemovesEntity()
    {
        await using var context = CreateContext();
        var handler = new DeleteLeaveApplicationCommandHandler(context);
        var refs = await SeedEmployeeAndLeaveTypeAsync(context);
        var leave = await SeedPendingLeaveAsync(context, refs.EmployeeId, refs.LeaveTypeId);

        var result = await handler.Handle(
            new DeleteLeaveApplicationCommand(leave.Id),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeTrue();
        context.LeaveApplications.Count().Should().Be(0);
    }

    private async Task<LeaveApplication> SeedPendingLeaveAsync(
        AppDbContext context,
        Guid employeeId,
        Guid leaveTypeId
    )
    {
        var startDate = DateTimeOffset.UtcNow.Date.AddDays(3);
        var leave = LeaveApplication
            .Create(
                employeeId,
                leaveTypeId,
                LeaveApplicationDurations.FullDay,
                startDate,
                startDate.AddDays(1),
                "Delete candidate",
                null
            )
            .Value;

        context.LeaveApplications.Add(leave);
        await context.SaveChangesAsync();

        return leave;
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
