using System.Reflection;
using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveApplications.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.LeaveApplications.Queries.GetLeaveApplicationLookups;

public sealed class GetLeaveApplicationLookupsQueryHandler
    : IRequestHandler<GetLeaveApplicationLookupsQuery, Result<LeaveApplicationLookupDto>>
{
    IAppDbContext _context;

    public GetLeaveApplicationLookupsQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<LeaveApplicationLookupDto>> Handle(
        GetLeaveApplicationLookupsQuery query,
        CancellationToken cancellationToken
    )
    {
        var employees = await _context
            .Employees.Select(e => new
            {
                e.Id,
                e.FirstName,
                e.LastName,
            })
            .ToListAsync(cancellationToken);

        var leaveTypes = await _context
            .LeaveTypes.Select(lt => new { lt.Id, lt.Code })
            .ToListAsync(cancellationToken);

        var lookupDto = new LeaveApplicationLookupDto(
            employees.Select(e => new EmployeeLookupDto(e.Id, e.FirstName + " " + e.LastName)),
            leaveTypes.Select(lt => new LeaveTypeLookupDto(lt.Id, lt.Code)),
            Enum.GetNames(typeof(Domain.Entities.LeaveApplications.Enums.LeaveApplicationDurations))
        );

        return lookupDto;
    }
}
