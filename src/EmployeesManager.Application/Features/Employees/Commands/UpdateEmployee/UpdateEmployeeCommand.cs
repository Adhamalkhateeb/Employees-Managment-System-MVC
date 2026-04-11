using EmployeesManager.Application.Features.Employees.Common;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Employees.Commands.UpdateEmployee;

public sealed record UpdateEmployeeCommand(
    Guid Id,
    string FirstName,
    string? MiddleName,
    string LastName,
    string PhoneNumber,
    string EmailAddress,
    DateTime DateOfBirth,
    string Address,
    Guid CountryId,
    Guid DepartmentId,
    Guid DesignationId
) : IRequest<Result<Updated>>, IEmployeeCommand;
