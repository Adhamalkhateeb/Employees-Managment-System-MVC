using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Departments.Dtos;
using EmployeesManager.Application.Features.Departments.Mappings;
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
        var entity = await _context
            .Departments.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (entity is null)
            return DepartmentErrors.NotFound(query.Id);

        return entity.ToDto();
    }
}
