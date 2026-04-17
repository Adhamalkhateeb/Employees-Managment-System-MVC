using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Cities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Cities.Commands.DeleteCity;

public sealed class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, Result<Deleted>>
{
    private readonly IAppDbContext _context;

    public DeleteCityCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Deleted>> Handle(
        DeleteCityCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.Cities.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return CityErrors.NotFound(command.Id);

        _context.Cities.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }
}
