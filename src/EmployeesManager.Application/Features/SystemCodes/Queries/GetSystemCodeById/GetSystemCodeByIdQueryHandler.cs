using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.SystemCodes.Dtos;
using EmployeesManager.Application.Features.SystemCodes.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.SystemCodes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.SystemCodes.Queries.GetSystemCodeById;

public sealed class GetSystemCodeByIdQueryHandler
    : IRequestHandler<GetSystemCodeByIdQuery, Result<SystemCodeDto>>
{
    private readonly IAppDbContext _context;

    public GetSystemCodeByIdQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<SystemCodeDto>> Handle(
        GetSystemCodeByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context
            .SystemCodes.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (entity is null)
            return SystemCodeErrors.NotFound(query.Id);

        return entity.ToDto();
    }
}
