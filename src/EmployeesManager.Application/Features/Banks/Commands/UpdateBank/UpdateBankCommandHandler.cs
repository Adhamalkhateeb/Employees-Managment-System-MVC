using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Banks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Banks.Commands.UpdateBank;

public sealed class UpdateBankCommandHandler : IRequestHandler<UpdateBankCommand, Result<Updated>>
{
    private readonly IAppDbContext _context;

    public UpdateBankCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Updated>> Handle(
        UpdateBankCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.Banks.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return BankErrors.NotFound(command.Id);

        var codeExists = await _context.Banks.AnyAsync(
            x => x.Code == command.Code && x.Id != command.Id,
            cancellationToken
        );

        if (codeExists)
            return BankErrors.CodeAlreadyExists;

        var nameExists = await _context.Banks.AnyAsync(
            x => x.Name == command.Name && x.Id != command.Id,
            cancellationToken
        );

        if (nameExists)
            return BankErrors.NameAlreadyExists;

        var accountNoExists = await _context.Banks.AnyAsync(
            x => x.AccountNo == command.AccountNo && x.Id != command.Id,
            cancellationToken
        );

        if (accountNoExists)
            return BankErrors.AccountNoAlreadyExists;

        var updateResult = entity.Update(command.Code, command.Name, command.AccountNo);

        if (updateResult.IsError)
            return updateResult.Errors;

        await _context.SaveChangesAsync(cancellationToken);
        return Result.Updated;
    }
}
