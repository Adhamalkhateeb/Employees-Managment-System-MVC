using AutoFixture;
using AutoFixture.AutoNSubstitute;
using EmployeesManager.Application.Features.LeaveApplications.Commands.UpdateLeaveApplication;
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

public sealed class UpdateLeaveApplicationTests
{
    private readonly Fixture _fixture;

    public UpdateLeaveApplicationTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }

    [Fact]
    public async Task Handle_NonExistentId_ReturnsNotFound()
    {
        await using var context = CreateContext();
        var handler = new UpdateLeaveApplicationCommandHandler(context);
        var refs = await SeedEmployeeAndLeaveTypeAsync(context);

        var result = await handler.Handle(
            BuildValidCommand(Guid.NewGuid(), refs.EmployeeId, refs.LeaveTypeId),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Type.Should().Be(ErrorKind.NotFound);
    }

    [Fact]
    public async Task Handle_UnknownEmployee_ReturnsEmployeeRequired()
    {
        await using var context = CreateContext();
        var handler = new UpdateLeaveApplicationCommandHandler(context);
        var refs = await SeedEmployeeAndLeaveTypeAsync(context);
        var target = await SeedPendingLeaveAsync(context, refs.EmployeeId, refs.LeaveTypeId);

        var result = await handler.Handle(
            BuildValidCommand(target.Id, Guid.NewGuid(), refs.LeaveTypeId),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Code.Should().Be(LeaveApplicationErrors.EmployeeRequired.Code);
    }

    [Fact]
    public async Task Handle_UnknownLeaveType_ReturnsLeaveTypeRequired()
    {
        await using var context = CreateContext();
        var handler = new UpdateLeaveApplicationCommandHandler(context);
        var refs = await SeedEmployeeAndLeaveTypeAsync(context);
        var target = await SeedPendingLeaveAsync(context, refs.EmployeeId, refs.LeaveTypeId);

        var result = await handler.Handle(
            BuildValidCommand(target.Id, refs.EmployeeId, Guid.NewGuid()),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Code.Should().Be(LeaveApplicationErrors.LeaveTypeRequired.Code);
    }

    [Fact]
    public async Task Handle_OverlappingRange_ReturnsOverlappingLeave()
    {
        await using var context = CreateContext();
        var handler = new UpdateLeaveApplicationCommandHandler(context);
        var refs = await SeedEmployeeAndLeaveTypeAsync(context);

        var target = await SeedPendingLeaveAsync(
            context,
            refs.EmployeeId,
            refs.LeaveTypeId,
            DateTimeOffset.UtcNow.Date.AddDays(6),
            DateTimeOffset.UtcNow.Date.AddDays(7)
        );

        await SeedPendingLeaveAsync(
            context,
            refs.EmployeeId,
            refs.LeaveTypeId,
            DateTimeOffset.UtcNow.Date.AddDays(8),
            DateTimeOffset.UtcNow.Date.AddDays(10)
        );

        var command = new UpdateLeaveApplicationCommand(
            target.Id,
            refs.EmployeeId,
            refs.LeaveTypeId,
            LeaveApplicationDurations.FullDay,
            LeaveApplicationStatus.Pending,
            DateTimeOffset.UtcNow.Date.AddDays(9),
            DateTimeOffset.UtcNow.Date.AddDays(11),
            "Overlapping update",
            null
        );

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Code.Should().Be(LeaveApplicationErrors.OverlappingLeave.Code);
    }

    [Fact]
    public async Task Handle_NotPendingEntity_ReturnsNotEditable()
    {
        await using var context = CreateContext();
        var handler = new UpdateLeaveApplicationCommandHandler(context);
        var refs = await SeedEmployeeAndLeaveTypeAsync(context);
        var target = await SeedPendingLeaveAsync(context, refs.EmployeeId, refs.LeaveTypeId);

        target.Approve();
        await context.SaveChangesAsync();

        var result = await handler.Handle(
            BuildValidCommand(target.Id, refs.EmployeeId, refs.LeaveTypeId),
            CancellationToken.None
        );

        result.IsSuccess.Should().BeFalse();
        result.TopError.Code.Should().Be(LeaveApplicationErrors.NotEditable.Code);
    }

    [Fact]
    public async Task Handle_DomainValidationFailure_ReturnsDescriptionRequired()
    {
        await using var context = CreateContext();
        var handler = new UpdateLeaveApplicationCommandHandler(context);
        var refs = await SeedEmployeeAndLeaveTypeAsync(context);
        var target = await SeedPendingLeaveAsync(context, refs.EmployeeId, refs.LeaveTypeId);

        var command = new UpdateLeaveApplicationCommand(
            target.Id,
            refs.EmployeeId,
            refs.LeaveTypeId,
            LeaveApplicationDurations.FullDay,
            LeaveApplicationStatus.Pending,
            DateTimeOffset.UtcNow.Date.AddDays(6),
            DateTimeOffset.UtcNow.Date.AddDays(8),
            string.Empty,
            null
        );

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Code.Should().Be(LeaveApplicationErrors.DescriptionRequired.Code);
    }

    [Fact]
    public async Task Handle_ValidCommand_UpdatesEntity()
    {
        await using var context = CreateContext();
        var handler = new UpdateLeaveApplicationCommandHandler(context);
        var refs = await SeedEmployeeAndLeaveTypeAsync(context);
        var target = await SeedPendingLeaveAsync(context, refs.EmployeeId, refs.LeaveTypeId);

        var startDate = DateTimeOffset.UtcNow.Date.AddDays(10);
        var endDate = startDate.AddDays(2);
        var command = new UpdateLeaveApplicationCommand(
            target.Id,
            refs.EmployeeId,
            refs.LeaveTypeId,
            LeaveApplicationDurations.FirstHalf,
            LeaveApplicationStatus.Pending,
            startDate,
            endDate,
            "Updated reason",
            null
        );

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();

        var updated = await context.LeaveApplications.FirstAsync(x => x.Id == target.Id);
        updated.Duration.Should().Be(LeaveApplicationDurations.FirstHalf);
        updated.StartDate.Should().Be(startDate);
        updated.EndDate.Should().Be(endDate);
        updated.Description.Should().Be("Updated reason");
    }

    private UpdateLeaveApplicationCommand BuildValidCommand(
        Guid id,
        Guid employeeId,
        Guid leaveTypeId
    )
    {
        var startDate = DateTimeOffset.UtcNow.Date.AddDays(6);

        return new UpdateLeaveApplicationCommand(
            id,
            employeeId,
            leaveTypeId,
            LeaveApplicationDurations.FullDay,
            LeaveApplicationStatus.Pending,
            startDate,
            startDate.AddDays(1),
            $"Update-{Math.Abs(_fixture.Create<int>())}",
            null
        );
    }

    private async Task<LeaveApplication> SeedPendingLeaveAsync(
        AppDbContext context,
        Guid employeeId,
        Guid leaveTypeId,
        DateTimeOffset? startDate = null,
        DateTimeOffset? endDate = null
    )
    {
        var start = startDate ?? DateTimeOffset.UtcNow.Date.AddDays(3);
        var end = endDate ?? start.AddDays(1);

        var leave = LeaveApplication
            .Create(
                employeeId,
                leaveTypeId,
                LeaveApplicationDurations.FullDay,
                start,
                end,
                "Initial request",
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
