using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Countries.Dtos;
using EmployeesManager.Application.Features.Countries.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Countries;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Countries.Queries.GetCountryById;

public sealed class GetCountryByIdQueryHandler
    : IRequestHandler<GetCountryByIdQuery, Result<CountryDto>>
{
    private readonly IAppDbContext _context;

    public GetCountryByIdQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<CountryDto>> Handle(
        GetCountryByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context
            .Countries.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (entity is null)
            return CountryErrors.NotFound(query.Id);

        return entity.ToDto();
    }
}
