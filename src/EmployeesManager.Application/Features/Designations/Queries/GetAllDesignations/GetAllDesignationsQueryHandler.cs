using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Designations.Dtos;
using EmployeesManager.Application.Features.Designations.Mappings;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Designations.Queries.GetAllDesignations;

public sealed class GetAllDesignationsQueryHandler
    : IRequestHandler<GetAllDesignationsQuery, Result<List<DesignationDto>>>
{
    private readonly IAppDbContext _context;

    public GetAllDesignationsQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<List<DesignationDto>>> Handle(
        GetAllDesignationsQuery query,
        CancellationToken cancellationToken
    )
    {
        var entities = await _context.Designations.AsNoTracking().ToListAsync(cancellationToken);

        return (Result<List<DesignationDto>>)entities.ToDtos();
    }
}
