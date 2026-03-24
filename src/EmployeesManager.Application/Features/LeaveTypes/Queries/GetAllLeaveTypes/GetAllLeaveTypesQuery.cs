using EmployeesManager.Application.Features.LeaveTypes.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.LeaveTypes.Queries.GetAllLeaveTypes;

public sealed record GetAllLeaveTypesQuery() : IRequest<Result<List<LeaveTypeDto>>>;
