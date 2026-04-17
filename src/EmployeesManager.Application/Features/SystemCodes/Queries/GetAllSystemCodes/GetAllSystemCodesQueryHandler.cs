using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.SystemCodes.Dtos;
using EmployeesManager.Application.Features.SystemCodes.Mappings;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.SystemCodes.Queries.GetAllSystemCodes;

public sealed class GetAllSystemCodesQueryHandler
    : IRequestHandler<GetAllSystemCodesQuery, Result<List<SystemCodeDto>>>
{
    private readonly IAppDbContext _context;

    public GetAllSystemCodesQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<List<SystemCodeDto>>> Handle(
        GetAllSystemCodesQuery query,
        CancellationToken cancellationToken
    )
    {
        var entities = await _context.SystemCodes.AsNoTracking().ToListAsync(cancellationToken);

        return (Result<List<SystemCodeDto>>)entities.ToDtos();
    }
}
