using EmployeesManager.Application.Features.Banks.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Banks.Queries.GetBankById;

public sealed record GetBankByIdQuery(Guid Id) : IRequest<Result<BankDto>>;
