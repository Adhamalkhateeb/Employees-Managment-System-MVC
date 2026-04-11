using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Banks.Dtos;
using EmployeesManager.Application.Features.Banks.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Banks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Banks.Queries.GetBankById;

public sealed class GetBankByIdQueryHandler : IRequestHandler<GetBankByIdQuery, Result<BankDto>>
{
    private readonly IAppDbContext _context;

    public GetBankByIdQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<BankDto>> Handle(
        GetBankByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context
            .Banks.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (entity is null)
            return BankErrors.NotFound(query.Id);

        return entity.ToDto();
    }
}
