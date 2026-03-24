using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Cities.Dtos;
using EmployeesManager.Application.Features.Cities.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Cities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Cities.Commands.CreateCity;

public sealed class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, Result<CityDto>>
{
    private readonly IAppDbContext _context;

    public CreateCityCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<CityDto>> Handle(
        CreateCityCommand command,
        CancellationToken cancellationToken
    )
    {
        var codeExists = await _context.Cities.AnyAsync(
            x => x.Code == command.Code,
            cancellationToken
        );

        if (codeExists)
            return CityErrors.CodeAlreadyExists;

        var nameExists = await _context.Cities.AnyAsync(
            x => x.Name == command.Name && x.CountryId == command.CountryId,
            cancellationToken
        );

        if (nameExists)
            return CityErrors.NameAlreadyExists;

        var createResult = City.Create(command.Code, command.Name, command.CountryId);

        if (createResult.IsError)
            return createResult.Errors;

        var entity = createResult.Value;
        _context.Cities.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }
}
