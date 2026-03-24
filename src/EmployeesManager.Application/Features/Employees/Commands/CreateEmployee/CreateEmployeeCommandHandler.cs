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

        var countryExists = await _context.Countries.AnyAsync(
            x => x.Id == command.CountryId,
            cancellationToken
        );

        if (!countryExists)
        {
            _logger.LogWarning(
                "Attempt to create employee with unknown country: {CountryId}",
                command.CountryId
            );
            return EmployeeErrors.CountryNotFound;
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

        var designationExists = await _context.Designations.AnyAsync(
            x => x.Id == command.DesignationId,
            cancellationToken
        );

        if (!designationExists)
        {
            _logger.LogWarning(
                "Attempt to create employee with unknown designation: {DesignationId}",
                command.DesignationId
            );
            return EmployeeErrors.DesignationNotFound;
        }

        var createResult = Employee.Create(
            command.FirstName,
            command.MiddleName,
            command.LastName,
            command.PhoneNumber,
            command.EmailAddress,
            command.DateOfBirth,
            command.Address,
            command.CountryId,
            command.DepartmentId,
            command.DesignationId
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
