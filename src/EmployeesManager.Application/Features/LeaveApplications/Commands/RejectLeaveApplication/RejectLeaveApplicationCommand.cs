using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.RejectLeaveApplication;

public sealed record RejectLeaveApplicationCommand(Guid Id, string RejectionReason)
    : IRequest<Result<Success>>;
