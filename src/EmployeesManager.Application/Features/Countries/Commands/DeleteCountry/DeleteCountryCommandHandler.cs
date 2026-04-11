using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Countries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Countries.Commands.DeleteCountry;

public sealed class DeleteCountryCommandHandler
    : IRequestHandler<DeleteCountryCommand, Result<Deleted>>
{
    private readonly IAppDbContext _context;

    public DeleteCountryCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Deleted>> Handle(
        DeleteCountryCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.Countries.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return CountryErrors.NotFound(command.Id);

        _context.Countries.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }
}
