using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Employees.Dtos;
using EmployeesManager.Application.Features.Employees.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Employees;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeesManager.Application.Features.Employees.Queries.GetEmployeeById;

public sealed class GetEmployeeByIdQueryHandler
    : IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDto>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<GetEmployeeByIdQueryHandler> _logger;

    public GetEmployeeByIdQueryHandler(
        IAppDbContext context,
        ILogger<GetEmployeeByIdQueryHandler> logger
    )
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<EmployeeDto>> Handle(
        GetEmployeeByIdQuery query,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context
            .Employees.AsNoTracking()
            .Where(e => e.Id == query.Id)
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
            .FirstOrDefaultAsync(cancellationToken);

        if (entity is null)
        {
            _logger.LogWarning("Employee not found for ID: {EmployeeId}", query.Id);
            return EmployeeErrors.NotFound(query.Id);
        }

        return entity;
    }
}
