using EmployeesManager.Application.Features.LeaveApplications.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.LeaveApplications.Queries.GetLeaveApplicationById;

public sealed record GetLeaveApplicationByIdQuery(Guid Id) : IRequest<Result<LeaveApplicationDto>>;
