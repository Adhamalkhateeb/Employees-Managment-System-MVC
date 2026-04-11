using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.RejectLeaveApplication;

public sealed class RejectLeaveApplicationCommandHandler
    : IRequestHandler<RejectLeaveApplicationCommand, Result<Success>>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public RejectLeaveApplicationCommandHandler(IAppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

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

        var result = leaveApplication.Reject(_currentUser.Id, command.RejectionReason);

        if (result.IsError)
            return result.Errors;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
