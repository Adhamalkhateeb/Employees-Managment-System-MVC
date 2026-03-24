using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.SystemCodes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.SystemCodes.Commands.UpdateSystemCode;

public sealed class UpdateSystemCodeCommandHandler
    : IRequestHandler<UpdateSystemCodeCommand, Result<Updated>>
{
    private readonly IAppDbContext _context;

    public UpdateSystemCodeCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Updated>> Handle(
        UpdateSystemCodeCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.SystemCodes.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return SystemCodeErrors.NotFound(command.Id);

        var nameExists = await _context.SystemCodes.AnyAsync(
            x => x.Name == command.Name && x.Id != command.Id,
            cancellationToken
        );

        if (nameExists)
            return SystemCodeErrors.NameAlreadyExists;

        var codeExists = await _context.SystemCodes.AnyAsync(
            x => x.Code == command.Code && x.Id != command.Id,
            cancellationToken
        );

        if (codeExists)
            return SystemCodeErrors.CodeAlreadyExists;

        var updateResult = entity.Update(command.Name, command.Code);

        if (updateResult.IsError)
            return updateResult.Errors;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }
}
