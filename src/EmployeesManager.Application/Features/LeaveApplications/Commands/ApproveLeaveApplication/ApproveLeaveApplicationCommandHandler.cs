using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.ApproveLeaveApplication;

public sealed class ApproveLeaveApplicationCommandHandler
    : IRequestHandler<ApproveLeaveApplicationCommand, Result<Success>>
{
    private readonly IAppDbContext _context;

    public ApproveLeaveApplicationCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Success>> Handle(
        ApproveLeaveApplicationCommand command,
        CancellationToken cancellationToken
    )
    {
        var leaveApplication = await _context.LeaveApplications.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (leaveApplication is null)
            return LeaveApplicationErrors.NotFound(command.Id);

        var result = leaveApplication.Approve();

        if (result.IsError)
            return result.Errors;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success;
    }
}
