using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.CancelLeaveApplication;

public sealed record CancelLeaveApplicationCommand(Guid Id) : IRequest<Result<Success>>;
