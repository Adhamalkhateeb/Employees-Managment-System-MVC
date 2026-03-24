using EmployeesManager.Application.Features.LeaveTypes.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.LeaveTypes.Queries.GetLeaveTypeById;

public sealed record GetLeaveTypeByIdQuery(Guid Id) : IRequest<Result<LeaveTypeDto>>;
