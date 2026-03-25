using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Banks.Dtos;
using EmployeesManager.Application.Features.Banks.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Banks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Banks.Commands.CreateBank;

public sealed class CreateBankCommandHandler : IRequestHandler<CreateBankCommand, Result<Created>>
{
    private readonly IAppDbContext _context;

    public CreateBankCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Created>> Handle(
        CreateBankCommand command,
        CancellationToken cancellationToken
    )
    {
        var codeExists = await _context.Banks.AnyAsync(
            x => x.Code == command.Code,
            cancellationToken
        );

        if (codeExists)
            return BankErrors.CodeAlreadyExists;

        var nameExists = await _context.Banks.AnyAsync(
            x => x.Name == command.Name,
            cancellationToken
        );

        if (nameExists)
            return BankErrors.NameAlreadyExists;

        var accountNoExists = await _context.Banks.AnyAsync(
            x => x.AccountNo == command.AccountNo,
            cancellationToken
        );

        if (accountNoExists)
            return BankErrors.AccountNoAlreadyExists;

        var createResult = Bank.Create(command.Code, command.Name, command.AccountNo);

        if (createResult.IsError)
            return createResult.Errors;

        var entity = createResult.Value;
        _context.Banks.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Created;
    }
}
