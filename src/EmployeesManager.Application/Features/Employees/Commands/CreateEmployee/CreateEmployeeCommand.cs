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
    DateTime DateOfBirth,
    string Address,
    Guid CountryId,
    Guid DepartmentId,
    Guid DesignationId
) : IRequest<Result<Created>>, IEmployeeCommand;
