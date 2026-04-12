using EmployeesManager.Application.Features.Employees.Common;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Employees.Commands.UpdateEmployee;

public sealed record UpdateEmployeeCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string NationalId,
    string PhoneNumber,
    string EmailAddress,
    DateTime HireDate,
    string Address,
    Guid DepartmentId,
    Guid? BranchId
) : IRequest<Result<Updated>>, IEmployeeCommand;
