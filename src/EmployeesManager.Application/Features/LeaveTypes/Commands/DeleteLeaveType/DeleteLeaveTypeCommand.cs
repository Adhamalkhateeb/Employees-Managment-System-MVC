using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.LeaveTypes.Commands.DeleteLeaveType;

public sealed record DeleteLeaveTypeCommand(Guid Id) : IRequest<Result<Deleted>>;
