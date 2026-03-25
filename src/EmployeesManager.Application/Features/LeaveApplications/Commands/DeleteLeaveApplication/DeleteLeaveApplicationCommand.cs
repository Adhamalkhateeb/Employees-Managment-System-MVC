using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.LeaveApplications.Commands.DeleteLeaveApplication;

public sealed record DeleteLeaveApplicationCommand(Guid Id) : IRequest<Result<Deleted>>;
