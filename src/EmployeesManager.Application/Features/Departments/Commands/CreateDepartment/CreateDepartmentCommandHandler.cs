using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Departments.Dtos;
using EmployeesManager.Application.Features.Departments.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Departments;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Departments.Commands.CreateDepartment;

public sealed class CreateDepartmentCommandHandler
    : IRequestHandler<CreateDepartmentCommand, Result<DepartmentDto>>
{
    private readonly IAppDbContext _context;

    public CreateDepartmentCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<DepartmentDto>> Handle(
        CreateDepartmentCommand command,
        CancellationToken cancellationToken
    )
    {
        var nameExists = await _context.Departments.AnyAsync(
            x => x.Name == command.Name,
            cancellationToken
        );

        if (nameExists)
            return DepartmentErrors.NameAlreadyExists;

        var codeExists = await _context.Departments.AnyAsync(
            x => x.Code == command.Code,
            cancellationToken
        );

        if (codeExists)
            return DepartmentErrors.CodeAlreadyExists;

        var createResult = Department.Create(command.Name, command.Code);

        if (createResult.IsError)
            return createResult.Errors;

        var entity = createResult.Value;
        _context.Departments.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }
}
