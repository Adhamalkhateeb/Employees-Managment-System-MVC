using EmployeesManager.Application.Features.Employees.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Employees.Queries.GetAllEmployees;

public sealed record GetAllEmployeesQuery() : IRequest<Result<List<EmployeeDto>>>;
