using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.RejectLeaveApplication;

public sealed class RejectLeaveApplicationCommandHandler
    : IRequestHandler<RejectLeaveApplicationCommand, Result<Success>>
{
    private readonly IAppDbContext _context;

    public RejectLeaveApplicationCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Success>> Handle(
        RejectLeaveApplicationCommand command,
        CancellationToken cancellationToken
    )
    {
        var leaveApplication = await _context.LeaveApplications.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (leaveApplication is null)
            return LeaveApplicationErrors.NotFound(command.Id);

        var result = leaveApplication.Reject(command.RejectionReason);

        if (result.IsError)
            return result.Errors;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
