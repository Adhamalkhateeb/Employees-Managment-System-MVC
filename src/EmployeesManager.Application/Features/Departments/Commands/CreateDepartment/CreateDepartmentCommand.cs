using EmployeesManager.Application.Features.Departments.Common;
using EmployeesManager.Application.Features.Departments.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Departments.Commands.CreateDepartment;

public sealed record CreateDepartmentCommand(string Name, Guid? ManagerId)
    : IRequest<Result<Created>>,
        IDepartmentCommand;
