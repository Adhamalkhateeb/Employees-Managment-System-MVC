using EmployeesManager.Application.Features.LeaveApplications.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.LeaveApplications.Queries.GetAllLeaveApplications;

public sealed record GetAllLeaveApplicationsQuery() : IRequest<Result<List<LeaveApplicationDto>>>;
