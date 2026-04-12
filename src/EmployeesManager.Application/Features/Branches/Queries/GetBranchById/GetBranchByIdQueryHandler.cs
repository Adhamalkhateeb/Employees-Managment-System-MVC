using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Branches.Dtos;
using EmployeesManager.Application.Features.Branches.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Branches;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Branches.Queries.GetBranchById;

public sealed class GetBranchByIdQueryHandler
    : IRequestHandler<GetBranchByIdQuery, Result<BranchDto>>
{
    private readonly IAppDbContext _context;

    public GetBranchByIdQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<BranchDto>> Handle(
        GetBranchByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context
            .Branches.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (entity is null)
            return BranchErrors.NotFound(query.Id);

        return entity.ToDto();
    }
}
