using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.LeaveTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.LeaveTypes.Commands.DeleteLeaveType;

public sealed class DeleteLeaveTypeCommandHandler
    : IRequestHandler<DeleteLeaveTypeCommand, Result<Deleted>>
{
    private readonly IAppDbContext _context;

    public DeleteLeaveTypeCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Deleted>> Handle(
        DeleteLeaveTypeCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.LeaveTypes.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return LeaveTypeErrors.NotFound(command.Id);

        _context.LeaveTypes.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }
}
