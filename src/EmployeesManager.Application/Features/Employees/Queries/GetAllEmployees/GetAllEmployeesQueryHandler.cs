using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Employees.Dtos;
using EmployeesManager.Application.Features.Employees.Mappings;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Employees.Queries.GetAllEmployees;

public sealed class GetAllEmployeesQueryHandler
    : IRequestHandler<GetAllEmployeesQuery, Result<List<EmployeeDto>>>
{
    private readonly IAppDbContext _context;

    public GetAllEmployeesQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<List<EmployeeDto>>> Handle(
        GetAllEmployeesQuery query,
        CancellationToken cancellationToken
    )
    {
        var entities = await _context
            .Employees.AsNoTracking()
            .Select(e => new EmployeeDto(
                e.Id,
                e.FirstName,
                e.LastName,
                e.NationalId,
                e.PhoneNumber,
                e.EmailAddress,
                e.HireDate,
                e.Address,
                e.DepartmentId,
                e.Department.Name,
                e.BranchId,
                e.Branch != null ? e.Branch.Name : null
            ))
            .ToListAsync(cancellationToken);

        return entities;
    }
}
