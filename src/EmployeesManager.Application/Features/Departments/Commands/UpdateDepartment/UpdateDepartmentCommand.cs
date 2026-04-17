using EmployeesManager.Application.Features.Departments.Common;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Departments.Commands.UpdateDepartment;

public sealed record UpdateDepartmentCommand(Guid Id, string Name, string Code)
    : IRequest<Result<Updated>>,
        IDepartmentCommand;
