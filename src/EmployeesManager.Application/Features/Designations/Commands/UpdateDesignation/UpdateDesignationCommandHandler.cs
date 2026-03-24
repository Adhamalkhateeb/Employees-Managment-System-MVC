using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Designations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Designations.Commands.UpdateDesignation;

public sealed class UpdateDesignationCommandHandler
    : IRequestHandler<UpdateDesignationCommand, Result<Updated>>
{
    private readonly IAppDbContext _context;

    public UpdateDesignationCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Updated>> Handle(
        UpdateDesignationCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.Designations.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return DesignationErrors.NotFound(command.Id);

        var nameExists = await _context.Designations.AnyAsync(
            x => x.Name == command.Name && x.Id != command.Id,
            cancellationToken
        );

        if (nameExists)
            return DesignationErrors.NameAlreadyExists;

        var updateResult = entity.Update(command.Name);

        if (updateResult.IsError)
            return updateResult.Errors;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }
}
