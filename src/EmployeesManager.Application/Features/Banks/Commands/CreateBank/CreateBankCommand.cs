using EmployeesManager.Application.Features.Banks.Common;
using EmployeesManager.Application.Features.Banks.Dtos;
using EmployeesManager.Domain.Common.Results;
using MediatR;

namespace EmployeesManager.Application.Features.Banks.Commands.CreateBank;

public sealed record CreateBankCommand(string Code, string Name, string AccountNo)
    : IRequest<Result<Created>>,
        IBankCommand;
