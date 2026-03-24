using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Banks;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EmployeesManager.Application.Features.Banks.Commands.DeleteBank;

public sealed class DeleteBankCommandHandler : IRequestHandler<DeleteBankCommand, Result<Deleted>>
{
    private readonly IAppDbContext _context;

    public DeleteBankCommandHandler(IAppDbContext context) => _context = context;

    public async Task<Result<Deleted>> Handle(
        DeleteBankCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.Banks.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
            return BankErrors.NotFound(command.Id);

        _context.Banks.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Deleted;
    }
}
