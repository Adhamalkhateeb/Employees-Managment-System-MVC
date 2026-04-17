using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveApplications.Common;
using EmployeesManager.Application.Features.SystemCodeDetails.Common;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.LeaveApplications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.CreateLeaveApplication;

public sealed class CreateLeaveApplicationCommandHandler
    : IRequestHandler<CreateLeaveApplicationCommand, Result<Created>>
{
    private readonly IAppDbContext _context;

    public CreateLeaveApplicationCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Created>> Handle(
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

        var hasOverlappingLeave = await _context.LeaveApplications.AnyAsync(
            x =>
                x.EmployeeId == command.EmployeeId
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

        var createResult = LeaveApplication.Create(
            command.EmployeeId,
            command.LeaveTypeId,
            command.Duration,
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

        return Result.Created;
    }
}
