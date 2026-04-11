using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.SystemCodeDetails.Dtos;
using EmployeesManager.Application.Features.SystemCodeDetails.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.SystemCodeDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Commands.CreateSystemCodeDetail;

public sealed class CreateSystemCodeDetailCommandHandler
    : IRequestHandler<CreateSystemCodeDetailCommand, Result<Created>>
{
    private readonly IAppDbContext _context;

    public CreateSystemCodeDetailCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Created>> Handle(
        CreateSystemCodeDetailCommand command,
        CancellationToken cancellationToken
    )
    {
        var codeExists = await _context.SystemCodeDetails.AnyAsync(
            x => x.SystemCodeId == command.SystemCodeId && x.Code == command.Code,
            cancellationToken
        );

        if (codeExists)
            return SystemCodeDetailErrors.CodeAlreadyExists;

        var createResult = SystemCodeDetail.Create(
            command.SystemCodeId,
            command.Code,
            command.Description,
            command.OrderNo
        );

        if (createResult.IsError)
            return createResult.Errors;

        var entity = createResult.Value;
        _context.SystemCodeDetails.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Created;
    }
}
