using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Employees.Commands.DeleteEmployee;

public sealed record DeleteEmployeeCommand(Guid Id) : IRequest<Result<Deleted>>;
