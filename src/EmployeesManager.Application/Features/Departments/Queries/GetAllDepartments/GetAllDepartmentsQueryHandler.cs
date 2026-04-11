using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Departments.Dtos;
using EmployeesManager.Application.Features.Departments.Mappings;
using EmployeesManager.Domain.Common.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Departments.Queries.GetAllDepartments;

public sealed class GetAllDepartmentsQueryHandler
    : IRequestHandler<GetAllDepartmentsQuery, Result<List<DepartmentDto>>>
{
    private readonly IAppDbContext _context;

    public GetAllDepartmentsQueryHandler(IAppDbContext context) => _context = context;

    public async Task<Result<List<DepartmentDto>>> Handle(
        GetAllDepartmentsQuery query,
        CancellationToken cancellationToken
    )
    {
        var entities = await _context.Departments.AsNoTracking().ToListAsync(cancellationToken);

        return entities.ToDtos();
    }
}
