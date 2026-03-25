using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveApplications.Common;
using EmployeesManager.Application.Features.LeaveApplications.Dtos;
using EmployeesManager.Application.Features.LeaveApplications.Mappings;
using EmployeesManager.Application.Features.SystemCodeDetails.Common;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.LeaveApplications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.CreateLeaveApplication;

public sealed class CreateLeaveApplicationCommandHandler
    : IRequestHandler<CreateLeaveApplicationCommand, Result<LeaveApplicationDto>>
{
    private readonly IAppDbContext _context;

    public CreateLeaveApplicationCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<LeaveApplicationDto>> Handle(
        CreateLeaveApplicationCommand command,
        CancellationToken cancellationToken
    )
    {
        var employeeExists = await _context.Employees.AnyAsync(
            x => x.Id == command.EmployeeId,
            cancellationToken
        );

        if (!employeeExists)
            return LeaveApplicationErrors.EmployeeRequired;

        var leaveTypeExists = await _context.LeaveTypes.AnyAsync(
            x => x.Id == command.LeaveTypeId,
            cancellationToken
        );

        if (!leaveTypeExists)
            return LeaveApplicationErrors.LeaveTypeRequired;

        var durationExists = await _context.SystemCodeDetails.AnyAsync(
            x =>
                x.Id == command.DurationId
                && x.SystemCode.Code == SystemCodeLookUpConstants.LeaveDurationSystemCode,
            cancellationToken
        );

        if (!durationExists)
            return LeaveApplicationErrors.DurationRequired;

        var statusExists = await _context.SystemCodeDetails.AnyAsync(
            x =>
                x.Id == command.StatusId
                && x.SystemCode.Code == SystemCodeLookUpConstants.LeaveApplicationStatusSystemCode,
            cancellationToken
        );

        if (!statusExists)
            return LeaveApplicationErrors.StatusRequired;

        var createResult = LeaveApplication.Create(
            command.EmployeeId,
            command.LeaveTypeId,
            command.DurationId,
            command.StatusId,
            command.StartDate,
            command.EndDate,
            command.Description,
            command.Attachment
        );

        if (createResult.IsError)
            return createResult.Errors;

        var entity = createResult.Value;
        _context.LeaveApplications.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        var dto = await _context
            .LeaveApplications.AsNoTracking()
            .Where(x => x.Id == entity.Id)
            .Select(x => new LeaveApplicationDto(
                x.Id,
                x.EmployeeId,
                x.Employee.FirstName + " " + x.Employee.LastName,
                x.LeaveTypeId,
                x.LeaveType.Name,
                x.DurationId,
                x.Duration.Description ?? x.Duration.Code,
                x.StatusId,
                x.Status.Description ?? x.Status.Code,
                x.StartDate,
                x.EndDate,
                x.Days,
                x.Description,
                x.Attachment,
                x.ApprovedBy,
                x.ApprovedAtUtc
            ))
            .FirstAsync(cancellationToken);

        return dto;
    }
}
