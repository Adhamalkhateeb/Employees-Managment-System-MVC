using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Employees;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeesManager.Application.Features.Employees.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandHandler
    : IRequestHandler<CreateEmployeeCommand, Result<Created>>
{
    private readonly IAppDbContext _context;
    private readonly ILogger<CreateEmployeeCommandHandler> _logger;

    public CreateEmployeeCommandHandler(
        IAppDbContext context,
        ILogger<CreateEmployeeCommandHandler> logger
    )
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<Created>> Handle(
        CreateEmployeeCommand command,
        CancellationToken cancellationToken
    )
    {
        var normalizedEmail = command.EmailAddress.Trim().ToUpperInvariant();

        var emailExists = await _context.Employees.AnyAsync(
            x => x.EmailAddress.ToUpper() == normalizedEmail,
            cancellationToken
        );

        if (emailExists)
        {
            _logger.LogWarning(
                "Attempt to create employee with existing email: {Email}",
                command.EmailAddress
            );
            return EmployeeErrors.EmailAddressAlreadyExists;
        }

        var phoneNumberExists = await _context.Employees.AnyAsync(
            x => x.PhoneNumber == command.PhoneNumber,
            cancellationToken
        );

        if (phoneNumberExists)
        {
            _logger.LogWarning(
                "Attempt to create employee with existing phone number: {PhoneNumber}",
                command.PhoneNumber
            );
            return EmployeeErrors.PhoneNumberAlreadyExists;
        }

        var nationalIdExists = await _context.Employees.AnyAsync(
            x => x.NationalId == command.NationalId,
            cancellationToken
        );

        if (nationalIdExists)
        {
            _logger.LogWarning(
                "Attempt to create employee with existing national id: {NationalId}",
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
                "Attempt to create employee with unknown department: {DepartmentId}",
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
                    "Attempt to create employee with unknown branch: {BranchId}",
                    command.BranchId.Value
                );
                return EmployeeErrors.BranchNotFound;
            }
        }

        var createResult = Employee.Create(
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

        if (createResult.IsError)
        {
            _logger.LogWarning(
                "Failed to create employee: {Errors}",
                string.Join(", ", createResult.Errors)
            );
            return createResult.Errors;
        }

        var entity = createResult.Value;
        _context.Employees.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return Result.Created;
    }
}
