using EmployeesManager.Application.Features.Employees.Common;
using EmployeesManager.Application.Features.Employees.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Employees.Commands.CreateEmployee;

public sealed record CreateEmployeeCommand(
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
) : IRequest<Result<EmployeeDto>>, IEmployeeCommand;
