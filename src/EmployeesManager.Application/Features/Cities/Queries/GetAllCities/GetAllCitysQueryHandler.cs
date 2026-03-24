using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Cities.Dtos;
using EmployeesManager.Application.Features.Cities.Mappings;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Cities.Queries.GetAllCities;

public sealed class GetAllCitiesQueryHandler
    : IRequestHandler<GetAllCitiesQuery, Result<List<CityDto>>>
{
    private readonly IAppDbContext _context;

    public GetAllCitiesQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<List<CityDto>>> Handle(
        GetAllCitiesQuery query,
        CancellationToken cancellationToken
    )
    {
        var entities = await _context
            .Cities.AsNoTracking()
            .Include(x => x.Country)
            .ToListAsync(cancellationToken);

        return (Result<List<CityDto>>)entities.ToDtos();
    }
}
