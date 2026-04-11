using EmployeesManager.Application.Features.Employees.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Employees.Queries.GetEmployeeById;

public sealed record GetEmployeeByIdQuery(Guid Id) : IRequest<Result<EmployeeDto>>;
