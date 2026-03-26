using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveApplications.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.LeaveApplications.Queries.GetAllLeaveApplications;

public sealed class GetAllLeaveApplicationsQueryHandler
    : IRequestHandler<GetAllLeaveApplicationsQuery, Result<List<LeaveApplicationDto>>>
{
    private readonly IAppDbContext _context;

    public GetAllLeaveApplicationsQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<List<LeaveApplicationDto>>> Handle(
        GetAllLeaveApplicationsQuery query,
        CancellationToken cancellationToken
    )
    {
        var entities = await _context
            .LeaveApplications.AsNoTracking()
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
                x.ApprovedBy,
                x.ApprovedAtUtc
            ))
            .ToListAsync(cancellationToken);

        return entities;
    }
}
