using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Branches;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Branches.Commands.UpdateBranch;

public sealed class UpdateBranchCommandHandler
    : IRequestHandler<UpdateBranchCommand, Result<Updated>>
{
    private readonly IAppDbContext _context;

    public UpdateBranchCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Updated>> Handle(
        UpdateBranchCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.Branches.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return BranchErrors.NotFound(command.Id);

        var nameExists = await _context.Branches.AnyAsync(
            x => x.Name == command.Name && x.Id != command.Id,
            cancellationToken
        );

        if (nameExists)
            return BranchErrors.AlreadyExists(command.Name);

        var phoneExists = await _context.Branches.AnyAsync(
            x => x.Phone.Value == command.PhoneNumber && x.Id != command.Id,
            cancellationToken
        );

        if (phoneExists)
            return BranchErrors.DuplicatePhone;

        var emailExists = await _context.Branches.AnyAsync(
            x => x.Email.Value == command.EmailAddress && x.Id != command.Id,
            cancellationToken
        );

        if (emailExists)
            return BranchErrors.DuplicateEmail;

        if (command.ManagerId.HasValue)
        {
            var managerExists = await _context.Employees.AnyAsync(
                x => x.Id == command.ManagerId.Value,
                cancellationToken
            );

            if (!managerExists)
                return BranchErrors.ManagerNotFound;
        }

        var updateResult = entity.Update(
            command.Name,
            command.Address,
            command.PhoneNumber,
            command.EmailAddress,
            command.ManagerId
        );

        if (updateResult.IsError)
            return updateResult.Errors;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }
}
