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
    string Country,
    DateTime DateOfBirth,
    string Address,
    string Department,
    string Designation
) : IRequest<Result<Updated>>, IEmployeeCommand;
