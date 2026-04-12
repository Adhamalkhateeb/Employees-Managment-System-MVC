using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Branches;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Branches.Commands.DeleteBranch;

public sealed class DeleteBranchCommandHandler
    : IRequestHandler<DeleteBranchCommand, Result<Deleted>>
{
    private readonly IAppDbContext _context;

    public DeleteBranchCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Deleted>> Handle(
        DeleteBranchCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.Branches.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return BranchErrors.NotFound(command.Id);

        _context.Branches.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }
}
