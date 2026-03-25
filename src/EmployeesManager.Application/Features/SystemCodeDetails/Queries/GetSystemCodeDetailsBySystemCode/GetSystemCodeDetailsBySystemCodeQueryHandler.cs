using System.Globalization;
using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.SystemCodeDetails.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.SystemCodeDetails.Queries.GetSystemCodeDetailsBySystemCode;

public sealed class GetSystemCodeDetailsBySystemCodeQueryHandler
    : IRequestHandler<GetSystemCodeDetailsBySystemCodeQuery, Result<List<SystemCodeDetailDto>>>
{
    private readonly IAppDbContext _context;

    public GetSystemCodeDetailsBySystemCodeQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<SystemCodeDetailDto>>> Handle(
        GetSystemCodeDetailsBySystemCodeQuery query,
        CancellationToken cancellationToken
    )
    {
        var normalizedCode = query.SystemCode?.Trim().ToUpper(CultureInfo.InvariantCulture);

        var entities = await _context
            .SystemCodeDetails.AsNoTracking()
            .Where(x => x.SystemCode.Code.ToUpper() == normalizedCode)
            .OrderBy(x => x.OrderNo ?? int.MaxValue)
            .ThenBy(x => x.Code)
            .Select(x => new SystemCodeDetailDto(
                x.Id,
                x.SystemCodeId,
                x.SystemCode.Code,
                x.Code,
                x.Description,
                x.OrderNo
            ))
            .ToListAsync(cancellationToken);

        if (entities.Count == 0)
        {
            return Error.NotFound(
                "SystemCodeDetails.NotConfigured",
                $"No system code details are configured for the system code: {query.SystemCode}"
            );
        }

        return entities;
    }
}
