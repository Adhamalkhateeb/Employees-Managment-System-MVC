using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Common.Models;
using EmployeesManager.Application.Features.Departments.Dtos;
using EmployeesManager.Application.Features.Departments.Mappings;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Departments.Queries.GetAllDepartments;

public sealed class GetDepartmentsQueryHandler
    : IRequestHandler<GetDepartmentsQuery, Result<PaginatedList<DepartmentWithoutEmployeesDto>>>
{
    private readonly IAppDbContext _context;

    public GetDepartmentsQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<PaginatedList<DepartmentWithoutEmployeesDto>>> Handle(
        GetDepartmentsQuery query,
        CancellationToken cancellationToken
    )
    {
        var departments = _context
            .Departments.AsNoTracking()
            .Where(x => string.IsNullOrEmpty(query.SearchTerm) || x.Name.Contains(query.SearchTerm))
            .LeftJoin(
                _context.Employees.AsNoTracking(),
                d => d.ManagerId,
                e => e.Id,
                (d, e) =>
                    new DepartmentWithoutEmployeesDto(
                        d.Id,
                        d.Name,
                        d.Employees.Count,
                        d.ManagerId,
                        e != null ? $"{e.FirstName} {e.LastName}" : null
                    )
            );

        return await PaginatedList<DepartmentWithoutEmployeesDto>.CreateAsync(
            departments,
            query.Page,
            query.PageSize,
            cancellationToken
        );
    }
}
