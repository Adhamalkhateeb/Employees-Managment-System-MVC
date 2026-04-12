using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Branches.Dtos;
using EmployeesManager.Application.Features.Branches.Mappings;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Branches.Queries.GetAllBranchs;

public sealed class GetAllBranchsQueryHandler
    : IRequestHandler<GetBranchesQuery, Result<List<BranchDto>>>
{
    private readonly IAppDbContext _context;

    public GetAllBranchsQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<List<BranchDto>>> Handle(
        GetBranchesQuery query,
        CancellationToken cancellationToken
    )
    {
        var entities = await _context.Branches.AsNoTracking().ToListAsync(cancellationToken);

        return entities.ToDtos();
    }
}
