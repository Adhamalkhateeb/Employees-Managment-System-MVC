using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Countries.Dtos;
using EmployeesManager.Application.Features.Countries.Mappings;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Countries.Queries.GetAllCountries;

public sealed class GetAllCountriesQueryHandler
    : IRequestHandler<GetAllCountriesQuery, Result<List<CountryDto>>>
{
    private readonly IAppDbContext _context;

    public GetAllCountriesQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<List<CountryDto>>> Handle(
        GetAllCountriesQuery query,
        CancellationToken cancellationToken
    )
    {
        var entities = await _context.Countries.AsNoTracking().ToListAsync(cancellationToken);

        return (Result<List<CountryDto>>)entities.ToDtos();
    }
}
