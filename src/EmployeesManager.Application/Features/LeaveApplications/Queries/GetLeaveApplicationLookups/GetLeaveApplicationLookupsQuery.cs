using EmployeesManager.Application.Features.LeaveApplications.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.LeaveApplications.Queries.GetLeaveApplicationLookups;

public sealed record GetLeaveApplicationLookupsQuery : IRequest<Result<LeaveApplicationLookupDto>>;
