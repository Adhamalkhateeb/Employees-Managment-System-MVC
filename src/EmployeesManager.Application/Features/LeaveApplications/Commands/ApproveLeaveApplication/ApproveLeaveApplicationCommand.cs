using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.ApproveLeaveApplication;

public sealed record ApproveLeaveApplicationCommand(Guid Id) : IRequest<Result<Success>>;
