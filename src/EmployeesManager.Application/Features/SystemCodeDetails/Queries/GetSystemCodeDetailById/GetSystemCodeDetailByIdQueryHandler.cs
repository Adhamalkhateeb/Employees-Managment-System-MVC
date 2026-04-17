using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.SystemCodeDetails.Dtos;
using EmployeesManager.Application.Features.SystemCodeDetails.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.SystemCodeDetails;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Queries.GetSystemCodeDetailById;

public sealed class GetSystemCodeDetailByIdQueryHandler
    : IRequestHandler<GetSystemCodeDetailByIdQuery, Result<SystemCodeDetailDto>>
{
    private readonly IAppDbContext _context;

    public GetSystemCodeDetailByIdQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<SystemCodeDetailDto>> Handle(
        GetSystemCodeDetailByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context
            .SystemCodeDetails.AsNoTracking()
            .Where(x => x.Id == query.Id)
            .Select(x => new SystemCodeDetailDto(
                x.Id,
                x.SystemCodeId,
                x.SystemCode.Code,
                x.Code,
                x.Description,
                x.OrderNo
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (entity is null)
            return SystemCodeDetailErrors.NotFound(query.Id);

        return entity;
    }
}
