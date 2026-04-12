using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Employees;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeesManager.Application.Features.Employees.Commands.UpdateEmployee;

public sealed class UpdateEmployeeCommandHandler
    : IRequestHandler<UpdateEmployeeCommand, Result<Updated>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<UpdateEmployeeCommandHandler> _logger;

    public UpdateEmployeeCommandHandler(
        IAppDbContext context,
        ILogger<UpdateEmployeeCommandHandler> logger
    )
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Updated>> Handle(
        UpdateEmployeeCommand command,
        CancellationToken cancellationToken
    )
    {
        var entity = await _context.Employees.FirstOrDefaultAsync(
            x => x.Id == command.Id,
            cancellationToken
        );

        if (entity is null)
        {
            _logger.LogWarning("Attempt to update non-existent employee: {Id}", command.Id);
            return EmployeeErrors.NotFound(command.Id);
        }

        var normalizedEmail = command.EmailAddress.Trim().ToUpperInvariant();
        var emailExists = await _context.Employees.AnyAsync(
            x => x.EmailAddress.ToUpper() == normalizedEmail && x.Id != command.Id,
            cancellationToken
        );

        if (emailExists)
        {
            _logger.LogWarning(
                "Attempt to update employee with existing email: {Email}",
                command.EmailAddress
            );
            return EmployeeErrors.EmailAddressAlreadyExists;
        }

        var phoneNumberExists = await _context.Employees.AnyAsync(
            x => x.PhoneNumber == command.PhoneNumber && x.Id != command.Id,
            cancellationToken
        );

        if (phoneNumberExists)
        {
            _logger.LogWarning(
                "Attempt to update employee with existing phone number: {PhoneNumber}",
                command.PhoneNumber
            );
            return EmployeeErrors.PhoneNumberAlreadyExists;
        }

        var nationalIdExists = await _context.Employees.AnyAsync(
            x => x.NationalId == command.NationalId && x.Id != command.Id,
            cancellationToken
        );

        if (nationalIdExists)
        {
            _logger.LogWarning(
                "Attempt to update employee with existing national id: {NationalId}",
                command.NationalId
            );
            return EmployeeErrors.NationalIdAlreadyExists;
        }

        var departmentExists = await _context.Departments.AnyAsync(
            x => x.Id == command.DepartmentId,
            cancellationToken
        );

        if (!departmentExists)
        {
            _logger.LogWarning(
                "Attempt to update employee with unknown department: {DepartmentId}",
                command.DepartmentId
            );
            return EmployeeErrors.DepartmentNotFound;
        }

        if (command.BranchId.HasValue)
        {
            var branchExists = await _context.Branches.AnyAsync(
                x => x.Id == command.BranchId.Value,
                cancellationToken
            );

            if (!branchExists)
            {
                _logger.LogWarning(
                    "Attempt to update employee with unknown branch: {BranchId}",
                    command.BranchId.Value
                );
                return EmployeeErrors.BranchNotFound;
            }
        }

        var updateResult = entity.Update(
            command.FirstName,
            command.LastName,
            command.NationalId,
            command.PhoneNumber,
            command.EmailAddress,
            command.HireDate,
            command.Address,
            command.DepartmentId,
            command.BranchId
        );

        if (updateResult.IsError)
        {
            _logger.LogWarning(
                "Failed to update employee: {Id}, Errors: {Errors}",
                command.Id,
                string.Join(", ", updateResult.Errors)
            );
            return updateResult.Errors;
        }

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Employee updated successfully: {Id}", command.Id);
        return Result.Updated;
    }
}
