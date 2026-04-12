using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Departments.Dtos;
using EmployeesManager.Application.Features.Employees.Dtos;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Departments;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Departments.Queries.GetDepartmentById;

public sealed class GetDepartmentByIdQueryHandler
    : IRequestHandler<GetDepartmentByIdQuery, Result<DepartmentDto>>
{
    private readonly IAppDbContext _context;

    public GetDepartmentByIdQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<DepartmentDto>> Handle(
        GetDepartmentByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var department = await _context
            .Departments.AsNoTracking()
            .Where(d => d.Id == query.Id)
            .LeftJoin(
                _context.Employees.AsNoTracking(),
                d => d.ManagerId,
                e => e.Id,
                (d, e) =>
                    new DepartmentDto(
                        d.Id,
                        d.Name,
                        d.Employees.Count,
                        d.ManagerId,
                        e != null ? $"{e.FirstName} {e.LastName}" : null,
                        d.Employees.Select(emp => new EmployeeDto(
                                emp.Id,
                                emp.FirstName,
                                emp.LastName,
                                emp.NationalId,
                                emp.PhoneNumber,
                                emp.EmailAddress,
                                emp.HireDate,
                                emp.Address,
                                emp.DepartmentId,
                                d.Name,
                                emp.BranchId,
                                emp.Branch != null ? emp.Branch.Name : null
                            ))
                            .ToList()
                    )
            )
            .FirstOrDefaultAsync(cancellationToken);

        if (department is null)
            return DepartmentErrors.NotFound(query.Id);

        return department;
    }
}
