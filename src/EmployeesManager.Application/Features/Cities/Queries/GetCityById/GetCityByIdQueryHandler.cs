using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Cities.Dtos;
using EmployeesManager.Application.Features.Cities.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Cities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Cities.Queries.GetCityById;

public sealed class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, Result<CityDto>>
{
    private readonly IAppDbContext _context;

    public GetCityByIdQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<CityDto>> Handle(
        GetCityByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var dto = await _context
            .Cities.AsNoTracking()
            .Where(x => x.Id == query.Id)
            .Select(x => new CityDto(x.Id, x.Code, x.Name, x.CountryId, x.Country.Name))
            .FirstOrDefaultAsync(cancellationToken);

        if (dto is null)
            return CityErrors.NotFound(query.Id);

        return dto;
    }
}
