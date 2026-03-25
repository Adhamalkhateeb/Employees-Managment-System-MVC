using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.LeaveApplications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.DeleteLeaveApplication;

public sealed class DeleteLeaveApplicationCommandHandler
    : IRequestHandler<DeleteLeaveApplicationCommand, Result<Deleted>>
{
    private readonly IAppDbContext _context;

    public DeleteLeaveApplicationCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Deleted>> Handle(
        DeleteLeaveApplicationCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.LeaveApplications.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return LeaveApplicationErrors.NotFound(command.Id);

        _context.LeaveApplications.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }
}
