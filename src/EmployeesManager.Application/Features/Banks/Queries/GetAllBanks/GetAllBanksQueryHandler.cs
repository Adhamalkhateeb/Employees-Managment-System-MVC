using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Banks.Dtos;
using EmployeesManager.Application.Features.Banks.Mappings;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Banks.Queries.GetAllBanks;

public sealed class GetAllBanksQueryHandler
    : IRequestHandler<GetAllBanksQuery, Result<List<BankDto>>>
{
    private readonly IAppDbContext _context;

    public GetAllBanksQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<List<BankDto>>> Handle(
        GetAllBanksQuery query,
        CancellationToken cancellationToken
    )
    {
        var entities = await _context.Banks.AsNoTracking().ToListAsync(cancellationToken);

        return (Result<List<BankDto>>)entities.ToDtos();
    }
}
