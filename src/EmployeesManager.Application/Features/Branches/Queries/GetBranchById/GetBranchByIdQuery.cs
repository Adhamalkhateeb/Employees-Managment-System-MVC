using EmployeesManager.Application.Features.Branches.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Branches.Queries.GetBranchById;

public sealed record GetBranchByIdQuery(Guid Id) : IRequest<Result<BranchDto>>;
