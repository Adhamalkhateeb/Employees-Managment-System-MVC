using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Designations.Dtos;
using EmployeesManager.Application.Features.Designations.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Designations;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Designations.Commands.CreateDesignation;

public sealed class CreateDesignationCommandHandler
    : IRequestHandler<CreateDesignationCommand, Result<DesignationDto>>
{
    private readonly IAppDbContext _context;

    public CreateDesignationCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<DesignationDto>> Handle(
        CreateDesignationCommand command,
        CancellationToken cancellationToken
    )
    {
        var nameExists = await _context.Designations.AnyAsync(
            x => x.Name == command.Name,
            cancellationToken
        );

        if (nameExists)
            return DesignationErrors.NameAlreadyExists;

        var createResult = Designation.Create(command.Name);

        if (createResult.IsError)
            return createResult.Errors;

        var entity = createResult.Value;
        _context.Designations.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }
}
