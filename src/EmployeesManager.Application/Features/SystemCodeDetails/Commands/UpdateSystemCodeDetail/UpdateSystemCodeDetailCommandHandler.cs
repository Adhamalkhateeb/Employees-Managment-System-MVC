using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.SystemCodeDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Commands.UpdateSystemCodeDetail;

public sealed class UpdateSystemCodeDetailCommandHandler
    : IRequestHandler<UpdateSystemCodeDetailCommand, Result<Updated>>
{
    private readonly IAppDbContext _context;

    public UpdateSystemCodeDetailCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Updated>> Handle(
        UpdateSystemCodeDetailCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.SystemCodeDetails.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return SystemCodeDetailErrors.NotFound(command.Id);

        var codeExists = await _context.SystemCodeDetails.AnyAsync(
            x =>
                x.SystemCodeId == command.SystemCodeId
                && x.Code == command.Code
                && x.Id != command.Id,
            cancellationToken
        );

        if (codeExists)
            return SystemCodeDetailErrors.CodeAlreadyExists;

        var updateResult = entity.Update(
            command.SystemCodeId,
            command.Code,
            command.Description,
            command.OrderNo
        );

        if (updateResult.IsError)
            return updateResult.Errors;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }
}
