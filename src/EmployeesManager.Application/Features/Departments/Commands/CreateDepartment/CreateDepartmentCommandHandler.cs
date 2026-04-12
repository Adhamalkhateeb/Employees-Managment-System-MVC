using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Departments;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Departments.Commands.CreateDepartment;

public sealed class CreateDepartmentCommandHandler
    : IRequestHandler<CreateDepartmentCommand, Result<Created>>
{
    private readonly IAppDbContext _context;

    public CreateDepartmentCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Created>> Handle(
        CreateDepartmentCommand command,
        CancellationToken cancellationToken
    )
    {
        var departmentName = command.Name.Trim().ToLowerInvariant();

        var nameExists = await _context.Departments.AnyAsync(
            x => x.Name.ToLower() == departmentName,
            cancellationToken
        );

        if (nameExists)
            return DepartmentErrors.NameAlreadyExists;

        var createResult = Department.Create(command.Name, command.ManagerId);

        if (createResult.IsError)
            return createResult.Errors;

        var department = createResult.Value;

        Guid? managerId = command.ManagerId;

        if (managerId.HasValue)
        {
            var managerExists = await _context.Employees.AnyAsync(
                x => x.Id == managerId.Value,
                cancellationToken
            );

            if (!managerExists)
                return DepartmentErrors.ManagerNotFound;

            var alreadyAssigned = await _context.Departments.AnyAsync(
                x => x.ManagerId == managerId.Value,
                cancellationToken
            );

            if (alreadyAssigned)
                return DepartmentErrors.ManagerAlreadyAssigned;

            department.AssignManager(managerId.Value);
        }

        _context.Departments.Add(department);

        await _context.SaveChangesAsync(cancellationToken);

        return Result.Created;
    }
}
