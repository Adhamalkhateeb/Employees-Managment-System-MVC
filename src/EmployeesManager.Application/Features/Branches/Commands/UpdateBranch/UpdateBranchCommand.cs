using EmployeesManager.Application.Features.Branches.Common;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Branches.Commands.UpdateBranch;

public sealed record UpdateBranchCommand(
    Guid Id,
    string Name,
    string Address,
    string PhoneNumber,
    string EmailAddress,
    Guid? ManagerId
) : IRequest<Result<Updated>>, IBranchCommand;
