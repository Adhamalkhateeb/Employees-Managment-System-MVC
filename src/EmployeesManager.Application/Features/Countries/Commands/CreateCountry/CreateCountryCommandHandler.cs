using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Countries.Dtos;
using EmployeesManager.Application.Features.Countries.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Countries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Countries.Commands.CreateCountry;

public sealed class CreateCountryCommandHandler
    : IRequestHandler<CreateCountryCommand, Result<Created>>
{
    private readonly IAppDbContext _context;

    public CreateCountryCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Created>> Handle(
        CreateCountryCommand command,
        CancellationToken cancellationToken
    )
    {
        var codeExists = await _context.Countries.AnyAsync(
            x => x.Code == command.Code,
            cancellationToken
        );

        if (codeExists)
            return CountryErrors.CodeAlreadyExists;

        var nameExists = await _context.Countries.AnyAsync(
            x => x.Name == command.Name,
            cancellationToken
        );

        if (nameExists)
            return CountryErrors.NameAlreadyExists;

        var createResult = Country.Create(command.Code, command.Name);

        if (createResult.IsError)
            return createResult.Errors;

        var entity = createResult.Value;
        _context.Countries.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Created;
    }
}
