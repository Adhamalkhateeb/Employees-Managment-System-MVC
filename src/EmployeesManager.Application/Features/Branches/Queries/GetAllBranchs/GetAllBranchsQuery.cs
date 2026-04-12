using EmployeesManager.Application.Features.Branches.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Branches.Queries.GetAllBranchs;

public sealed record GetBranchesQuery() : IRequest<Result<List<BranchDto>>>;
