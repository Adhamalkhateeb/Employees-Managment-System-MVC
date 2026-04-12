using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
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

        var hasOverlappingLeave = await _context.LeaveApplications.AnyAsync(
            x =>
                x.Id != command.Id
                && x.EmployeeId == command.EmployeeId
                && (
                    x.Status == LeaveApplicationStatus.Pending
                    || x.Status == LeaveApplicationStatus.Approved
                )
                && x.StartDate.Date <= command.EndDate.Date
                && command.StartDate.Date <= x.EndDate.Date,
            cancellationToken
        );

        if (hasOverlappingLeave)
            return LeaveApplicationErrors.OverlappingLeave;

        var updateResult = entity.Update(
            command.EmployeeId,
            command.LeaveTypeId,
            command.Duration,
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
