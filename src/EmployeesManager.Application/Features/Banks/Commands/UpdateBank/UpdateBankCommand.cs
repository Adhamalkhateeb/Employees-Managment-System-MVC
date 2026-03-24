using EmployeesManager.Application.Features.Banks.Common;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Banks.Commands.UpdateBank;

public sealed record UpdateBankCommand(Guid Id, string Code, string Name, string AccountNo)
    : IRequest<Result<Updated>>,
        IBankCommand;
