using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.LeaveTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.LeaveTypes.Commands.UpdateLeaveType;

public sealed class UpdateLeaveTypeCommandHandler
    : IRequestHandler<UpdateLeaveTypeCommand, Result<Updated>>
{
    private readonly IAppDbContext _context;

    public UpdateLeaveTypeCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Updated>> Handle(
        UpdateLeaveTypeCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.LeaveTypes.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return LeaveTypeErrors.NotFound(command.Id);

        var nameExists = await _context.LeaveTypes.AnyAsync(
            x => x.Name == command.Name && x.Id != command.Id,
            cancellationToken
        );

        if (nameExists)
            return LeaveTypeErrors.NameAlreadyExists;

        var codeExists = await _context.LeaveTypes.AnyAsync(
            x => x.Code == command.Code && x.Id != command.Id,
            cancellationToken
        );

        if (codeExists)
            return LeaveTypeErrors.CodeAlreadyExists;

        var updateResult = entity.Update(command.Name, command.Code);

        if (updateResult.IsError)
            return updateResult.Errors;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }
}
