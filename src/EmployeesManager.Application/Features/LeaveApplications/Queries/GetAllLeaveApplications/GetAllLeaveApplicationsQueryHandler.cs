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
                x.LeaveType.Name,
                x.DurationId,
                x.Duration.Description ?? x.Duration.Code,
                x.StatusId,
                x.Status.Description ?? x.Status.Code,
                x.StartDate,
                x.EndDate,
                x.Days,
                x.Description,
                x.Attachment,
                x.ApprovedBy,
                x.ApprovedAtUtc
            ))
            .ToListAsync(cancellationToken);

        return entities;
    }
}
