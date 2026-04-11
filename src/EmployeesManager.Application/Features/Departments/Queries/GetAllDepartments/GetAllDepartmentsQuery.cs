using EmployeesManager.Application.Features.Departments.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Departments.Queries.GetAllDepartments;

public sealed record GetAllDepartmentsQuery() : IRequest<Result<List<DepartmentDto>>>;
