using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveApplications.Common;
using EmployeesManager.Application.Features.SystemCodeDetails.Common;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.LeaveApplications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.UpdateLeaveApplication;

public sealed class UpdateLeaveApplicationCommandHandler
    : IRequestHandler<UpdateLeaveApplicationCommand, Result<Updated>>
{
    private readonly IAppDbContext _context;

    public UpdateLeaveApplicationCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Updated>> Handle(
        UpdateLeaveApplicationCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.LeaveApplications.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return LeaveApplicationErrors.NotFound(command.Id);

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

        var updateResult = entity.Update(
            command.EmployeeId,
            command.LeaveTypeId,
            command.DurationId,
            command.StatusId,
            command.StartDate,
            command.EndDate,
            command.Description,
            command.Attachment
        );

        if (updateResult.IsError)
            return updateResult.Errors;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }
}
