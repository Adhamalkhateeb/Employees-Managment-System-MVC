using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.SystemCodes.Dtos;
using EmployeesManager.Application.Features.SystemCodes.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.SystemCodes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.SystemCodes.Commands.CreateSystemCode;

public sealed class CreateSystemCodeCommandHandler
    : IRequestHandler<CreateSystemCodeCommand, Result<Created>>
{
    private readonly IAppDbContext _context;

    public CreateSystemCodeCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Created>> Handle(
        CreateSystemCodeCommand command,
        CancellationToken cancellationToken
    )
    {
        var codeExists = await _context.SystemCodes.AnyAsync(
            x => x.Code == command.Code,
            cancellationToken
        );

        if (codeExists)
            return SystemCodeErrors.CodeAlreadyExists;

        var createResult = SystemCode.Create(command.Code, command.Description);

        if (createResult.IsError)
            return createResult.Errors;

        var entity = createResult.Value;
        _context.SystemCodes.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Created;
    }
}
