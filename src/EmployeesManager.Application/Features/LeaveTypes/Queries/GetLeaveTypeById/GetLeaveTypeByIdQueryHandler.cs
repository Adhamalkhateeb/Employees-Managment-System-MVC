using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveTypes.Dtos;
using EmployeesManager.Application.Features.LeaveTypes.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.LeaveTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.LeaveTypes.Queries.GetLeaveTypeById;

public sealed class GetLeaveTypeByIdQueryHandler
    : IRequestHandler<GetLeaveTypeByIdQuery, Result<LeaveTypeDto>>
{
    private readonly IAppDbContext _context;

    public GetLeaveTypeByIdQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<LeaveTypeDto>> Handle(
        GetLeaveTypeByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context
            .LeaveTypes.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (entity is null)
            return LeaveTypeErrors.NotFound(query.Id);

        return entity.ToDto();
    }
}
