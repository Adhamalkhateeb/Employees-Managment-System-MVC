using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.SystemCodeDetails.Dtos;
using EmployeesManager.Application.Features.SystemCodeDetails.Mappings;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Queries.GetAllSystemCodeDetails;

public sealed class GetAllSystemCodeDetailsQueryHandler
    : IRequestHandler<GetAllSystemCodeDetailsQuery, Result<List<SystemCodeDetailDto>>>
{
    private readonly IAppDbContext _context;

    public GetAllSystemCodeDetailsQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<List<SystemCodeDetailDto>>> Handle(
        GetAllSystemCodeDetailsQuery query,
        CancellationToken cancellationToken
    )
    {
        var entities = await _context
            .SystemCodeDetails.AsNoTracking()
            .Include(x => x.SystemCode)
            .ToListAsync(cancellationToken);

        return (Result<List<SystemCodeDetailDto>>)entities.ToDtos();
    }
}
