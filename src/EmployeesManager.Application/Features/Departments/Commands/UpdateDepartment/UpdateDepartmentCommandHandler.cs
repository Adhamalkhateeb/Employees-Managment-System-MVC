using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Departments;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Departments.Commands.UpdateDepartment;

public sealed class UpdateDepartmentCommandHandler
    : IRequestHandler<UpdateDepartmentCommand, Result<Updated>>
{
    private readonly IAppDbContext _context;

    public UpdateDepartmentCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Updated>> Handle(
        UpdateDepartmentCommand command,
        CancellationToken cancellationToken
    )
    {
        var department = await _context.Departments.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (department is null)
            return DepartmentErrors.NotFound(command.Id);

        var nameExists = await _context.Departments.AnyAsync(
            x => x.Name == command.Name && x.Id != command.Id,
            cancellationToken
        );

        if (nameExists)
            return DepartmentErrors.NameAlreadyExists;


        var updateResult = department.Update(command.Name, command.ManagerId);

        if (updateResult.IsError)
            return updateResult.Errors;

        var updatedDepartment = updateResult.Value;

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
                x => x.ManagerId == managerId.Value && x.Id != command.Id,
                cancellationToken
            );

            if (alreadyAssigned)
                return DepartmentErrors.ManagerAlreadyAssigned;

            department.AssignManager(managerId.Value);
        }
        else
        {
            department.RemoveManager();
        }

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }
}
