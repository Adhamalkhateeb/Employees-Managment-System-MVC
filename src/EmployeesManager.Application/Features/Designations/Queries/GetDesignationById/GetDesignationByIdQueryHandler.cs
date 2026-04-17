using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Designations.Dtos;
using EmployeesManager.Application.Features.Designations.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Designations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Designations.Queries.GetDesignationById;

public sealed class GetDesignationByIdQueryHandler
    : IRequestHandler<GetDesignationByIdQuery, Result<DesignationDto>>
{
    private readonly IAppDbContext _context;

    public GetDesignationByIdQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<DesignationDto>> Handle(
        GetDesignationByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context
            .Designations.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (entity is null)
            return DesignationErrors.NotFound(query.Id);

        return entity.ToDto();
    }
}
