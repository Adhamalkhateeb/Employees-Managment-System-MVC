using EmployeesManager.Application.Features.Banks.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Banks.Queries.GetAllBanks;

public sealed record GetAllBanksQuery() : IRequest<Result<List<BankDto>>>;
