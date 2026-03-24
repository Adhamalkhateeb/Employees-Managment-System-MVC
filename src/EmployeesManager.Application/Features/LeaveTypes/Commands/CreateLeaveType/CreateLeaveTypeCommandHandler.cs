using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.LeaveTypes.Dtos;
using EmployeesManager.Application.Features.LeaveTypes.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.LeaveTypes;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.LeaveTypes.Commands.CreateLeaveType;

public sealed class CreateLeaveTypeCommandHandler
    : IRequestHandler<CreateLeaveTypeCommand, Result<LeaveTypeDto>>
{
    private readonly IAppDbContext _context;

    public CreateLeaveTypeCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<LeaveTypeDto>> Handle(
        CreateLeaveTypeCommand command,
        CancellationToken cancellationToken
    )
    {
        var nameExists = await _context.LeaveTypes.AnyAsync(
            x => x.Name == command.Name,
            cancellationToken
        );

        if (nameExists)
            return LeaveTypeErrors.NameAlreadyExists;

        var createResult = LeaveType.Create(command.Name);

        if (createResult.IsError)
            return createResult.Errors;

        var entity = createResult.Value;
        _context.LeaveTypes.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return entity.ToDto();
    }
}
