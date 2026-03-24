using EmployeesManager.Application.Common.Interfaces;
using EmployeesManager.Application.Features.Employees.Dtos;
using EmployeesManager.Application.Features.Employees.Mappings;
using EmployeesManager.Domain.Common.Results;
using EmployeesManager.Domain.Entities.Employees;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmployeesManager.Application.Features.Employees.Commands.CreateEmployee;

public sealed class CreateEmployeeCommandHandler
    : IRequestHandler<CreateEmployeeCommand, Result<EmployeeDto>>
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

    public async Task<Result<EmployeeDto>> Handle(
        CreateEmployeeCommand command,
        CancellationToken cancellationToken
    )
    {
        var normalizedEmail = command.EmailAddress.Trim().ToUpper();

        var emailExists = await _context.Employees.AnyAsync(
            x => x.EmailAddress == normalizedEmail,
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

        var createResult = Employee.Create(
            command.FirstName,
            command.MiddleName,
            command.LastName,
            command.PhoneNumber,
            command.EmailAddress,
            command.Country,
            command.DateOfBirth,
            command.Address,
            command.Department,
            command.Designation
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

        return entity.ToDto();
    }
}
