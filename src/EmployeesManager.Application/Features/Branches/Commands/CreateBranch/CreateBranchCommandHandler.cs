using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Branches;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Branches.Commands.CreateBranch;

public sealed class CreateBranchCommandHandler
    : IRequestHandler<CreateBranchCommand, Result<Created>>
{
    private readonly IAppDbContext _context;

    public CreateBranchCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Created>> Handle(
        CreateBranchCommand command,
        CancellationToken cancellationToken
    )
    {
        var nameExists = await _context.Branches.AnyAsync(
            x => x.Name == command.Name,
            cancellationToken
        );

        if (nameExists)
            return BranchErrors.AlreadyExists(command.Name);

        var phoneExists = await _context.Branches.AnyAsync(
            x => x.Phone.Value == command.PhoneNumber,
            cancellationToken
        );

        if (phoneExists)
            return BranchErrors.DuplicatePhone;

        var emailExists = await _context.Branches.AnyAsync(
            x => x.Email.Value == command.EmailAddress,
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

        var createResult = Branch.Create(
            command.Name,
            command.Address,
            command.PhoneNumber,
            command.EmailAddress,
            command.ManagerId
        );

        if (createResult.IsError)
            return createResult.Errors;

        var entity = createResult.Value;
        _context.Branches.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Created;
    }
}
