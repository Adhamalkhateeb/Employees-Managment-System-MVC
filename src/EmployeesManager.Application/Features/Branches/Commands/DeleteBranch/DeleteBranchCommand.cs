using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Branches.Commands.DeleteBranch;

public sealed record DeleteBranchCommand(Guid Id) : IRequest<Result<Deleted>>;
