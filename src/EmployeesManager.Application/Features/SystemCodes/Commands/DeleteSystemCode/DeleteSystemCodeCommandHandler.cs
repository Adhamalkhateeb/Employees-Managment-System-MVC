using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.SystemCodes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.SystemCodes.Commands.DeleteSystemCode;

public sealed class DeleteSystemCodeCommandHandler
    : IRequestHandler<DeleteSystemCodeCommand, Result<Deleted>>
{
    private readonly IAppDbContext _context;

    public DeleteSystemCodeCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Deleted>> Handle(
        DeleteSystemCodeCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.SystemCodes.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return SystemCodeErrors.NotFound(command.Id);

        _context.SystemCodes.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }
}
