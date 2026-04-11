using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.CancelLeaveApplication;

public sealed class CancelLeaveApplicationCommandHandler
    : IRequestHandler<CancelLeaveApplicationCommand, Result<Success>>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUser _currentUser;

    public CancelLeaveApplicationCommandHandler(IAppDbContext context, ICurrentUser currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Result<Success>> Handle(
        CancelLeaveApplicationCommand command,
        CancellationToken cancellationToken
    )
    {
        var leaveApplication = await _context.LeaveApplications.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (leaveApplication is null)
            return LeaveApplicationErrors.NotFound(command.Id);

        var result = leaveApplication.Cancel(_currentUser.Id);

        if (result.IsError)
            return result.Errors;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
