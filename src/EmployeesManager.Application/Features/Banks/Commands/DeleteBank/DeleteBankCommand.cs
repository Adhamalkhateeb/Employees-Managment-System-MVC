using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Banks.Commands.DeleteBank;

public sealed record DeleteBankCommand(Guid Id) : IRequest<Result<Deleted>>;
