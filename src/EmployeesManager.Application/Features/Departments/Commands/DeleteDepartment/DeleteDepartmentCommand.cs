using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Departments.Commands.DeleteDepartment;

public sealed record DeleteDepartmentCommand(Guid Id) : IRequest<Result<Deleted>>;
