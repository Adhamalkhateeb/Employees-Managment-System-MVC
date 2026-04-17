using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Cities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Cities.Commands.UpdateCity;

public sealed class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, Result<Updated>>
{
    private readonly IAppDbContext _context;

    public UpdateCityCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Updated>> Handle(
        UpdateCityCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.Cities.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return CityErrors.NotFound(command.Id);

        var codeExists = await _context.Cities.AnyAsync(
            x => x.Code == command.Code && x.Id != command.Id,
            cancellationToken
        );

        if (codeExists)
            return CityErrors.CodeAlreadyExists;

        var nameExists = await _context.Cities.AnyAsync(
            x => x.Name == command.Name && x.CountryId == command.CountryId && x.Id != command.Id,
            cancellationToken
        );

        if (nameExists)
            return CityErrors.NameAlreadyExists;

        var updateResult = entity.Update(command.Code, command.Name, command.CountryId);

        if (updateResult.IsError)
            return updateResult.Errors;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }
}
