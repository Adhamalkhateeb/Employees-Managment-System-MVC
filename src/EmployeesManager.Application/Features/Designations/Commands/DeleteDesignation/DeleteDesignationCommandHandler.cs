using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Designations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Designations.Commands.DeleteDesignation;

public sealed class DeleteDesignationCommandHandler
    : IRequestHandler<DeleteDesignationCommand, Result<Deleted>>
{
    private readonly IAppDbContext _context;

    public DeleteDesignationCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Deleted>> Handle(
        DeleteDesignationCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.Designations.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return DesignationErrors.NotFound(command.Id);

        _context.Designations.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }
}
