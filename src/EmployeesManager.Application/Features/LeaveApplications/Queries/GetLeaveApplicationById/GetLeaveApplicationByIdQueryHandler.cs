using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveApplications.Dtos;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.LeaveApplications;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.LeaveApplications.Queries.GetLeaveApplicationById;

public sealed class GetLeaveApplicationByIdQueryHandler
    : IRequestHandler<GetLeaveApplicationByIdQuery, Result<LeaveApplicationDto>>
{
    private readonly IAppDbContext _context;

    public GetLeaveApplicationByIdQueryHandler(IAppDbContext context)
    {
        _context = context;
    }

    public async Task<Result<LeaveApplicationDto>> Handle(
        GetLeaveApplicationByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var dto = await _context
            .LeaveApplications.AsNoTracking()
            .Where(x => x.Id == query.Id)
            .Select(x => new LeaveApplicationDto(
                x.Id,
                x.EmployeeId,
                x.Employee.FirstName + " " + x.Employee.LastName,
                x.LeaveTypeId,
                x.LeaveType.Code ?? x.LeaveType.Name,
                x.Duration,
                x.Status,
                x.StartDate,
                x.EndDate,
                x.EndDate.Date.Subtract(x.StartDate.Date).Days + 1,
                x.Description,
                x.Attachment,
                x.RejectionReason,
                x.DecisionById,
                null,
                x.DecisionAtUtc
            ))
            .FirstOrDefaultAsync(cancellationToken);

        if (dto is null)
            return LeaveApplicationErrors.NotFound(query.Id);

        return dto;
    }
}
