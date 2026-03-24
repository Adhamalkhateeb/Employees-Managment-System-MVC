using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Employees;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeesManager.Application.Features.Employees.Commands.DeleteEmployee;

public sealed class DeleteEmployeeCommandHandler
    : IRequestHandler<DeleteEmployeeCommand, Result<Deleted>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<DeleteEmployeeCommandHandler> _logger;

    public DeleteEmployeeCommandHandler(
        IAppDbContext context,
        ILogger<DeleteEmployeeCommandHandler> logger
    )
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Deleted>> Handle(
        DeleteEmployeeCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.Employees.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
        {
            _logger.LogWarning("Attempt to delete non-existent employee: {Id}", command.Id);
            return EmployeeErrors.NotFound(command.Id);
        }

        _context.Employees.Remove(entity);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Employee deleted successfully: {Id}", command.Id);
        return Result.Deleted;
    }
}
