using EmployeesManager.Application.Common.Models;
using EmployeesManager.Application.Features.Departments.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Departments.Queries.GetAllDepartments;

public sealed record GetDepartmentsQuery(string? SearchTerm, int Page, int PageSize)
    : IRequest<Result<PaginatedList<DepartmentWithoutEmployeesDto>>>;
