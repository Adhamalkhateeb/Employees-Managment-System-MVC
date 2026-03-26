using AutoFixture;
using AutoFixture.AutoNSubstitute;
using EmployeesManager.Application.Features.LeaveApplications.Commands.CreateLeaveApplication;
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

public sealed class CreateLeaveApplicationTests
{
    private readonly Fixture _fixture;

    public CreateLeaveApplicationTests()
    {
        _fixture = new Fixture();
        _fixture.Customize(new AutoNSubstituteCustomization());
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        await using var context = CreateContext();
        var handler = new CreateLeaveApplicationCommandHandler(context);
        var refs = await SeedEmployeeAndLeaveTypeAsync(context);

        var command = BuildValidCommand(refs.EmployeeId, refs.LeaveTypeId);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        context.LeaveApplications.Count().Should().Be(1);
    }

    [Fact]
    public async Task Handle_UnknownEmployee_ReturnsEmployeeRequired()
    {
        await using var context = CreateContext();
        var handler = new CreateLeaveApplicationCommandHandler(context);

        var leaveType = LeaveType.Create("Annual", "ANL").Value;
        context.LeaveTypes.Add(leaveType);
        await context.SaveChangesAsync();

        var command = BuildValidCommand(Guid.NewGuid(), leaveType.Id);

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Code.Should().Be(LeaveApplicationErrors.EmployeeRequired.Code);
    }

    [Fact]
    public async Task Handle_UnknownLeaveType_ReturnsLeaveTypeRequired()
    {
        await using var context = CreateContext();
        var handler = new CreateLeaveApplicationCommandHandler(context);

        var employeeId = await SeedEmployeeOnlyAsync(context);
        var command = BuildValidCommand(employeeId, Guid.NewGuid());

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Code.Should().Be(LeaveApplicationErrors.LeaveTypeRequired.Code);
    }

    [Fact]
    public async Task Handle_OverlappingRange_ReturnsOverlappingLeave()
    {
        await using var context = CreateContext();
        var handler = new CreateLeaveApplicationCommandHandler(context);
        var refs = await SeedEmployeeAndLeaveTypeAsync(context);

        var startDate = DateTimeOffset.UtcNow.Date.AddDays(2);
        var existing = LeaveApplication
            .Create(
                refs.EmployeeId,
                refs.LeaveTypeId,
                LeaveApplicationDurations.FullDay,
                startDate,
                startDate.AddDays(2),
                "Existing request",
                null
            )
            .Value;

        context.LeaveApplications.Add(existing);
        await context.SaveChangesAsync();

        var command = new CreateLeaveApplicationCommand(
            refs.EmployeeId,
            refs.LeaveTypeId,
            LeaveApplicationDurations.FirstHalf,
            startDate.AddDays(1),
            startDate.AddDays(3),
            "New request",
            null
        );

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Code.Should().Be(LeaveApplicationErrors.OverlappingLeave.Code);
    }

    [Fact]
    public async Task Handle_DomainValidationFailure_ReturnsStartDateInPast()
    {
        await using var context = CreateContext();
        var handler = new CreateLeaveApplicationCommandHandler(context);
        var refs = await SeedEmployeeAndLeaveTypeAsync(context);

        var command = new CreateLeaveApplicationCommand(
            refs.EmployeeId,
            refs.LeaveTypeId,
            LeaveApplicationDurations.FullDay,
            DateTimeOffset.UtcNow.Date.AddDays(-1),
            DateTimeOffset.UtcNow.Date.AddDays(1),
            "Annual leave",
            null
        );

        var result = await handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.TopError.Code.Should().Be(LeaveApplicationErrors.StartDateInPast.Code);
    }

    [Fact]
    public async Task Handle_InvalidCommand_ValidatorFails()
    {
        var validator = new CreateLeaveApplicationCommandValidator();
        var command = new CreateLeaveApplicationCommand(
            Guid.Empty,
            Guid.Empty,
            (LeaveApplicationDurations)999,
            DateTimeOffset.MinValue,
            DateTimeOffset.MinValue,
            string.Empty,
            new string('a', LeaveApplicationConstants.AttachmentMaxLength + 1)
        );

        var validation = await validator.ValidateAsync(command);

        validation.IsValid.Should().BeFalse();
        validation.Errors.Should().NotBeEmpty();
    }

    private CreateLeaveApplicationCommand BuildValidCommand(Guid employeeId, Guid leaveTypeId)
    {
        var startDate = DateTimeOffset.UtcNow.Date.AddDays(3);
        var endDate = startDate.AddDays(2);

        return new CreateLeaveApplicationCommand(
            employeeId,
            leaveTypeId,
            LeaveApplicationDurations.FullDay,
            startDate,
            endDate,
            $"Leave-{Math.Abs(_fixture.Create<int>())}",
            null
        );
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

    private async Task<Guid> SeedEmployeeOnlyAsync(AppDbContext context)
    {
        var country = Country.Create("EG", "Egypt").Value;
        var department = Department.Create("Engineering", "ENG").Value;
        var designation = Designation.Create("Developer", "DEV").Value;

        context.Countries.Add(country);
        context.Departments.Add(department);
        context.Designations.Add(designation);
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

        return employee.Id;
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }
}
