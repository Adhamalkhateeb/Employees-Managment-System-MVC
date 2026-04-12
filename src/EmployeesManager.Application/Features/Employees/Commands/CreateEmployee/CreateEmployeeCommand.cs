using EmployeesManager.Application.Features.Employees.Common;
using EmployeesManager.Application.Features.Employees.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Employees.Commands.CreateEmployee;

public sealed record CreateEmployeeCommand(
    string FirstName,
    string LastName,
    string NationalId,
    string PhoneNumber,
    string EmailAddress,
    DateTime HireDate,
    string Address,
    Guid DepartmentId,
    Guid? BranchId
) : IRequest<Result<Created>>, IEmployeeCommand;
