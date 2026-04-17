using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Countries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Countries.Commands.UpdateCountry;

public sealed class UpdateCountryCommandHandler
    : IRequestHandler<UpdateCountryCommand, Result<Updated>>
{
    private readonly IAppDbContext _context;

    public UpdateCountryCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Updated>> Handle(
        UpdateCountryCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.Countries.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return CountryErrors.NotFound(command.Id);

        var codeExists = await _context.Countries.AnyAsync(
            x => x.Code == command.Code && x.Id != command.Id,
            cancellationToken
        );

        if (codeExists)
            return CountryErrors.CodeAlreadyExists;

        var nameExists = await _context.Countries.AnyAsync(
            x => x.Name == command.Name && x.Id != command.Id,
            cancellationToken
        );

        if (nameExists)
            return CountryErrors.NameAlreadyExists;

        var updateResult = entity.Update(command.Code, command.Name);

        if (updateResult.IsError)
            return updateResult.Errors;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }
}
