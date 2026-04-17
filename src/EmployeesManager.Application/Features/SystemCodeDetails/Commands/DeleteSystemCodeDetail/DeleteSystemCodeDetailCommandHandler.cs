using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.SystemCodeDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Commands.DeleteSystemCodeDetail;

public sealed class DeleteSystemCodeDetailCommandHandler
    : IRequestHandler<DeleteSystemCodeDetailCommand, Result<Deleted>>
{
    private readonly IAppDbContext _context;

    public DeleteSystemCodeDetailCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Deleted>> Handle(
        DeleteSystemCodeDetailCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.SystemCodeDetails.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return SystemCodeDetailErrors.NotFound(command.Id);

        _context.SystemCodeDetails.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }
}
