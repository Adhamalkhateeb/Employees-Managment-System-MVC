using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveTypes.Dtos;
using EmployeesManager.Application.Features.LeaveTypes.Mappings;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.LeaveTypes.Queries.GetAllLeaveTypes;

public sealed class GetAllLeaveTypesQueryHandler
    : IRequestHandler<GetAllLeaveTypesQuery, Result<List<LeaveTypeDto>>>
{
    private readonly IAppDbContext _context;

    public GetAllLeaveTypesQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<List<LeaveTypeDto>>> Handle(
        GetAllLeaveTypesQuery query,
        CancellationToken cancellationToken
    )
    {
        var entities = await _context.LeaveTypes.AsNoTracking().ToListAsync(cancellationToken);

        return entities.ToDtos();
    }
}
