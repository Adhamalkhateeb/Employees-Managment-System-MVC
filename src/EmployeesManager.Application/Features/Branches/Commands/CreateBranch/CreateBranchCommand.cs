using EmployeesManager.Application.Features.Branches.Common;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Branches.Commands.CreateBranch;

public sealed record CreateBranchCommand(
    string Name,
    string Address,
    string PhoneNumber,
    string EmailAddress,
    Guid? ManagerId
) : IRequest<Result<Created>>, IBranchCommand;
